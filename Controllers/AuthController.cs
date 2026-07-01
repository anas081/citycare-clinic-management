using Microsoft.AspNetCore.Mvc;
using ClinicManagment.Services;
using CityCareClinic;

namespace ClinicManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ClinicService _clinicService;

        public AuthController(ClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        public class LoginRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty; // "Admin", "Doctor", "Patient"
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Username and password are required." });
            }

            string role = request.Role.ToLower();

            if (role == "admin")
            {
                if (request.Username == "anas" && request.Password == "0000")
                {
                    _clinicService.LogActivity("[ADMIN] Logged in successfully");
                    return Ok(new { username = "anas", name = "Admin Anas", role = "Admin" });
                }
                _clinicService.LogActivity("[FAILED] Admin login attempt failed");
                return Unauthorized(new { message = "Invalid admin credentials." });
            }
            else if (role == "doctor")
            {
                Doctor doc = _clinicService.DoctorManager.AuthenticateDoctor(request.Username, request.Password);
                if (doc != null)
                {
                    _clinicService.LogActivity($"[DOCTOR] {doc.Name} logged in successfully");
                    return Ok(new { username = doc.Username, name = doc.Name, role = "Doctor" });
                }
                _clinicService.LogActivity($"[FAILED] Doctor login attempt failed for username: {request.Username}");
                return Unauthorized(new { message = "Invalid doctor credentials." });
            }
            else if (role == "patient")
            {
                // AuthenticatePatient takes name and password as parameters
                Patient pat = _clinicService.PatientManager.AuthenticatePatient(request.Username, request.Password);
                if (pat != null)
                {
                    _clinicService.LogActivity($"[PATIENT] {pat.Name} logged in successfully");
                    return Ok(new { username = pat.Name, name = pat.Name, role = "Patient" });
                }
                _clinicService.LogActivity($"[FAILED] Patient login attempt failed for name: {request.Username}");
                return Unauthorized(new { message = "Invalid patient credentials." });
            }

            return BadRequest(new { message = "Invalid role specified." });
        }
    }
}
