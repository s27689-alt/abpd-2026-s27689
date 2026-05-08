using System.ComponentModel.DataAnnotations;

namespace TrainingCenterAPI.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "BuildingCode is required.")]
        public string BuildingCode { get; set; }

        public int Floor { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than zero.")]
        public int Capacity { get; set; }

        public bool HasProjector { get; set; }
        
        public bool IsActive { get; set; }
    }
}