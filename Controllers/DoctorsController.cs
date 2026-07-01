using Microsoft.AspNetCore.Mvc;
using ClinicManagment.Services;
using CityCareClinic;
using System.Collections.Generic;

namespace ClinicManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly ClinicService _clinicService;

        public DoctorsController(ClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Doctor>> GetDoctors()
        {
            return Ok(_clinicService.DoctorManager.Doctors);
        }

        [HttpPost]
        public IActionResult AddDoctor([FromBody] Doctor doctor)
        {
            if (doctor == null || string.IsNullOrEmpty(doctor.Name) || string.IsNullOrEmpty(doctor.Username) || string.IsNullOrEmpty(doctor.Password))
            {
                return BadRequest(new { message = "Invalid doctor data." });
            }

            bool success = _clinicService.DoctorManager.AddDoctor(doctor);
            if (success)
            {
                _clinicService.SaveDoctors();
                _clinicService.LogActivity($"[ADMIN] Added Doctor: {doctor.Name}");
                return Ok(new { message = "Doctor added successfully." });
            }

            return BadRequest(new { message = "Failed to add doctor (limit reached or username already exists)." });
        }

        [HttpPut]
        public IActionResult UpdateDoctor([FromBody] Doctor doctor)
        {
            if (doctor == null || string.IsNullOrEmpty(doctor.Name))
            {
                return BadRequest(new { message = "Invalid doctor data." });
            }

            bool success = _clinicService.DoctorManager.UpdateDoctor(doctor.Name, doctor.Qualification, doctor.Specialization, doctor.Experience);
            if (success)
            {
                _clinicService.SaveDoctors();
                _clinicService.LogActivity($"[ADMIN] Updated Doctor details for: {doctor.Name}");
                return Ok(new { message = "Doctor details updated successfully." });
            }

            return NotFound(new { message = "Doctor not found." });
        }

        [HttpDelete("{name}")]
        public IActionResult DeleteDoctor(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(new { message = "Doctor name is required." });
            }

            bool success = _clinicService.DoctorManager.DeleteDoctor(name);
            if (success)
            {
                _clinicService.SaveDoctors();
                _clinicService.LogActivity($"[ADMIN] Removed Doctor: {name}");
                return Ok(new { message = "Doctor removed successfully." });
            }

            return NotFound(new { message = "Doctor not found." });
        }

        [HttpGet("recommend")]
        public ActionResult<IEnumerable<Doctor>> GetRecommendation([FromQuery] string symptoms)
        {
            if (string.IsNullOrEmpty(symptoms))
            {
                return BadRequest(new { message = "Symptoms query is required." });
            }

            List<Doctor> docs = _clinicService.DoctorManager.RecommendDoctorApi(symptoms);
            return Ok(docs);
        }

        public class ChangePasswordRequest
        {
            public string Username { get; set; } = string.Empty;
            public string OldPassword { get; set; } = string.Empty;
            public string NewPassword { get; set; } = string.Empty;
        }

        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username))
            {
                return BadRequest(new { message = "Username and passwords are required." });
            }

            bool success = _clinicService.DoctorManager.ChangePassword(request.Username, request.OldPassword, request.NewPassword);
            if (success)
            {
                _clinicService.SaveDoctors();
                _clinicService.LogActivity($"[DOCTOR] {request.Username} changed password");
                return Ok(new { message = "Password changed successfully." });
            }

            return BadRequest(new { message = "Incorrect current password or doctor not found." });
        }
    }
}
