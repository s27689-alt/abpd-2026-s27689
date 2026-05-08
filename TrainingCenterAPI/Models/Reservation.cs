using System.ComponentModel.DataAnnotations;

namespace TrainingCenterAPI.Models
{
    public class Reservation : IValidatableObject
    {
        public int Id { get; set; }
        
        [Required]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "OrganizerName is required.")]
        public string OrganizerName { get; set; }

        [Required(ErrorMessage = "Topic is required.")]
        public string Topic { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public string Status { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndTime <= StartTime)
            {
                yield return new ValidationResult(
                    "EndTime must be later than StartTime.",
                    new[] { nameof(EndTime) }
                );
            }
        }
    }
}