using Microsoft.AspNetCore.Mvc;
using CarManagementApp.Domain.DTOs;
using CarManagementAppRun.Services;
using Microsoft.AspNetCore.Authorization;

namespace CarManagementAppRun.Controllers
{
    public class ServiceRecordsController : Controller
    {
        private readonly ServiceRecordService _serviceRecordService;
        private readonly CustomerService _customerService;
        private readonly CarService _carService;

        public ServiceRecordsController(ServiceRecordService serviceRecordService, CustomerService customerService, CarService carService)
        {
            _serviceRecordService = serviceRecordService;
            _customerService = customerService;
            _carService = carService;
        }

        private bool IsAdministrator()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            return userRole == "Administrator";
        }

        private bool IsMechanic()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            return userRole == "Mechanic";
        }

        private bool IsCustomer()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            return userRole == "Customer";
        }

        public async Task<IActionResult> Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var userEmail = HttpContext.Session.GetString("UserEmail");
            
            if (string.IsNullOrEmpty(userRole))
            {
                return RedirectToAction("Login", "Account");
            }

            List<ServiceRecordDto> serviceRecords;
            
           
            if (IsAdministrator())
            {
                // Admin can see all service records
                serviceRecords = await _serviceRecordService.GetAllServiceRecordsAsync();
            }
            else if (IsMechanic())
            {
                // Mechanic can only see their assigned service records
                serviceRecords = await _serviceRecordService.GetServiceRecordsByMechanicEmailAsync(userEmail);
            }
            else if (IsCustomer())
            {
                // Customer can only see their own service records
                serviceRecords = await _serviceRecordService.GetServiceRecordsByCustomerEmailAsync(userEmail);
            }
            else
            {
                // Default to empty list for unknown roles
                serviceRecords = new List<ServiceRecordDto>();
            }

            ViewBag.Message = "Service Records Management";
            ViewBag.UserRole = userRole;
            return View(serviceRecords);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole))
            {
                return RedirectToAction("Login", "Account");
            }

            var serviceRecord = await _serviceRecordService.GetServiceRecordByIdAsync(id);
            if (serviceRecord == null)
            {
                TempData["ErrorMessage"] = "Service record not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Message = $"Service Record Details - ID: {id}";
            ViewBag.UserRole = userRole;
            return View(serviceRecord);
        }

        public async Task<IActionResult> Create()
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var customers = await _customerService.GetActiveCustomersForDropdownAsync();
            var cars = await _carService.GetActiveCarsForDropdownAsync();
            var mechanics = await _serviceRecordService.GetActiveMechanicsForDropdownAsync();

            ViewBag.Customers = customers;
            ViewBag.Cars = cars;
            ViewBag.Mechanics = mechanics;

            ViewBag.Message = "Create New Service Record";
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateServiceRecordDto createDto)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (ModelState.IsValid)
            {
                var success = await _serviceRecordService.CreateServiceRecordAsync(createDto);
                if (success)
                {
                    TempData["SuccessMessage"] = "Service record created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to create service record.";
                }
            }

            var customers = await _customerService.GetActiveCustomersForDropdownAsync();
            var cars = await _carService.GetActiveCarsForDropdownAsync();
            var mechanics = await _serviceRecordService.GetActiveMechanicsForDropdownAsync();

            ViewBag.Customers = customers;
            ViewBag.Cars = cars;
            ViewBag.Mechanics = mechanics;

            ViewBag.Message = "Create New Service Record";
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View(createDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var serviceRecord = await _serviceRecordService.GetServiceRecordByIdAsync(id);
            if (serviceRecord == null)
            {
                TempData["ErrorMessage"] = "Service record not found.";
                return RedirectToAction(nameof(Index));
            }

            var updateDto = new UpdateServiceRecordDto
            {
                Id = serviceRecord.Id,
                Description = serviceRecord.WorkDescription,
                HoursWorked = serviceRecord.HoursWorked,
                IsCompleted = serviceRecord.IsCompleted
            };

            ViewBag.Message = $"Edit Service Record - ID: {id}";
            ViewBag.UserRole = userRole;
            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateServiceRecordDto updateDto)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (ModelState.IsValid)
            {
                var success = await _serviceRecordService.UpdateServiceRecordAsync(id, updateDto);
                if (success)
                {
                    TempData["SuccessMessage"] = "Service record updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update service record.";
                }
            }

            ViewBag.Message = $"Edit Service Record - ID: {id}";
            ViewBag.UserRole = userRole;
            return View(updateDto);
        }

        public async Task<IActionResult> Complete(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!IsMechanic())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var serviceRecord = await _serviceRecordService.GetServiceRecordByIdAsync(id);
            if (serviceRecord == null)
            {
                TempData["ErrorMessage"] = "Service record not found.";
                return RedirectToAction(nameof(MyServices));
            }

            if (serviceRecord.IsCompleted)
            {
                TempData["ErrorMessage"] = "Service record is already completed.";
                return RedirectToAction(nameof(MyServices));
            }

            ViewBag.Message = $"Complete Service Record - ID: {id}";
            ViewBag.UserRole = userRole;
            return View(serviceRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id, CompleteServiceRecordDto completeDto)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!IsMechanic())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (ModelState.IsValid)
            {
                var success = await _serviceRecordService.CompleteServiceRecordAsync(id, completeDto);
                if (success)
                {
                    TempData["SuccessMessage"] = "Service record completed successfully.";
                    return RedirectToAction(nameof(MyServices));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to complete service record.";
                }
            }

            ViewBag.Message = $"Complete Service Record - ID: {id}";
            ViewBag.UserRole = userRole;
            return View(completeDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var serviceRecord = await _serviceRecordService.GetServiceRecordByIdAsync(id);
            if (serviceRecord == null)
            {
                TempData["ErrorMessage"] = "Service record not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Message = $"Delete Service Record - ID: {id}";
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View(serviceRecord);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdministrator())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var success = await _serviceRecordService.DeleteServiceRecordAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Service record deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete service record.";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MyServices()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!IsMechanic())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var userEmail = HttpContext.Session.GetString("UserEmail");
            var serviceRecords = await _serviceRecordService.GetServiceRecordsByMechanicEmailAsync(userEmail);

            ViewBag.Message = "My Assigned Services - Mechanic View";
            ViewBag.UserRole = userRole;
            return View(serviceRecords);
        }

        public async Task<IActionResult> MyVehicleServices()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!IsCustomer())
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var userEmail = HttpContext.Session.GetString("UserEmail");
            var serviceRecords = await _serviceRecordService.GetServiceRecordsByCustomerEmailAsync(userEmail);

            ViewBag.Message = "My Vehicle Services - Customer View";
            ViewBag.UserRole = userRole;
            return View(serviceRecords);
        }
    }
} 