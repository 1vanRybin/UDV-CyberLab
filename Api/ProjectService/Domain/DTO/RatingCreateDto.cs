using System.ComponentModel.DataAnnotations;

namespace Domain.DTO
{
    public class RatingCreateDto
    {
        public required Guid ProjectId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating value must be between 1 and 5.")]
        public double Value { get; set; } // Rating value (1-5)
    }
}