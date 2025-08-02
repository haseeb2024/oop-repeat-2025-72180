using System.ComponentModel.DataAnnotations;

namespace CarManagementApp.Domain.Entities
{
    public class Car
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(20)]
        public string RegistrationNumber { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Make { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Model { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? Color { get; set; }
        
        public int Year { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public string CustomerId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        
        // Navigation properties
        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<ServiceRecord> ServiceRecords { get; set; } = new List<ServiceRecord>();
    }
} 