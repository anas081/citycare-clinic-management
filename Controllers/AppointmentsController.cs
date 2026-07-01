using Microsoft.AspNetCore.Mvc;
using ClinicManagment.Services;
using CityCareClinic;
using System.Collections.Generic;
using System.Linq;

namespace ClinicManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly ClinicService _clinicService;

        public AppointmentsController(ClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Appointment>> GetAppointments()
        {
            return Ok(_clinicService.AppointmentManager.Appointments);
        }

        [HttpGet("slots")]
        public IActionResult GetSlots([FromQuery] string doctorName, [FromQuery] string date)
        {
            if (string.IsNullOrEmpty(doctorName) || string.IsNullOrEmpty(date))
            {
                return BadRequest(new { message = "Doctor name and date are required." });
            }

            string[] slots = AppointmentManager.GetTimeSlots();
            var result = slots.Select(slot => new
            {
                timeSlot = slot,
                available = _clinicService.AppointmentManager.IsTimeSlotAvailable(doctorName, date, slot)
            }).ToList();

            return Ok(result);
        }

        public class BookRequest
        {
            public string PatientName { get; set; } = string.Empty;
            public string DoctorName { get; set; } = string.Empty;
            public string Date { get; set; } = string.Empty; // DD/MM/YYYY
            public string TimeSlot { get; set; } = string.Empty;
        }

        [HttpPost("book")]
        public IActionResult Book([FromBody] BookRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.PatientName) || string.IsNullOrEmpty(request.DoctorName) || string.IsNullOrEmpty(request.Date) || string.IsNullOrEmpty(request.TimeSlot))
            {
                return BadRequest(new { message = "All booking parameters are required." });
            }

            bool success = _clinicService.AppointmentManager.BookAppointmentApi(request.PatientName, request.DoctorName, request.Date, request.TimeSlot);
            if (success)
            {
                _clinicService.SaveAppointments();
                _clinicService.LogActivity($"[PATIENT] {request.PatientName} booked appointment with Dr. {request.DoctorName} on {request.Date} ({request.TimeSlot})");
                return Ok(new { message = "Appointment booked successfully." });
            }

            return BadRequest(new { message = "Failed to book appointment. The time slot might already be booked or limit reached." });
        }

        public class CancelRequest
        {
            public string PatientName { get; set; } = string.Empty;
            public string DoctorName { get; set; } = string.Empty;
            public string Date { get; set; } = string.Empty;
            public string TimeSlot { get; set; } = string.Empty;
        }

        [HttpPost("cancel")]
        public IActionResult Cancel([FromBody] CancelRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.PatientName) || string.IsNullOrEmpty(request.DoctorName) || string.IsNullOrEmpty(request.Date) || string.IsNullOrEmpty(request.TimeSlot))
            {
                return BadRequest(new { message = "All parameters are required to cancel an appointment." });
            }

            bool success = _clinicService.AppointmentManager.CancelAppointmentApi(request.PatientName, request.DoctorName, request.Date, request.TimeSlot);
            if (success)
            {
                _clinicService.SaveAppointments();
                _clinicService.LogActivity($"[PATIENT] {request.PatientName} cancelled appointment with Dr. {request.DoctorName} on {request.Date} ({request.TimeSlot})");
                return Ok(new { message = "Appointment cancelled successfully." });
            }

            return NotFound(new { message = "Appointment not found or already cancelled." });
        }

        public class RescheduleRequest
        {
            public string PatientName { get; set; } = string.Empty;
            public string DoctorName { get; set; } = string.Empty;
            public string OldDate { get; set; } = string.Empty;
            public string OldTimeSlot { get; set; } = string.Empty;
            public string NewDate { get; set; } = string.Empty;
            public string NewTimeSlot { get; set; } = string.Empty;
        }

        [HttpPost("reschedule")]
        public IActionResult Reschedule([FromBody] RescheduleRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.PatientName) || string.IsNullOrEmpty(request.DoctorName) || string.IsNullOrEmpty(request.NewDate) || string.IsNullOrEmpty(request.NewTimeSlot))
            {
                return BadRequest(new { message = "All parameters are required to reschedule." });
            }

            bool success = _clinicService.AppointmentManager.RescheduleAppointmentApi(
                request.PatientName, request.DoctorName, request.OldDate, request.OldTimeSlot, request.NewDate, request.NewTimeSlot);

            if (success)
            {
                _clinicService.SaveAppointments();
                _clinicService.LogActivity($"[PATIENT] {request.PatientName} rescheduled appointment with Dr. {request.DoctorName} from {request.OldDate} ({request.OldTimeSlot}) to {request.NewDate} ({request.NewTimeSlot})");
                return Ok(new { message = "Appointment rescheduled successfully." });
            }

            return BadRequest(new { message = "Failed to reschedule appointment. The new time slot might already be booked or original appointment not found." });
        }
    }
}
