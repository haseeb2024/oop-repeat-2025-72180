using CarManagementApp.Domain.DTOs;
using CarManagementApp.Domain.Entities;
using CarManagementAppRun.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CarManagementAppRun.Services
{
    public class CustomerService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<CustomerDto>> GetAllCustomersAsync()
        {
            return await _context.Customers
                .Where(c => c.IsActive)
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber ?? "",
                    Address = c.Address ?? "",
                    CreatedAt = c.CreatedAt,
                    IsActive = c.IsActive,
                    CarCount = c.Cars.Count(car => car.IsActive)
                })
                .ToListAsync();
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(string id)
        {
            var customer = await _context.Customers
                .Include(c => c.Cars)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            if (customer == null)
                return null;

            return new CustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber ?? "",
                Address = customer.Address ?? "",
                CreatedAt = customer.CreatedAt,
                IsActive = customer.IsActive,
                CarCount = customer.Cars.Count(car => car.IsActive)
            };
        }

        public async Task<CustomerDto?> GetCustomerByEmailAsync(string email)
        {
            var customer = await _context.Customers
                .Include(c => c.Cars)
                .FirstOrDefaultAsync(c => c.Email == email && c.IsActive);

            if (customer == null)
                return null;

            return new CustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber ?? "",
                Address = customer.Address ?? "",
                CreatedAt = customer.CreatedAt,
                IsActive = customer.IsActive,
                CarCount = customer.Cars.Count(car => car.IsActive)
            };
        }

        public async Task<bool> CreateCustomerAsync(CreateCustomerDto createDto)
        {
            if (await _context.Customers.AnyAsync(c => c.Email == createDto.Email) ||
                await _userManager.FindByEmailAsync(createDto.Email) != null)
                return false;

            var applicationUser = new ApplicationUser
            {
                UserName = createDto.Email,
                Email = createDto.Email,
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                EmailConfirmed = true,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            var userResult = await _userManager.CreateAsync(applicationUser, createDto.Password);
            if (!userResult.Succeeded)
                return false;

            await _userManager.AddToRoleAsync(applicationUser, "Customer");

            var customer = new Customer
            {
                Id = applicationUser.Id,
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                Email = createDto.Email,
                PhoneNumber = createDto.PhoneNumber,
                Address = createDto.Address,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCustomerAsync(string id, UpdateCustomerDto updateDto)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            if (customer == null)
                return false;

            var applicationUser = await _userManager.FindByIdAsync(id);
            if (applicationUser != null)
            {
                applicationUser.FirstName = updateDto.FirstName;
                applicationUser.LastName = updateDto.LastName;
                applicationUser.Email = updateDto.Email;
                applicationUser.UserName = updateDto.Email;
                applicationUser.UpdatedAt = DateTime.Now;

                var userResult = await _userManager.UpdateAsync(applicationUser);
                if (!userResult.Succeeded)
                    return false;
            }

            customer.FirstName = updateDto.FirstName;
            customer.LastName = updateDto.LastName;
            customer.Email = updateDto.Email;
            customer.PhoneNumber = updateDto.PhoneNumber;
            customer.Address = updateDto.Address;
            customer.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCustomerAsync(string id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            if (customer == null)
                return false;

            var applicationUser = await _userManager.FindByIdAsync(id);
            if (applicationUser != null)
            {
                applicationUser.IsActive = false;
                applicationUser.UpdatedAt = DateTime.Now;
                await _userManager.UpdateAsync(applicationUser);
            }

            customer.IsActive = false;
            customer.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CustomerDto>> GetActiveCustomersForDropdownAsync()
        {
            return await _context.Customers
                .Where(c => c.IsActive)
                .OrderBy(c => c.FirstName)
                .ThenBy(c => c.LastName)
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber ?? "",
                    Address = c.Address ?? "",
                    CreatedAt = c.CreatedAt,
                    IsActive = c.IsActive,
                    CarCount = c.Cars.Count(car => car.IsActive)
                })
                .ToListAsync();
        }

        public async Task<bool> TestDatabaseConnectionAsync()
        {
            try
            {
                var customerCount = await _context.Customers.CountAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> GetCustomerCountFromDatabaseAsync()
        {
            return await _context.Customers.Where(c => c.IsActive).CountAsync();
        }
    }
} 