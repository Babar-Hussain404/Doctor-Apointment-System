using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DocApp.Data;
using DocApp.Models;
using DocApp.ViewModels;

namespace DocApp.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly DocAppContext _context;

        public AppointmentsController(DocAppContext context)
        {
            _context = context;
        }

        // GET: Booking List
        public async Task<IActionResult> AppointmentList()
        {
            var _apointmentList = await _context.Appointments.ToListAsync();
            return View(_apointmentList);
        }

        public async Task<IActionResult> Create(string Id)
            {
            var _patient = _context.Patients.FirstOrDefault(r => r.Id == new Guid(Id));

            if (_patient == null) { return NotFound(); }

            if (IsAppointed(Id))
            {
                TempData["error"] = "You have already made an appointment";
                return RedirectToAction("AppointmentList", "Appointments");
            }

            // Get the current user's ID from the session
            byte[]? userIdBytes;
            if (!HttpContext.Session.TryGetValue("UserId", out userIdBytes))
            {
                return RedirectToAction("Login", "Users");
            }

            Guid userId = new Guid(Encoding.ASCII.GetString(userIdBytes));
            // Retrieve the user's information from the database using their ID
            User? _user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if(_user == null) { return NotFound(); }

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                DoctorName = "Doctor",
                PatientName = _patient.FName + _patient.LName,
                PatientPhone = _patient.Phoneno,
                SlotNo = "11",
                Date = DateTime.Now,
                Status = Appointment.StatusEnum.Pending
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            TempData["success"] = "Appointment Created successfully";
            return RedirectToAction(nameof(AppointmentList));
            
        }

        //GET: Bookings/Approved/Id
        public async Task<IActionResult> Approved(string Id)
        {
            if (Id == null || !AppointmentExists(Id))
            {
                return NotFound();
            }

            var appointment = _context.Appointments.FirstOrDefault(b => b.Id == new Guid(Id));

            if (appointment != null)
            {
                appointment.Status = Appointment.StatusEnum.Active;
                TempData["success"] = "Appointment Activeted successfully";
            }
            else
            {
                TempData["error"] = "Unable to find the Appointment.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AppointmentList));
        }

        //GET: Bookings/Rejected/Id
        public async Task<IActionResult> Rejected(string Id)
        {
            if (Id == null || !AppointmentExists(Id))
            {
                return NotFound();
            }

            var booking = _context.Appointments.FirstOrDefault(b => b.Id == new Guid(Id));

            if (booking != null)
            {
                booking.Status = Appointment.StatusEnum.Closed;
                TempData["success"] = "Appointment Closed successfully";
            }
            else
            {
                TempData["error"] = "Unable to find the Appointment.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AppointmentList));
        }

        //GET: Bookings/Delete/Id
        public async Task<IActionResult> Delete(string Id)
        {
            if (Id == null || !AppointmentExists(Id))
            {
                return NotFound();
            }

            var booking = _context.Appointments.FirstOrDefault(b => b.Id == new Guid(Id));
            
            if(booking != null) 
            {
                _context.Appointments.Remove(booking);
                await _context.SaveChangesAsync();
            }

            TempData["success"] = "Appointment deleted successfully";
            return RedirectToAction(nameof(AppointmentList));
        }

        private bool IsAppointed(string Id)
        {
            return (_context.Appointments?.Any(e => e.PatientId == new Guid(Id))).GetValueOrDefault();
        }
        private bool AppointmentExists(string Id)
        {
            return (_context.Appointments?.Any(e => e.Id == new Guid(Id))).GetValueOrDefault();
        }
    }
}
