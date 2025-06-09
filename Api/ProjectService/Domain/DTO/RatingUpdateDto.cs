using System.ComponentModel.DataAnnotations;

namespace Domain.DTO
{
    public class RatingUpdateDto
    {
        public required Guid Id { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating value must be between 1 and 5.")]
        public double Value { get; set; }
    }
}