using CarManagementApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarManagementAppRun.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));
            }
            if (!await roleManager.RoleExistsAsync("Mechanic"))
            {
                await roleManager.CreateAsync(new IdentityRole("Mechanic"));
            }
            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                await roleManager.CreateAsync(new IdentityRole("Customer"));
            }

            await SeedAdminUsersAsync(userManager);

            await SeedCustomersAsync(context);

            await SeedMechanicsAsync(context);

            await SeedCarsAsync(context);

            await SeedServiceRecordsAsync(context);

            await context.SaveChangesAsync();
        }

        private static async Task SeedAdminUsersAsync(UserManager<ApplicationUser> userManager)
        {
            var adminEmail = "admin@carservice.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };
                var result = await userManager.CreateAsync(adminUser, "Dorset001^");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrator");
                }
            }

            var customer1Email = "customer1@carservice.com";
            var customer1User = await userManager.FindByEmailAsync(customer1Email);
            if (customer1User == null)
            {
                customer1User = new ApplicationUser
                {
                    UserName = customer1Email,
                    Email = customer1Email,
                    FirstName = "John",
                    LastName = "Customer",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };
                var result = await userManager.CreateAsync(customer1User, "Dorset001^");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customer1User, "Customer");
                }
            }

            var customer2Email = "customer2@carservice.com";
            var customer2User = await userManager.FindByEmailAsync(customer2Email);
            if (customer2User == null)
            {
                customer2User = new ApplicationUser
                {
                    UserName = customer2Email,
                    Email = customer2Email,
                    FirstName = "Jane",
                    LastName = "Customer",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };
                var result = await userManager.CreateAsync(customer2User, "Dorset001^");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customer2User, "Customer");
                }
            }

            var mechanic1Email = "mechanic1@carservice.com";
            var mechanic1User = await userManager.FindByEmailAsync(mechanic1Email);
            if (mechanic1User == null)
            {
                mechanic1User = new ApplicationUser
                {
                    UserName = mechanic1Email,
                    Email = mechanic1Email,
                    FirstName = "John",
                    LastName = "Mechanic",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };
                var result = await userManager.CreateAsync(mechanic1User, "Dorset001^");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(mechanic1User, "Mechanic");
                }
            }

            var mechanic2Email = "mechanic2@carservice.com";
            var mechanic2User = await userManager.FindByEmailAsync(mechanic2Email);
            if (mechanic2User == null)
            {
                mechanic2User = new ApplicationUser
                {
                    UserName = mechanic2Email,
                    Email = mechanic2Email,
                    FirstName = "Mike",
                    LastName = "Mechanic",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };
                var result = await userManager.CreateAsync(mechanic2User, "Dorset001^");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(mechanic2User, "Mechanic");
                }
            }
        }

        private static async Task SeedCustomersAsync(ApplicationDbContext context)
        {
            if (!await context.Customers.AnyAsync())
            {
                var customers = new List<Customer>
                {
                    new Customer
                    {
                        Id = Guid.NewGuid().ToString(),
                        FirstName = "John",
                        LastName = "Customer",
                        Email = "customer1@carservice.com",
                        PhoneNumber = "+353123456789",
                        Address = "123 Main Street, Dublin",
                        CreatedAt = DateTime.Now.AddDays(-10),
                        IsActive = true
                    },
                    new Customer
                    {
                        Id = Guid.NewGuid().ToString(),
                        FirstName = "Jane",
                        LastName = "Customer",
                        Email = "customer2@carservice.com",
                        PhoneNumber = "+353987654321",
                        Address = "456 Oak Avenue, Cork",
                        CreatedAt = DateTime.Now.AddDays(-5),
                        IsActive = true
                    }
                };

                await context.Customers.AddRangeAsync(customers);
            }
        }

        private static async Task SeedMechanicsAsync(ApplicationDbContext context)
        {
            if (!await context.Mechanics.AnyAsync())
            {
                var mechanics = new List<Mechanic>
                {
                    new Mechanic
                    {
                        Id = Guid.NewGuid().ToString(),
                        FirstName = "John",
                        LastName = "Mechanic",
                        Email = "mechanic1@carservice.com",
                        PhoneNumber = "+353111222333",
                        Specialization = "Engine and Transmission",
                        CreatedAt = DateTime.Now.AddDays(-30),
                        IsActive = true
                    },
                    new Mechanic
                    {
                        Id = Guid.NewGuid().ToString(),
                        FirstName = "Mike",
                        LastName = "Mechanic",
                        Email = "mechanic2@carservice.com",
                        PhoneNumber = "+353444555666",
                        Specialization = "Electrical and Diagnostics",
                        CreatedAt = DateTime.Now.AddDays(-25),
                        IsActive = true
                    }
                };

                await context.Mechanics.AddRangeAsync(mechanics);
            }
        }

        private static async Task SeedCarsAsync(ApplicationDbContext context)
        {
            if (!await context.Cars.AnyAsync())
            {
                var customers = await context.Customers.ToListAsync();
                var johnCustomer = customers.FirstOrDefault(c => c.Email == "customer1@carservice.com");
                var janeCustomer = customers.FirstOrDefault(c => c.Email == "customer2@carservice.com");

                if (johnCustomer != null && janeCustomer != null)
                {
                    var cars = new List<Car>
                    {
                        new Car
                        {
                            RegistrationNumber = "ABC123",
                            Make = "Toyota",
                            Model = "Camry",
                            Color = "Silver",
                            Year = 2020,
                            CustomerId = johnCustomer.Id,
                            CreatedAt = DateTime.Now.AddDays(-30),
                            RegistrationDate = DateTime.Now.AddDays(-30),
                            IsActive = true
                        },
                        new Car
                        {
                            RegistrationNumber = "XYZ789",
                            Make = "Honda",
                            Model = "Civic",
                            Color = "Blue",
                            Year = 2019,
                            CustomerId = janeCustomer.Id,
                            CreatedAt = DateTime.Now.AddDays(-20),
                            RegistrationDate = DateTime.Now.AddDays(-20),
                            IsActive = true
                        }
                    };

                    await context.Cars.AddRangeAsync(cars);
                }
            }
        }

        private static async Task SeedServiceRecordsAsync(ApplicationDbContext context)
        {
            if (!await context.ServiceRecords.AnyAsync())
            {
                var cars = await context.Cars.ToListAsync();
                var mechanics = await context.Mechanics.ToListAsync();

                if (cars.Any() && mechanics.Any())
                {
                    var serviceRecords = new List<ServiceRecord>
                    {
                        new ServiceRecord
                        {
                            ServiceDate = DateTime.Now.AddDays(-5),
                            Description = "Oil change and brake inspection",
                            WorkDescription = "Completed oil change and brake inspection. All systems working properly.",
                            HoursWorked = 2.5m,
                            IsCompleted = true,
                            CompletionDate = DateTime.Now.AddDays(-3),
                            TotalCost = CalculateCost(2.5m),
                            CarId = cars[0].Id,
                            MechanicId = mechanics[0].Id,
                            CreatedAt = DateTime.Now.AddDays(-7),
                            IsActive = true
                        },
                        new ServiceRecord
                        {
                            ServiceDate = DateTime.Now.AddDays(-2),
                            Description = "Engine tune-up",
                            WorkDescription = "",
                            HoursWorked = 0,
                            IsCompleted = false,
                            CompletionDate = null,
                            TotalCost = 0,
                            CarId = cars[1].Id,
                            MechanicId = mechanics[1].Id,
                            CreatedAt = DateTime.Now.AddDays(-3),
                            IsActive = true
                        }
                    };

                    await context.ServiceRecords.AddRangeAsync(serviceRecords);
                }
            }
        }

        private static decimal CalculateCost(decimal hoursWorked)
        {
            var roundedHours = Math.Ceiling(hoursWorked);
            return roundedHours * 75.00m;
        }
    }
} 