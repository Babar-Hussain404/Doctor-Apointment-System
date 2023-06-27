using DocApp.Models;
using DocApp.Services.AppointmentService;
using DocApp.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DocApp.Controllers
{
    //[AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAppointmentService _appointments;
        private readonly IUserService _users;

        public HomeController(ILogger<HomeController> logger, IAppointmentService appointments, IUserService users)
        {
            _logger = logger;
            _appointments = appointments;
            _users = users;
        }

        public IActionResult Index()
        {
            var _allUsers = _users.GetAll();
            var _totalDoctors = _allUsers.Count(u => u.UserType == "Doctor");
            var _totalPatients = _allUsers.Count(u => u.UserType == "Patient");
            var _totalAppointments = _appointments.GetAll().Count();

            TempData["totalDoctors"] = _totalDoctors;
            TempData["totalPatients"] = _totalPatients;
            TempData["totalAppointments"] = _totalAppointments;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}