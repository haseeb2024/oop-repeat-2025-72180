using System.ComponentModel.DataAnnotations;

namespace CarManagementApp.Domain.Entities
{
    public class Customer
    {
        public string Id { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Phone]
        public string? PhoneNumber { get; set; }
        
        [StringLength(200)]
        public string? Address { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        
        // Navigation properties
        public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
    }
} 