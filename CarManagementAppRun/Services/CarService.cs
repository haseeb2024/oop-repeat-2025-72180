using CarManagementApp.Domain.DTOs;
using CarManagementApp.Domain.Entities;
using CarManagementAppRun.Data;
using Microsoft.EntityFrameworkCore;

namespace CarManagementAppRun.Services
{
    public class CarService
    {
        private readonly ApplicationDbContext _context;

        public CarService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CarDto>> GetAllCarsAsync()
        {
            return await _context.Cars
                .Include(c => c.Customer)
                .Include(c => c.ServiceRecords)
                .Where(c => c.IsActive)
                .Select(c => new CarDto
                {
                    Id = c.Id,
                    RegistrationNumber = c.RegistrationNumber,
                    Make = c.Make,
                    Model = c.Model,
                    Color = c.Color ?? "",
                    Year = c.Year,
                    CustomerId = c.CustomerId,
                    CustomerName = $"{c.Customer.FirstName} {c.Customer.LastName}",
                    CustomerEmail = c.Customer.Email,
                    CreatedAt = c.CreatedAt,
                    RegistrationDate = c.RegistrationDate,
                    IsActive = c.IsActive,
                    ServiceRecordCount = c.ServiceRecords.Count(sr => sr.IsActive)
                })
                .ToListAsync();
        }

        public async Task<CarDto?> GetCarByIdAsync(int id)
        {
            var car = await _context.Cars
                .Include(c => c.Customer)
                .Include(c => c.ServiceRecords)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            if (car == null)
                return null;

            return new CarDto
            {
                Id = car.Id,
                RegistrationNumber = car.RegistrationNumber,
                Make = car.Make,
                Model = car.Model,
                Color = car.Color ?? "",
                Year = car.Year,
                CustomerId = car.CustomerId,
                CustomerName = $"{car.Customer.FirstName} {car.Customer.LastName}",
                CustomerEmail = car.Customer.Email,
                CreatedAt = car.CreatedAt,
                RegistrationDate = car.RegistrationDate,
                IsActive = car.IsActive,
                ServiceRecordCount = car.ServiceRecords.Count(sr => sr.IsActive)
            };
        }

        public async Task<bool> CreateCarAsync(CreateCarDto createDto)
        {
            if (await _context.Cars.AnyAsync(c => c.RegistrationNumber == createDto.RegistrationNumber))
                return false;

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == createDto.CustomerEmail);
            if (customer == null)
                return false;

            var car = new Car
            {
                RegistrationNumber = createDto.RegistrationNumber,
                Make = createDto.Make,
                Model = createDto.Model,
                Color = createDto.Color,
                Year = createDto.Year,
                CustomerId = customer.Id,
                CreatedAt = DateTime.Now,
                RegistrationDate = DateTime.Now,
                IsActive = true
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCarAsync(int id, UpdateCarDto updateDto)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            if (car == null)
                return false;

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == updateDto.CustomerEmail);
            if (customer == null)
                return false;

            car.RegistrationNumber = updateDto.RegistrationNumber;
            car.Make = updateDto.Make;
            car.Model = updateDto.Model;
            car.Color = updateDto.Color;
            car.Year = updateDto.Year;
            car.CustomerId = customer.Id;
            car.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            if (car == null)
                return false;

            car.IsActive = false;
            car.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CarDto>> GetActiveCarsForDropdownAsync()
        {
            return await _context.Cars
                .Include(c => c.Customer)
                .Where(c => c.IsActive)
                .OrderBy(c => c.RegistrationNumber)
                .Select(c => new CarDto
                {
                    Id = c.Id,
                    RegistrationNumber = c.RegistrationNumber,
                    Make = c.Make,
                    Model = c.Model,
                    Color = c.Color ?? "",
                    Year = c.Year,
                    CustomerId = c.CustomerId,
                    CustomerName = $"{c.Customer.FirstName} {c.Customer.LastName}",
                    CustomerEmail = c.Customer.Email,
                    CreatedAt = c.CreatedAt,
                    RegistrationDate = c.RegistrationDate,
                    IsActive = c.IsActive,
                    ServiceRecordCount = c.ServiceRecords.Count(sr => sr.IsActive)
                })
                .ToListAsync();
        }
    }
} 