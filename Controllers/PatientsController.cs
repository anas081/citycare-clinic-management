using Microsoft.AspNetCore.Mvc;
using ClinicManagment.Services;
using CityCareClinic;
using System.Collections.Generic;

namespace ClinicManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly ClinicService _clinicService;

        public PatientsController(ClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Patient>> GetPatients()
        {
            return Ok(_clinicService.PatientManager.Patients);
        }

        [HttpPost]
        public IActionResult AddPatient([FromBody] Patient patient)
        {
            if (patient == null || string.IsNullOrEmpty(patient.Name) || string.IsNullOrEmpty(patient.Password))
            {
                return BadRequest(new { message = "Invalid patient data." });
            }

            bool success = _clinicService.PatientManager.AddPatient(patient);
            if (success)
            {
                _clinicService.SavePatients();
                _clinicService.LogActivity($"[ADMIN] Added Patient: {patient.Name}");
                return Ok(new { message = "Patient added successfully." });
            }

            return BadRequest(new { message = "Failed to add patient (limit reached or patient name already exists)." });
        }

        [HttpPut]
        public IActionResult UpdatePatient([FromBody] Patient patient)
        {
            if (patient == null || string.IsNullOrEmpty(patient.Name))
            {
                return BadRequest(new { message = "Invalid patient data." });
            }

            bool success = _clinicService.PatientManager.UpdatePatient(patient.Name, patient.Age, patient.Contact, patient.Symptoms);
            if (success)
            {
                _clinicService.SavePatients();
                _clinicService.LogActivity($"[ADMIN] Updated Patient details for: {patient.Name}");
                return Ok(new { message = "Patient details updated successfully." });
            }

            return NotFound(new { message = "Patient not found." });
        }

        [HttpDelete("{name}")]
        public IActionResult DeletePatient(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(new { message = "Patient name is required." });
            }

            bool success = _clinicService.PatientManager.DeletePatient(name);
            if (success)
            {
                _clinicService.SavePatients();
                _clinicService.LogActivity($"[ADMIN] Removed Patient: {name}");
                return Ok(new { message = "Patient removed successfully." });
            }

            return NotFound(new { message = "Patient not found." });
        }

        public class ChangePasswordRequest
        {
            public string Name { get; set; } = string.Empty;
            public string OldPassword { get; set; } = string.Empty;
            public string NewPassword { get; set; } = string.Empty;
        }

        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Name))
            {
                return BadRequest(new { message = "Patient name and passwords are required." });
            }

            bool success = _clinicService.PatientManager.ChangePassword(request.Name, request.OldPassword, request.NewPassword);
            if (success)
            {
                _clinicService.SavePatients();
                _clinicService.LogActivity($"[PATIENT] {request.Name} changed password");
                return Ok(new { message = "Password changed successfully." });
            }

            return BadRequest(new { message = "Incorrect current password or patient not found." });
        }
    }
}
