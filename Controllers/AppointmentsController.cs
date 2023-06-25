using DocApp.Data;
using DocApp.GenericRepository;
using DocApp.Models;
using DocApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DocApp.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IGenericRepository<Appointment> _appointments;
        private readonly IGenericRepository<User> _users;

        public AppointmentsController(IGenericRepository<Appointment> appointments, IGenericRepository<User> users)
        {
            _appointments = appointments;
            _users = users;
        }

        // GET: Booking List
        public IActionResult AppointmentList()
        {
            var _apointmentList = _appointments.GetAll();
            return View(_apointmentList);
        }

        // GET: Add Appointment
        public IActionResult AddAppointment()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                TempData["error"] = "You have already made an appointment";
                return RedirectToAction("AppointmentList");
            }

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                PatientId = _patient.Id,
                DoctorName = "Doctor",
                PatientName = model.FName + model.LName,
                PatientPhone = _patient.Phoneno,
                SlotNo = model.SlotNo,
                Date = DateTime.Now,
                Status = Appointment.StatusEnum.Pending
            };

            _appointments.Add(appointment);
            _appointments.SaveChanges();

            TempData["success"] = "Appointment Created successfully";
            return RedirectToAction("AppointmentList");

        }

        //GET: Bookings/Approved/Id
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
                TempData["success"] = "Appointment Activeted successfully";
            }
            else
            {
                TempData["error"] = "Unable to find the Appointment.";
            }

            _appointments.SaveChanges();

            return RedirectToAction("AppointmentList");
        }

        //GET: Bookings/Rejected/Id
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

            return RedirectToAction("AppointmentList");
        }

        //GET: Bookings/Delete/Id
        public IActionResult Delete(string Id)
        {
            if (Id == null || !AppointmentExists(Id))
            {
                return NotFound();
            }

            _appointments.Delete(new Guid(Id));
            _appointments.SaveChanges();

            TempData["success"] = "Appointment deleted successfully";
            return RedirectToAction("AppointmentList");
        }

        private bool AppointmentExists(string Id)
        {
            return _appointments.GetById(new Guid(Id)) != null;
        }

    }
}
