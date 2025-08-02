using Microsoft.AspNetCore.Mvc;
using CarManagementApp.Domain.DTOs;
using CarManagementAppRun.Services;

namespace CarManagementAppRun.Controllers
{
    public class CarsController : Controller
    {
        private readonly CarService _carService;
        private readonly CustomerService _customerService;

        public CarsController(CarService carService, CustomerService customerService)
        {
            _carService = carService;
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

            var cars = await _carService.GetAllCarsAsync();
            ViewBag.Message = "Cars Management - Administrator Access";
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View(cars);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                TempData["ErrorMessage"] = "Car not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Message = $"Car Details - ID: {id}";
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View(car);
        }

        public async Task<IActionResult> Create()
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var customers = await _customerService.GetActiveCustomersForDropdownAsync();
            ViewBag.Customers = customers;
            ViewBag.Message = "Create New Car";
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarDto createDto)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (ModelState.IsValid)
            {
                var success = await _carService.CreateCarAsync(createDto);
                if (success)
                {
                    TempData["SuccessMessage"] = "Car created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to create car. Registration number may already exist.";
                }
            }

            var customers = await _customerService.GetActiveCustomersForDropdownAsync();
            ViewBag.Customers = customers;
            ViewBag.Message = "Create New Car";
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View(createDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                TempData["ErrorMessage"] = "Car not found.";
                return RedirectToAction(nameof(Index));
            }

            var customers = await _customerService.GetActiveCustomersForDropdownAsync();
            ViewBag.Customers = customers;

            var updateDto = new UpdateCarDto
            {
                Id = car.Id,
                RegistrationNumber = car.RegistrationNumber,
                Make = car.Make,
                Model = car.Model,
                Color = car.Color,
                Year = car.Year,
                CustomerEmail = car.CustomerEmail
            };

            ViewBag.Message = $"Edit Car - ID: {id}";
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateCarDto updateDto)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (ModelState.IsValid)
            {
                var success = await _carService.UpdateCarAsync(id, updateDto);
                if (success)
                {
                    TempData["SuccessMessage"] = "Car updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update car.";
                }
            }

            var customers = await _customerService.GetActiveCustomersForDropdownAsync();
            ViewBag.Customers = customers;
            ViewBag.Message = $"Edit Car - ID: {id}";
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View(updateDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                TempData["ErrorMessage"] = "Car not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Message = $"Delete Car - ID: {id}";
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var success = await _carService.DeleteCarAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Car deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete car.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
} 