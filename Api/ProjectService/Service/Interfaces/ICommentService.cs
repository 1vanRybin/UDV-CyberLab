using Domain.DTO;

namespace Service.Interfaces;

public interface ICommentService
{
    Task<CommentDto> CreateAsync(CommentCreateDto commentDto, Guid authorId);
    Task<List<CommentDto>> GetCommentsByProjectIdAsync(Guid projectId);
    Task<CommentDto> UpdateAsync(CommentUpdateDto updateDto, Guid userId);
    Task<bool> DeleteAsync(Guid commentId, Guid userId);
}