using CarManagementApp.Domain.DTOs;
using CarManagementApp.Domain.Entities;
using CarManagementAppRun.Data;
using Microsoft.EntityFrameworkCore;

namespace CarManagementAppRun.Services
{
    public class ServiceRecordService
    {
        private readonly ApplicationDbContext _context;

        public ServiceRecordService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceRecordDto>> GetAllServiceRecordsAsync()
        {
            return await _context.ServiceRecords
                .Include(sr => sr.Car)
                .ThenInclude(c => c.Customer)
                .Include(sr => sr.Mechanic)
                .Where(sr => sr.IsActive)
                .Select(sr => new ServiceRecordDto
                {
                    Id = sr.Id,
                    ServiceDate = sr.ServiceDate,
                    CompletionDate = sr.CompletionDate,
                    Description = sr.Description,
                    WorkDescription = sr.WorkDescription,
                    HoursWorked = sr.HoursWorked,
                    IsCompleted = sr.IsCompleted,
                    TotalCost = sr.TotalCost,
                    CarId = sr.CarId,
                    CarRegistrationNumber = sr.Car.RegistrationNumber,
                    CarMakeModel = $"{sr.Car.Make} {sr.Car.Model}",
                    MechanicId = sr.MechanicId,
                    MechanicName = $"{sr.Mechanic.FirstName} {sr.Mechanic.LastName}",
                    MechanicEmail = sr.Mechanic.Email,
                    CustomerId = sr.Car.CustomerId,
                    CustomerName = $"{sr.Car.Customer.FirstName} {sr.Car.Customer.LastName}",
                    CustomerEmail = sr.Car.Customer.Email,
                    CreatedAt = sr.CreatedAt,
                    IsActive = sr.IsActive
                })
                .ToListAsync();
        }

        public async Task<ServiceRecordDto?> GetServiceRecordByIdAsync(int id)
        {
            var serviceRecord = await _context.ServiceRecords
                .Include(sr => sr.Car)
                .ThenInclude(c => c.Customer)
                .Include(sr => sr.Mechanic)
                .FirstOrDefaultAsync(sr => sr.Id == id && sr.IsActive);

            if (serviceRecord == null)
                return null;

            return new ServiceRecordDto
            {
                Id = serviceRecord.Id,
                ServiceDate = serviceRecord.ServiceDate,
                CompletionDate = serviceRecord.CompletionDate,
                Description = serviceRecord.Description,
                WorkDescription = serviceRecord.WorkDescription,
                HoursWorked = serviceRecord.HoursWorked,
                IsCompleted = serviceRecord.IsCompleted,
                TotalCost = serviceRecord.TotalCost,
                CarId = serviceRecord.CarId,
                CarRegistrationNumber = serviceRecord.Car.RegistrationNumber,
                CarMakeModel = $"{serviceRecord.Car.Make} {serviceRecord.Car.Model}",
                MechanicId = serviceRecord.MechanicId,
                MechanicName = $"{serviceRecord.Mechanic.FirstName} {serviceRecord.Mechanic.LastName}",
                MechanicEmail = serviceRecord.Mechanic.Email,
                CustomerId = serviceRecord.Car.CustomerId,
                CustomerName = $"{serviceRecord.Car.Customer.FirstName} {serviceRecord.Car.Customer.LastName}",
                CustomerEmail = serviceRecord.Car.Customer.Email,
                CreatedAt = serviceRecord.CreatedAt,
                IsActive = serviceRecord.IsActive
            };
        }

        public async Task<List<ServiceRecordDto>> GetServiceRecordsByMechanicEmailAsync(string mechanicEmail)
        {
            return await _context.ServiceRecords
                .Include(sr => sr.Car)
                .ThenInclude(c => c.Customer)
                .Include(sr => sr.Mechanic)
                .Where(sr => sr.Mechanic.Email == mechanicEmail && sr.IsActive)
                .Select(sr => new ServiceRecordDto
                {
                    Id = sr.Id,
                    ServiceDate = sr.ServiceDate,
                    CompletionDate = sr.CompletionDate,
                    Description = sr.Description,
                    WorkDescription = sr.WorkDescription,
                    HoursWorked = sr.HoursWorked,
                    IsCompleted = sr.IsCompleted,
                    TotalCost = sr.TotalCost,
                    CarId = sr.CarId,
                    CarRegistrationNumber = sr.Car.RegistrationNumber,
                    CarMakeModel = $"{sr.Car.Make} {sr.Car.Model}",
                    MechanicId = sr.MechanicId,
                    MechanicName = $"{sr.Mechanic.FirstName} {sr.Mechanic.LastName}",
                    MechanicEmail = sr.Mechanic.Email,
                    CustomerId = sr.Car.CustomerId,
                    CustomerName = $"{sr.Car.Customer.FirstName} {sr.Car.Customer.LastName}",
                    CustomerEmail = sr.Car.Customer.Email,
                    CreatedAt = sr.CreatedAt,
                    IsActive = sr.IsActive
                })
                .ToListAsync();
        }

        public async Task<List<ServiceRecordDto>> GetServiceRecordsByCustomerEmailAsync(string customerEmail)
        {
            return await _context.ServiceRecords
                .Include(sr => sr.Car)
                .ThenInclude(c => c.Customer)
                .Include(sr => sr.Mechanic)
                .Where(sr => sr.Car.Customer.Email == customerEmail && sr.IsActive)
                .Select(sr => new ServiceRecordDto
                {
                    Id = sr.Id,
                    ServiceDate = sr.ServiceDate,
                    CompletionDate = sr.CompletionDate,
                    Description = sr.Description,
                    WorkDescription = sr.WorkDescription,
                    HoursWorked = sr.HoursWorked,
                    IsCompleted = sr.IsCompleted,
                    TotalCost = sr.TotalCost,
                    CarId = sr.CarId,
                    CarRegistrationNumber = sr.Car.RegistrationNumber,
                    CarMakeModel = $"{sr.Car.Make} {sr.Car.Model}",
                    MechanicId = sr.MechanicId,
                    MechanicName = $"{sr.Mechanic.FirstName} {sr.Mechanic.LastName}",
                    MechanicEmail = sr.Mechanic.Email,
                    CustomerId = sr.Car.CustomerId,
                    CustomerName = $"{sr.Car.Customer.FirstName} {sr.Car.Customer.LastName}",
                    CustomerEmail = sr.Car.Customer.Email,
                    CreatedAt = sr.CreatedAt,
                    IsActive = sr.IsActive
                })
                .ToListAsync();
        }

        public async Task<bool> CreateServiceRecordAsync(CreateServiceRecordDto createDto)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == createDto.CustomerEmail);
            if (customer == null)
                return false;

            var car = await _context.Cars.FirstOrDefaultAsync(c => c.RegistrationNumber == createDto.CarRegistrationNumber);
            if (car == null)
                return false;

            var mechanic = await _context.Mechanics.FirstOrDefaultAsync(m => m.Email == createDto.MechanicEmail);
            if (mechanic == null)
                return false;

            var serviceRecord = new ServiceRecord
            {
                ServiceDate = createDto.ServiceDate,
                Description = "Service scheduled",
                WorkDescription = "",
                HoursWorked = 0,
                IsCompleted = false,
                CompletionDate = null,
                TotalCost = 0,
                CarId = car.Id,
                MechanicId = mechanic.Id,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            _context.ServiceRecords.Add(serviceRecord);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateServiceRecordAsync(int id, UpdateServiceRecordDto updateDto)
        {
            var serviceRecord = await _context.ServiceRecords.FirstOrDefaultAsync(sr => sr.Id == id && sr.IsActive);
            if (serviceRecord == null)
                return false;

            serviceRecord.Description = updateDto.Description;
            serviceRecord.WorkDescription = updateDto.Description;
            serviceRecord.HoursWorked = updateDto.HoursWorked;
            serviceRecord.IsCompleted = updateDto.IsCompleted;

            if (updateDto.IsCompleted && !serviceRecord.IsCompleted)
            {
                serviceRecord.CompletionDate = DateTime.Now;
                serviceRecord.TotalCost = CalculateCost(updateDto.HoursWorked);
            }

            serviceRecord.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteServiceRecordAsync(int id, CompleteServiceRecordDto completeDto)
        {
            var serviceRecord = await _context.ServiceRecords.FirstOrDefaultAsync(sr => sr.Id == id && sr.IsActive);
            if (serviceRecord == null || serviceRecord.IsCompleted)
                return false;

            serviceRecord.WorkDescription = completeDto.Description;
            serviceRecord.HoursWorked = completeDto.HoursWorked;
            serviceRecord.IsCompleted = true;
            serviceRecord.CompletionDate = DateTime.Now;
            serviceRecord.TotalCost = CalculateCost(completeDto.HoursWorked);
            serviceRecord.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteServiceRecordAsync(int id)
        {
            var serviceRecord = await _context.ServiceRecords.FirstOrDefaultAsync(sr => sr.Id == id && sr.IsActive);
            if (serviceRecord == null)
                return false;

            serviceRecord.IsActive = false;
            serviceRecord.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<MechanicDto>> GetActiveMechanicsForDropdownAsync()
        {
            return await _context.Mechanics
                .Where(m => m.IsActive)
                .OrderBy(m => m.FirstName)
                .ThenBy(m => m.LastName)
                .Select(m => new MechanicDto
                {
                    Id = m.Id,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Email = m.Email,
                    PhoneNumber = m.PhoneNumber,
                    Specialization = m.Specialization,
                    IsActive = m.IsActive
                })
                .ToListAsync();
        }

        private decimal CalculateCost(decimal hoursWorked)
        {
            var roundedHours = Math.Ceiling(hoursWorked);
            return roundedHours * 75.00m;
        }
    }

    public class MechanicDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
} 