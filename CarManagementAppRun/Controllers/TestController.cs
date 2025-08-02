using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CarManagementAppRun.Data;
using Microsoft.EntityFrameworkCore;
using CarManagementAppRun.Services;

namespace CarManagementAppRun.Controllers
{
    public class TestController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly CustomerService _customerService;

        public TestController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, CustomerService customerService)
        {
            _userManager = userManager;
            _context = context;
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new TestViewModel();

            var adminUser = await _userManager.FindByEmailAsync("admin@example.com");
            viewModel.AdminExists = adminUser != null;
            if (adminUser != null)
            {
                viewModel.AdminId = adminUser.Id;
                viewModel.AdminEmail = adminUser.Email;
                viewModel.AdminUserName = adminUser.UserName;
                viewModel.AdminPasswordHash = adminUser.PasswordHash;
                
                var signInManager = HttpContext.RequestServices.GetRequiredService<SignInManager<ApplicationUser>>();
                var result = await signInManager.PasswordSignInAsync(adminUser, "123", false, lockoutOnFailure: false);
                viewModel.AdminCanSignIn = result.Succeeded;
            }

            var allUsers = await _userManager.Users.ToListAsync();
            viewModel.TotalUsers = allUsers.Count;
            viewModel.Users = allUsers.Select(u => new UserInfo
            {
                Id = u.Id,
                Email = u.Email,
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PasswordHash = u.PasswordHash,
                IsActive = u.IsActive
            }).ToList();

            viewModel.CustomersCount = await _context.Customers.CountAsync();
            viewModel.MechanicsCount = await _context.Mechanics.CountAsync();
            viewModel.CarsCount = await _context.Cars.CountAsync();
            viewModel.ServiceRecordsCount = await _context.ServiceRecords.CountAsync();

            viewModel.CustomersFromService = await _customerService.GetAllCustomersAsync();
            viewModel.CustomerServiceCount = viewModel.CustomersFromService.Count;

            return View(viewModel);
        }
    }

    public class TestViewModel
    {
        public bool AdminExists { get; set; }
        public string? AdminId { get; set; }
        public string? AdminEmail { get; set; }
        public string? AdminUserName { get; set; }
        public string? AdminPasswordHash { get; set; }
        public bool AdminCanSignIn { get; set; }
        public int TotalUsers { get; set; }
        public List<UserInfo> Users { get; set; } = new();
        public int CustomersCount { get; set; }
        public int MechanicsCount { get; set; }
        public int CarsCount { get; set; }
        public int ServiceRecordsCount { get; set; }
        public List<CarManagementApp.Domain.DTOs.CustomerDto> CustomersFromService { get; set; } = new();
        public int CustomerServiceCount { get; set; }
    }

    public class UserInfo
    {
        public string Id { get; set; } = "";
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PasswordHash { get; set; }
        public bool IsActive { get; set; }
    }
} 