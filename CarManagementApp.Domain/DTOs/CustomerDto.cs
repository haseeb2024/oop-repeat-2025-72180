using System.ComponentModel.DataAnnotations;

namespace CarManagementApp.Domain.DTOs
{
    public class CustomerDto
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
        public bool IsActive { get; set; }
        
        public int CarCount { get; set; }
    }
    
    public class CreateCustomerDto
    {
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
        
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }
    
    public class UpdateCustomerDto
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
    }
} 