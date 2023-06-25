using DocApp.Data;
using DocApp.GenericRepository;
using DocApp.Models;
using DocApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace DocApp.Controllers
{
    //[AllowAnonymous]
    public class UsersController : Controller
    {
        private readonly IGenericRepository<User> _users;

        public UsersController(IGenericRepository<User> users)
        {
            _users = users;
        }

        public IActionResult DoctorList()
        {
            var _doctors = _users.GetAll().Where(u=> u.UserType == "Doctor");
            return View(_doctors);
        }

        public IActionResult PatientList()
        {
            var _patients = _users.GetAll().Where(u => u.UserType == "Patient");
            return View(_patients);
        }

        // GET: User/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid Model State";
                return View(model);
            }

            if (model.TermsAndConditions == false)
            {
                ModelState.AddModelError("TermsAndConditions", "You must agree to the terms and conditions.");
                return View(model);
            }

            var _user = new User
            {
                FName = model.FName,
                LName = model.LName,
                Email = model.Email,
                Password = model.Password,
                DOB = model.DOB,
                Gender = model.Gender,
                UserType = model.UserType,
                CNIC = model.CNIC,
                Phoneno = model.Phoneno,
                Address = model.Address,
                CreatedOn = DateTime.UtcNow
            };

            if (UserExists(_user.Email, _user.Password))
            {
                TempData["error"] = "You are already registered Go to login page.";
                return View(model);
            }

            _users.Add(_user);
            _user.CreatedBy = _user.Id;
            _users.SaveChanges();

            TempData["success"] = "You have been registered successfully";
            return RedirectToAction("Login", "Users");
        }

        // GET: User/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            var _user = new User
            {
                Email = model.Email,
                Password = model.Password,
                RememberMe = model.RememberMe
            };

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid Model State";
                return View(model);
            }

            if (!UserExists(_user.Email, _user.Password))
            {
                TempData["Error"] = "You have not registered yet go to Register page and register first.";
                return View(model);
            }

            var user = _users.GetAll().FirstOrDefault(u => u.Email == _user.Email);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, _user.Email),
                new Claim(ClaimTypes.Name, $"{user?.FName ?? string.Empty} {user?.LName ?? string.Empty}"),
                new Claim(ClaimTypes.Role, user?.UserType ?? string.Empty)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = _user.RememberMe
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Store the user ID in the session
            byte[] userIdBytes = Encoding.ASCII.GetBytes(user?.Id.ToString() ?? string.Empty);
            HttpContext.Session.Set("UserId", userIdBytes);

            TempData["success"] = "Logged in successfully";
            return RedirectToAction("Index", "Home");

        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        //Logout Functionality
        public IActionResult LogOutConfirm()
        {
            // Clear authentication cookies and claims
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Clear session data
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }

        private bool UserExists(string email, string password)
        {
            return _users.GetAll().Any(u => u.Email == email && u.Password == password);
        }
    }
}
