using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarManagementApp.Domain.Entities
{
    public class ServiceRecord
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
        public string MechanicId { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        
        // Navigation properties
        public virtual Car Car { get; set; } = null!;
        public virtual Mechanic Mechanic { get; set; } = null!;
    }
} 