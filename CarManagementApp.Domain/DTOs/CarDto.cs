using System.ComponentModel.DataAnnotations;

namespace CarManagementApp.Domain.DTOs
{
    public class CarDto
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
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }
        
        public int ServiceRecordCount { get; set; }
    }
    
    public class CreateCarDto
    {
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
        
        [Required]
        [Range(1900, 2030)]
        public int Year { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;
    }
    
    public class UpdateCarDto
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
        
        [Required]
        [Range(1900, 2030)]
        public int Year { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;
    }
} 