using CarManagementApp.Domain.DTOs;
using CarManagementAppRun.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarManagementAppRun.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CustomerService _customerService;

        public CustomersController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        private bool IsAdministrator()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            return userRole == "Administrator";
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var customers = await _customerService.GetAllCustomersAsync();
            ViewBag.Message = "Customers Management - Administrator Access";
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View(customers);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            ViewBag.Message = $"Customer Details - ID: {id}";
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View(customer);
        }

        public IActionResult Create()
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            ViewBag.Message = "Create New Customer";
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerDto createCustomerDto)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (ModelState.IsValid)
            {
                var result = await _customerService.CreateCustomerAsync(createCustomerDto);
                if (result)
                {
                    TempData["SuccessMessage"] = "Customer created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Failed to create customer. Email might already exist.");
            }

            ViewBag.Message = "Create New Customer";
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View(createCustomerDto);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateCustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address
            };

            ViewBag.Message = $"Edit Customer - ID: {id}";
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UpdateCustomerDto updateCustomerDto)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (id != updateCustomerDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _customerService.UpdateCustomerAsync(id, updateCustomerDto);
                if (result)
                {
                    TempData["SuccessMessage"] = "Customer updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Failed to update customer.");
            }

            ViewBag.Message = $"Edit Customer - ID: {id}";
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View(updateCustomerDto);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            ViewBag.Message = $"Delete Customer - ID: {id}";
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var result = await _customerService.DeleteCustomerAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Customer deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete customer.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
} 