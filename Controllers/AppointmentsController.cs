using DocApp.Data;
using DocApp.GenericRepository;
using DocApp.Models;
using DocApp.Services.AppointmentService;
using DocApp.Services.UserService;
using DocApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;

namespace DocApp.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointments;
        private readonly IUserService _users;

        public AppointmentsController(IAppointmentService appointments, IUserService users)
        {
            _appointments = appointments;
            _users = users;
        }


        // GET: Appointment List
        [Authorize(Roles = "Staff , Patient")]
        public IActionResult AppointmentList()
        {
            var _apointmentList = _appointments.GetAll();
            return View(_apointmentList);
        }

        // GET: My Appointment List
        [Authorize(Roles = "Patient")]
        public IActionResult MyAppointmentList()
        {
            // Get the current user's ID from the session
            byte[]? userIdBytes;
            if (!HttpContext.Session.TryGetValue("UserId", out userIdBytes))
            {
                return RedirectToAction("Login", "Users");
            }
            Guid _currentUserId = new Guid(Encoding.ASCII.GetString(userIdBytes));

            var _myApointmentList = _appointments.GetAll().Where(a => a.PatientId == _currentUserId);
            return View(_myApointmentList);
        }

        // GET: Active Appointment List
        [Authorize(Roles = "Doctor")]
        public IActionResult ActiveAppointmentList()
        {
            var _activeApointmentList = _appointments.GetAll().Where(a => a.isActive == true );
            return View(_activeApointmentList);
        }

        // GET: Patient Details
        [Authorize(Roles = "Doctor")]
        public IActionResult PatientDetails(string Id)
        {
            var _patientDetails = _appointments.GetById(new Guid(Id));
            return View(_patientDetails);
        }

        // GET: Patient History
        [Authorize(Roles = "Doctor")]
        public IActionResult PatientHistory(string Id)
        {
            var _patientHistory = _appointments.GetAll().Where(a => a.PatientId == new Guid(Id));
            return View(_patientHistory);
        }

        // GET: Add Appointment
        [Authorize(Roles = "Patient")]
        public IActionResult AddAppointment()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Patient")]
        public IActionResult AddAppointment(PatientVM model)
        {
            // Get the current user's ID from the session
            byte[]? userIdBytes;
            if (!HttpContext.Session.TryGetValue("UserId", out userIdBytes))
            {
                return RedirectToAction("Login", "Users");
            }

            Guid userId = new Guid(Encoding.ASCII.GetString(userIdBytes));

            var _patient = _users.GetById(userId);

            //Cheeck if there is any active appointment
            var _apointment = _appointments.GetAll().Any(e => e.PatientId == userId && e.isActive == true);
            
            if (_apointment)
            {
                TempData["error"] = "You already have an active appointment";
                return RedirectToAction("AddAppointment");
            }

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                PatientId = _patient.Id,
                PatientName = model.FName + model.LName,
                Age = CalculateAge(DateTime.Parse(_patient.DOB)),

                PatientPhone = _patient.Phoneno,
                PatientEmail = _patient.Email,

                Disease = model.Disease,
                Description = model.Description,

                SlotNo = model.SlotNo,
                Date = DateTime.Now,
                Status = Appointment.StatusEnum.Pending,
                isActive = false
            };
             
            _appointments.Add(appointment);
            _appointments.SaveChanges();

            TempData["success"] = "Appointment Created successfully";
            return RedirectToAction("MyAppointmentList");

        }

        // Method to calculate age based on date of birth
        private int CalculateAge(DateTime dob)
        {
            var age = DateTime.Now.Year - dob.Year;
            
            //if the person's birthdy has not been arrived subtract current year
            if (DateTime.Now < dob.AddYears(age))
            {
                age--;
            }
            return age;
        }

        //GET: Appointments/Approved/Id
        [Authorize(Roles = "Doctor")]
        public IActionResult AddPrescription(Appointment model)
        {
            // Get the current user's ID from the session
            byte[]? userIdBytes;
            if (!HttpContext.Session.TryGetValue("UserId", out userIdBytes))
            {
                return RedirectToAction("Login", "Users");
            }
            Guid userId = new Guid(Encoding.ASCII.GetString(userIdBytes));
            var _currentUser = _users.GetById(userId);

            var _appointment = _appointments.GetById(model.Id);

            if (_appointment != null)
            {
                _appointment.DoctorName = _currentUser.FName+_currentUser.LName;

                if(_appointment.Prescription == null)
                {
                    _appointment.Prescription = model.Prescription;
                    TempData["success"] = "Prescription added successfully";
                }
                else
                {
                    _appointment.Prescription = model.Prescription;
                    TempData["success"] = "Prescription Updated successfully";
                }
            }
            else
            {
                TempData["error"] = "Unable to Add prescription";
            }
            
            _appointments.SaveChanges();

            return RedirectToAction("ActiveAppointmentList");
        }

        //GET: Appointments/Approved/Id
        [Authorize(Roles = "Staff")]
        public IActionResult Active(string Id)
        {
            if (Id == null || !AppointmentExists(Id))
            {
                return NotFound();
            }

            var _appointment = _appointments.GetById(new Guid(Id));

            if (_appointment != null)
            {
                _appointment.Status = Appointment.StatusEnum.Active;
                _appointment.isActive = true;
                TempData["success"] = "Appointment Activated successfully";
            }
            else
            {
                TempData["error"] = "Unable to find the Appointment.";
            }

            _appointments.SaveChanges();

            return RedirectToAction("AppointmentList");
        }

        //GET: Appointments/Rejected/Id
        [Authorize(Roles = "Doctor , Staff")]
        public IActionResult Closed(string Id)
        {
            if (Id == null || !AppointmentExists(Id))
            {
                return NotFound();
            }

            var _appointment = _appointments.GetById(new Guid(Id));

            if (_appointment != null)
            {
                _appointment.Status = Appointment.StatusEnum.Closed;
                _appointment.isActive = false;
                TempData["success"] = "Appointment Closed successfully";
            }
            else
            {
                TempData["error"] = "Unable to find the Appointment.";
            }

            _appointments.SaveChanges();

            // Get the current user's ID from the session
            byte[]? userIdBytes;
            if (!HttpContext.Session.TryGetValue("UserId", out userIdBytes))
            {
                return RedirectToAction("Login", "Users");
            }
            Guid userId = new Guid(Encoding.ASCII.GetString(userIdBytes));
            var _currentUser = _users.GetById(userId);
            
            if(_currentUser.UserType == "Doctor")
            {
                TempData["success"] = "Appointment Closed successfully";
                return RedirectToAction("ActiveAppointmentList");
            }

            return RedirectToAction("AppointmentList");
        }

        //GET: Appointments/Delete/Id
        [Authorize(Roles = "Doctor , Staff , Patient")]
        public IActionResult Delete(string Id)
        {
            if (Id == null || !AppointmentExists(Id))
            {
                return NotFound();
            }

            var _appointment = _appointments.GetById(new Guid(Id));

            // Get the current user's ID from the session
            byte[]? userIdBytes;
            if (!HttpContext.Session.TryGetValue("UserId", out userIdBytes))
            {
                return RedirectToAction("Login", "Users");
            }
            Guid userId = new Guid(Encoding.ASCII.GetString(userIdBytes));
            var _currentUser = _users.GetById(userId);

            if (_currentUser.UserType =="Patient" && _appointment.PatientId != _currentUser.Id) 
            {
                TempData["error"] = "You can only delete your own Appointment!";
                return RedirectToAction("MyAppointmentList");
            }
            else if (_currentUser.UserType == "Doctor") 
            {
                TempData["success"] = "Appointment deleted successfully";
                return RedirectToAction("ActiveAppointmentList");
            }
            else if(_currentUser.UserType == "Staff")
            {
                _appointments.Delete(_appointment.Id);
                _appointments.SaveChanges();
                TempData["success"] = "Appointment deleted successfully";
                return RedirectToAction("AppointmentList");
            }

            _appointments.Delete(_appointment.Id);
            _appointments.SaveChanges();
            TempData["success"] = "Appointment deleted successfully";
            return RedirectToAction("MyAppointmentList");
        }

        private bool AppointmentExists(string Id)
        {
            return _appointments.GetById(new Guid(Id)) != null;
        }

    }
}
