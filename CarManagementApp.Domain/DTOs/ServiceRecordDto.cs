using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarManagementApp.Domain.DTOs
{
    public class ServiceRecordDto
    {
        public int Id { get; set; }
        
        public DateTime ServiceDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        
        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string WorkDescription { get; set; } = string.Empty;
        
        [Required]
        [Range(0.1, 100)]
        public decimal HoursWorked { get; set; }
        
        public bool IsCompleted { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCost { get; set; }
        
        public int CarId { get; set; }
        public string CarRegistrationNumber { get; set; } = string.Empty;
        public string CarMakeModel { get; set; } = string.Empty;
        
        public string MechanicId { get; set; } = string.Empty;
        public string MechanicName { get; set; } = string.Empty;
        public string MechanicEmail { get; set; } = string.Empty;
        
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
    
    public class CreateServiceRecordDto
    {
        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string CarRegistrationNumber { get; set; } = string.Empty;
        
        [Required]
        public DateTime ServiceDate { get; set; }
        
        [Required]
        [StringLength(100)]
        public string MechanicEmail { get; set; } = string.Empty;
    }
    
    public class UpdateServiceRecordDto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [Range(0.1, 100)]
        public decimal HoursWorked { get; set; }
        
        public bool IsCompleted { get; set; }
    }
    
    public class CompleteServiceRecordDto
    {
        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [Range(0.1, 100)]
        public decimal HoursWorked { get; set; }
    }
} 