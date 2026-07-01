using System;

namespace CityCareClinic
{
    // ==========================================
    //  Appointment Class
    // ==========================================
    public class Appointment
    {
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string Date { get; set; }
        public string TimeSlot { get; set; }
        public string Status { get; set; }
        // Constructor
        public Appointment()
        {
            PatientName = "";
            DoctorName = "";
            Date = "";
            TimeSlot = "";
            Status = "";
        }
        // Parameterized Constructor
        public Appointment(string patientName, string doctorName,
                           string date, string timeSlot, string status)
        {
            PatientName = patientName;
            DoctorName = doctorName;
            Date = date;
            TimeSlot = timeSlot;
            Status = status;
        }
        // Display appointment info
        public void DisplayInfo()
        {
            Console.WriteLine($"\nPatient: {PatientName} | Doctor: Dr. {DoctorName}");
            Console.WriteLine($"Date: {Date} | Time: {TimeSlot}");
            Console.WriteLine($"Status: {Status}");
            Console.WriteLine("---------------------------------");
        }
    }
}
