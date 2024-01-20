using Microsoft.AspNetCore.Mvc;
//using SchoolManagementDbFirst.ViewModels;
using SchoolManagementDatabaseFirst.Models;
using Microsoft.EntityFrameworkCore;
using SchoolManagementDatabaseFirst.SessionManager;
using Microsoft.AspNetCore.Identity;
using SchoolManagement.ViewModels;
using Microsoft.CodeAnalysis.Scripting;

namespace SchoolManagementDbFirst.Controllers
{
    public class AccountController : Controller
    {
        private readonly SchoolManagementDbfirstContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;


        public AccountController(SchoolManagementDbfirstContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if the email address is already in use
            var userExists = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.EmailAddress);

            if (userExists != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(model);
            }
            string hashedPassword = _passwordHasher.HashPassword(null, model.Password);

            var user = new User
            {

                UserType = model.UserType,
                DateOfBirth = model.DateOfBirth,
                Email = model.EmailAddress,
                FullName = model.FullName,
                Gender = model.Gender,
                ProfileImageUrl = model.ProfileImageUrl,
                Password= hashedPassword
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Check UserType and add to specific table
            if (user.UserType == "Teacher")
            {
                var teacher = new Teacher
                {
                    UserType = user.UserType,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email,
                    FullName = user.FullName,
                    Gender = user.Gender,
                    ProfileImageUrl = user.ProfileImageUrl

                };
                _context.Teachers.Add(teacher);
                await _context.SaveChangesAsync();
            }
            else if (user.UserType == "Student")
            {
                var student = new Student
                {

                    UserType = user.UserType,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email,
                    FullName = user.FullName,
                    Gender = user.Gender,
                    ProfileImageUrl = user.ProfileImageUrl
                };
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
            }
            // Set the user in the session
            UserSessionHelper.SetCurrentUser(HttpContext, user);
            TempData["Success"] = "Registration successful";


            // Redirect to the appropriate dashboard based on UserType
            if (user.UserType == "Teacher")
            {
                return RedirectToAction("Index", "Students");
            }
            else if (user.UserType == "Student")
            {
                return RedirectToAction("StudentDashboard");
            }

            return RedirectToAction("Index", "Home"); // Redirect to a default page if UserType is not recognized
        }

        [HttpGet]
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginViewModel.EmailAddress);

            if (user != null)
            {
                // User is found, check password
                var result = _passwordHasher.VerifyHashedPassword(null, user.Password, loginViewModel.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    // Password correct, perform your login logic here

                    // For example, you can set some information in the session
                    UserSessionHelper.SetCurrentUser(HttpContext, user);

                    // Redirect based on UserType
                    if (user.UserType == "Teacher")
                    {
                        return RedirectToAction("TeacherDashboard");
                    }
                    else if (user.UserType == "Student")
                    {
                        return RedirectToAction("StudentDashboard");
                    }
                }
            }

            // Password is incorrect or user not found
            TempData["Error"] = "Wrong credentials. Please try again";
            return View(loginViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
           

            // Clear the user session
            UserSessionHelper.ClearCurrentUser(HttpContext);
            return RedirectToAction("Index", "Home");
        }


        public ActionResult TeacherDashboard()
        {
           
                return View();
            
            
        }

        public ActionResult StudentDashboard()
        {
            return View();
        }






    }
}


