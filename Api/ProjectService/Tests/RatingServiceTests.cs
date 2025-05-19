using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Service.Services;
using Xunit;

namespace Tests;

public class RatingServiceTests
{
    private readonly Mock<IRatingRepository> _mockRatingRepository;
    private readonly Mock<IProjectRepository> _mockProjectRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly RatingService _ratingService;

    public RatingServiceTests()
    {
        _mockRatingRepository = new Mock<IRatingRepository>();
        _mockProjectRepository = new Mock<IProjectRepository>();
        _mockMapper = new Mock<IMapper>();
        _ratingService = new RatingService(
            _mockRatingRepository.Object,
            _mockProjectRepository.Object,
            _mockMapper.Object
        );
    }

    [Fact]
    public async Task CreateOrUpdateRatingAsync_WithExistingRating_ShouldUpdateRating()
    {
        // Arrange
        Guid projectId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        double ratingValue = 4.0;
        Guid ratingId = Guid.NewGuid();
        
        var ratingCreateDto = new RatingCreateDto
        {
            ProjectId = projectId,
            Value = ratingValue
        };

        var existingRating = new Rating
        {
            Id = ratingId,
            ProjectId = projectId,
            UserId = userId,
            Value = 3.0, // old value
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };
        
        var project = new ProjectCard
        {
            Id = projectId,
            Name = "Test Project",
            Description = "Test Project Description",
            OwnerName = "Test Owner",
            OwnerId = Guid.NewGuid(),
            Rating = 3.0,
            LogoPath = "logo.png",
            LandingURL = "https://example.com",
            DocumentationPath = "/docs"
        };
        
        var expectedRatingDto = new RatingDto
        {
            Id = ratingId,
            ProjectId = projectId,
            UserId = userId,
            Value = ratingValue,
            CreatedAt = existingRating.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        _mockProjectRepository.Setup(repo => repo.GetByIdAsync<ProjectCard>(projectId))
            .ReturnsAsync(project);
            
        _mockRatingRepository.Setup(repo => repo.GetUserRatingForProjectAsync(projectId, userId))
            .ReturnsAsync(existingRating);
            
        _mockRatingRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Rating>()))
            .Returns(Task.CompletedTask);
            
        _mockRatingRepository.Setup(repo => repo.GetAverageRatingForProjectAsync(projectId))
            .ReturnsAsync(ratingValue);
            
        _mockMapper.Setup(mapper => mapper.Map<RatingDto>(It.IsAny<Rating>()))
            .Returns(expectedRatingDto);

        // Act
        var result = await _ratingService.CreateOrUpdateRatingAsync(ratingCreateDto, userId);

        // Assert
        Assert.Equal(expectedRatingDto, result);
        _mockRatingRepository.Verify(repo => repo.UpdateAsync(It.Is<Rating>(r => 
            r.Value == ratingValue && 
            r.UpdatedAt != null)), 
            Times.Once);
        _mockProjectRepository.Verify(repo => repo.UpdateAsync(It.Is<ProjectCard>(p => 
            p.Rating == ratingValue)), 
            Times.Once);
    }

    [Fact]
    public async Task CreateOrUpdateRatingAsync_WithNewRating_ShouldCreateRating()
    {
        // Arrange
        Guid projectId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        double ratingValue = 5.0;
        
        var ratingCreateDto = new RatingCreateDto
        {
            ProjectId = projectId,
            Value = ratingValue
        };
        
        var project = new ProjectCard
        {
            Id = projectId,
            Name = "Test Project",
            Description = "Test Project Description",
            OwnerName = "Test Owner",
            OwnerId = Guid.NewGuid(),
            Rating = 0, // No ratings yet
            LogoPath = "logo.png",
            LandingURL = "https://example.com",
            DocumentationPath = "/docs"
        };
        
        var expectedRatingDto = new RatingDto
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            UserId = userId,
            Value = ratingValue,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        _mockProjectRepository.Setup(repo => repo.GetByIdAsync<ProjectCard>(projectId))
            .ReturnsAsync(project);
            
        _mockRatingRepository.Setup(repo => repo.GetUserRatingForProjectAsync(projectId, userId))
            .ReturnsAsync((Rating)null); // No existing rating
            
        _mockRatingRepository.Setup(repo => repo.CreateAsync(It.IsAny<Guid>(), It.IsAny<Rating>()))
            .Returns(Task.CompletedTask);
            
        _mockRatingRepository.Setup(repo => repo.GetAverageRatingForProjectAsync(projectId))
            .ReturnsAsync(ratingValue);
            
        _mockMapper.Setup(mapper => mapper.Map<RatingDto>(It.IsAny<Rating>()))
            .Returns(expectedRatingDto);

        // Act
        var result = await _ratingService.CreateOrUpdateRatingAsync(ratingCreateDto, userId);

        // Assert
        Assert.Equal(expectedRatingDto, result);
        _mockRatingRepository.Verify(repo => repo.CreateAsync(It.IsAny<Guid>(), It.Is<Rating>(r => 
            r.ProjectId == projectId && 
            r.UserId == userId && 
            r.Value == ratingValue)), 
            Times.Once);
        _mockProjectRepository.Verify(repo => repo.UpdateAsync(It.Is<ProjectCard>(p => 
            p.Rating == ratingValue)), 
            Times.Once);
    }
    
    [Fact]
    public async Task DeleteRatingAsync_WithExistingRating_ShouldDeleteRating()
    {
        // Arrange
        Guid projectId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        Guid ratingId = Guid.NewGuid();
        
        var existingRating = new Rating
        {
            Id = ratingId,
            ProjectId = projectId,
            UserId = userId,
            Value = 4.0,
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };
        
        var project = new ProjectCard
        {
            Id = projectId,
            Name = "Test Project",
            Description = "Test Project Description",
            OwnerName = "Test Owner",
            OwnerId = Guid.NewGuid(),
            Rating = 4.0,
            LogoPath = "logo.png",
            LandingURL = "https://example.com",
            DocumentationPath = "/docs"
        };

        _mockRatingRepository.Setup(repo => repo.GetUserRatingForProjectAsync(projectId, userId))
            .ReturnsAsync(existingRating);
            
        _mockRatingRepository.Setup(repo => repo.DeleteAsync<Rating>(ratingId))
            .Returns(Task.CompletedTask);
            
        _mockProjectRepository.Setup(repo => repo.GetByIdAsync<ProjectCard>(projectId))
            .ReturnsAsync(project);
            
        _mockRatingRepository.Setup(repo => repo.GetAverageRatingForProjectAsync(projectId))
            .ReturnsAsync(0); // No more ratings
            
        // Act
        var result = await _ratingService.DeleteRatingAsync(projectId, userId);

        // Assert
        Assert.True(result);
        _mockRatingRepository.Verify(repo => repo.DeleteAsync<Rating>(ratingId), Times.Once);
        _mockProjectRepository.Verify(repo => repo.UpdateAsync(It.Is<ProjectCard>(p => 
            p.Rating == 0)), 
            Times.Once);
    }
}
