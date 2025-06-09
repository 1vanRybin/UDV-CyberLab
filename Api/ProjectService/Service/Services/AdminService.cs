using AutoMapper;
using CRM.Data.Common.Exceptions;
using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Service.Interfaces;

namespace Service.Services
{
    public class AdminService : IAdminService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;
        private readonly IRatingRepository _ratingRepository;

        public AdminService(
            IProjectRepository projectRepository,
            ICommentRepository commentRepository,
            IRatingRepository ratingRepository,
            IMapper mapper)
        {
            _projectRepository = projectRepository;
            _commentRepository = commentRepository;
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        public async Task<AdminDashboardDto> GetDashboardAsync()
        {
            var projects = await _projectRepository.GetAllAsync<ProjectCard>();

            var projectCommentCounts = new Dictionary<Guid, int>();
            foreach (var project in projects)
            {
                var comments = await _commentRepository.GetCommentsByProjectIdAsync(project.Id);
                projectCommentCounts[project.Id] = comments.Count;
            }

            var projectStatistics = new List<ProjectStatisticsDto>();
            foreach (var project in projects)
            {
                var ratingCount = await _ratingRepository.GetRatingCountForProjectAsync(project.Id);

                projectStatistics.Add(new ProjectStatisticsDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    ViewsCount = project.ViewsCount,
                    LandingVisits = project.ViewsCount,
                    Rating = project.Rating,
                    TotalRatings = ratingCount,
                    CommentsCount = projectCommentCounts.GetValueOrDefault(project.Id)
                });
            }

            return new AdminDashboardDto
            {
                TotalProjects = projects.Count,
                TotalComments = projectCommentCounts.Values.Sum(),
                TotalViews = projects.Sum(p => p.ViewsCount),
                ProjectsStatistics = projectStatistics
            };
        }

        public async Task<ProjectStatisticsDto> GetProjectStatisticsAsync(Guid projectId)
        {
            var project = await _projectRepository.GetByIdAsync<ProjectCard>(projectId);
            if (project == null)
            {
                throw new NotFoundException($"Проект с ID {projectId} не найден");
            }

            var comments = await _commentRepository.GetCommentsByProjectIdAsync(projectId);
            var ratingCount = await _ratingRepository.GetRatingCountForProjectAsync(projectId);

            return new ProjectStatisticsDto
            {
                Id = project.Id,
                Name = project.Name,
                ViewsCount = project.ViewsCount,
                LandingVisits = project.ViewsCount,
                Rating = project.Rating,
                TotalRatings = ratingCount,
                CommentsCount = comments.Count
            };
        }

        public async Task<bool> DeleteCommentAsync(Guid commentId)
        {
            var comment = await _commentRepository.GetByIdAsync<Comment>(commentId);
            if (comment == null)
            {
                throw new NotFoundException($"Комментарий с ID {commentId} не найден");
            }

            var project = await _projectRepository.GetByIdAsync<ProjectCard>(comment.ProjectId);
            project.CommentsCount--;
            await _projectRepository.UpdateAsync(project);

            await _commentRepository.DeleteAsync(comment);

            return true;
        }

        public async Task<bool> DeleteProjectAsync(Guid projectId)
        {
            var project = await _projectRepository.GetByIdAsync<ProjectCard>(projectId);
            if (project == null)
            {
                throw new NotFoundException($"Проект с ID {projectId} не найден");
            }

            var comments = await _commentRepository.GetCommentsByProjectIdAsync(projectId);
            foreach (var comment in comments)
            {
                await _commentRepository.DeleteAsync(comment);
            }

            var ratings = await _ratingRepository.GetAllRatingsForProjectAsync(projectId);
            foreach (var rating in ratings)
            {
                await _ratingRepository.DeleteAsync(rating);
            }

            await _projectRepository.DeleteAsync(project);
            return true;
        }

        public async Task<List<CommentDto>> GetAllCommentsForModerationAsync()
        {
            var allProjects = await _projectRepository.GetAllAsync<ProjectCard>();
            var allComments = new List<Comment>();

            foreach (var project in allProjects)
            {
                var comments = await _commentRepository.GetCommentsByProjectIdAsync(project.Id);
                allComments.AddRange(comments);
            }

            return _mapper.Map<List<CommentDto>>(allComments);
        }
    }
}