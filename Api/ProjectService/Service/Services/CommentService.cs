using AutoMapper;
using CRM.Data.Common.Exceptions;
using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Service.Interfaces;

namespace Service.Services;

public class CommentService(
    ICommentRepository _commentRepository,
    IProjectRepository _projectRepository,
    IMapper _mapper)
    : ICommentService
{
    public async Task<CommentDto> CreateAsync(CommentCreateDto commentDto, Guid authorId)
    {
        var project = await _projectRepository.GetByIdAsync<ProjectCard>(commentDto.ProjectId);
        if (project == null)
        {
            throw new NotFoundException($"Project with id {commentDto.ProjectId} not found.");
        }

        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            Text = commentDto.Text,
            AuthorId = authorId,
            AuthorName = commentDto.UserName,
            ProjectId = commentDto.ProjectId,
            CreatedAt = DateTime.UtcNow
        };

        project.CommentsCount++;
        await _projectRepository.UpdateAsync(project);

        await _commentRepository.CreateAsync(comment.Id, comment);

        return _mapper.Map<CommentDto>(comment);
    }

    public async Task<List<CommentDto>> GetCommentsByProjectIdAsync(Guid projectId)
    {
        var project = await _projectRepository.GetByIdAsync<ProjectCard>(projectId);
        if (project == null)
        {
            throw new NotFoundException($"Project with id {projectId} not found.");
        }

        var comments = await _commentRepository.GetCommentsByProjectIdAsync(projectId);
        return _mapper.Map<List<CommentDto>>(comments);
    }

    public async Task<CommentDto> UpdateAsync(CommentUpdateDto updateDto, Guid userId)
    {
        var comment = await _commentRepository.GetByIdAsync<Comment>(updateDto.Id);
        if (comment == null)
        {
            throw new NotFoundException($"Comment with id {updateDto.Id} not found.");
        }

        if (comment.AuthorId != userId)
        {
            throw new NoAccessException("You are not authorized to update this comment.");
        }

        comment.Text = updateDto.Text;

        await _commentRepository.UpdateAsync(comment);

        return _mapper.Map<CommentDto>(comment);
    }

    public async Task<bool> DeleteAsync(Guid commentId, Guid userId)
    {
        var comment = await _commentRepository.GetByIdAsync<Comment>(commentId);
        if (comment == null)
        {
            throw new NotFoundException($"Comment with id {commentId} not found.");
        }

        if (comment.AuthorId != userId)
        {
            throw new NoAccessException("You are not authorized to delete this comment.");
        }
        var project = await _projectRepository.GetByIdAsync<ProjectCard>(comment.ProjectId);
        project.CommentsCount--;
        await _projectRepository.UpdateAsync(project);

        await _commentRepository.DeleteAsync(comment);

        return true;
    }
}