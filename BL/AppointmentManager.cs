using System;
using System.Collections.Generic;

namespace CityCareClinic
{
    // ==========================================
    //  AppointmentManager - Manages all Appointment operations
    // ==========================================
    public class AppointmentManager
    {
        // Encapsulation - private list with public methods
        private List<Appointment> appointments;
        private const int MaxAppointments = 100;
        // Available time slots
        private static readonly string[] TimeSlots = {
            "09:00 AM - 10:00 AM",
            "10:00 AM - 11:00 AM",
            "11:00 AM - 12:00 PM",
            "02:00 PM - 03:00 PM",
            "03:00 PM - 04:00 PM",
            "04:00 PM - 05:00 PM"
        };
        // Constructor
        public AppointmentManager()
        {
            appointments = new List<Appointment>();
        }
        // ---------- Load appointments from file ----------
        public void LoadFromFile(string filePath)
        {
            appointments = FileManager.LoadAppointments(filePath);
        }
        // ---------- Save appointments to file ----------
        public void SaveToFile(string filePath)
        {
            FileManager.SaveAppointments(appointments, filePath);
        }
        // ---------- Check Time Slot Availability ----------
        public bool IsTimeSlotAvailable(string doctorName, string date, string timeSlot)
        {
            foreach (Appointment appt in appointments)
            {
                if (appt.DoctorName == doctorName &&
                    appt.Date == date &&
                    appt.TimeSlot == timeSlot &&
                    appt.Status == "Confirmed")
                {
                    return false; // Slot is already booked
                }
            }
            return true; // Slot is available
        }
        // ---------- Display Time Slots ----------
        private void DisplayTimeSlots()
        {
            Console.WriteLine("\nAvailable Time Slots:");
            for (int i = 0; i < TimeSlots.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {TimeSlots[i]}");
            }
        }
        // ---------- Book Appointment ----------
        public void BookAppointment(DoctorManager doctorMgr, string patientName)
        {
            if (appointments.Count >= MaxAppointments)
            {
                Console.WriteLine("\nAppointment slots full!");
                return;
            }
            List<Doctor> availableDoctors = doctorMgr.Doctors;
            if (availableDoctors.Count == 0)
            {
                Console.WriteLine("\nNo doctors available!");
                return;
            }
            Console.WriteLine("\n========== AVAILABLE DOCTORS ==========");
            for (int i = 0; i < availableDoctors.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Dr. {availableDoctors[i].Name} " +
                    $"({availableDoctors[i].Specialization}) - " +
                    $"{availableDoctors[i].Experience} years exp");
            }
            Console.Write($"\nSelect Doctor (1-{availableDoctors.Count}): ");
            int choice;
            int.TryParse(Console.ReadLine(), out choice);
            if (choice < 1 || choice > availableDoctors.Count)
            {
                Console.WriteLine("\nInvalid selection!");
                return;
            }
            string selectedDoctor = availableDoctors[choice - 1].Name;
            Console.Write("Enter Date (DD/MM/YYYY): ");
            string appointmentDate = Console.ReadLine();
            DisplayTimeSlots();
            Console.Write("Select Time Slot (1-6): ");
            int slotChoice;
            int.TryParse(Console.ReadLine(), out slotChoice);
            if (slotChoice < 1 || slotChoice > 6)
            {
                Console.WriteLine("\nInvalid time slot!");
                return;
            }
            string appointmentTime = TimeSlots[slotChoice - 1];
            // Check if time slot is available
            if (!IsTimeSlotAvailable(selectedDoctor, appointmentDate, appointmentTime))
            {
                Console.WriteLine("\nSorry! This time slot is already booked.");
                Console.WriteLine("Please select a different time slot or date.");
                return;
            }
            Appointment newAppt = new Appointment(
                patientName, selectedDoctor, appointmentDate,
                appointmentTime, "Confirmed"
            );
            appointments.Add(newAppt);
            Console.WriteLine("\n✓ Appointment booked successfully!");
            Console.WriteLine($"Doctor: Dr. {selectedDoctor}");
            Console.WriteLine($"Date: {appointmentDate}");
            Console.WriteLine($"Time: {appointmentTime}");
        }
        // ---------- Cancel Appointment ----------
        public void CancelAppointment(string patientName)
        {
            List<Appointment> patientAppts = new List<Appointment>();
            Console.WriteLine("\n========== YOUR APPOINTMENTS ==========");
            int count = 0;
            foreach (Appointment appt in appointments)
            {
                if (appt.PatientName == patientName && appt.Status == "Confirmed")
                {
                    count++;
                    patientAppts.Add(appt);
                    Console.WriteLine($"{count}. Dr. {appt.DoctorName} | " +
                        $"Date: {appt.Date} | Time: {appt.TimeSlot}");
                }
            }
            if (count == 0)
            {
                Console.WriteLine("\nNo active appointments found!");
                return;
            }
            Console.Write($"\nSelect appointment to cancel (1-{count}): ");
            int choice;
            int.TryParse(Console.ReadLine(), out choice);
            if (choice < 1 || choice > count)
            {
                Console.WriteLine("\nInvalid selection!");
                return;
            }
            patientAppts[choice - 1].Status = "Cancelled";
            Console.WriteLine("\n✓ Appointment cancelled successfully!");
        }
        // ---------- Reschedule Appointment ----------
        public void RescheduleAppointment(string patientName)
        {
            List<Appointment> patientAppts = new List<Appointment>();
            Console.WriteLine("\n========== YOUR APPOINTMENTS ==========");
            int count = 0;
            foreach (Appointment appt in appointments)
            {
                if (appt.PatientName == patientName && appt.Status == "Confirmed")
                {
                    count++;
                    patientAppts.Add(appt);
                    Console.WriteLine($"{count}. Dr. {appt.DoctorName} | " +
                        $"Date: {appt.Date} | Time: {appt.TimeSlot}");
                }
            }
            if (count == 0)
            {
                Console.WriteLine("\nNo active appointments found!");
                return;
            }
            Console.Write($"\nSelect appointment to reschedule (1-{count}): ");
            int choice;
            int.TryParse(Console.ReadLine(), out choice);
            if (choice < 1 || choice > count)
            {
                Console.WriteLine("\nInvalid selection!");
                return;
            }
            Appointment selectedAppt = patientAppts[choice - 1];
            string doctorName = selectedAppt.DoctorName;
            Console.Write("\nEnter new date (DD/MM/YYYY): ");
            string newDate = Console.ReadLine();
            DisplayTimeSlots();
            Console.Write("Select new time slot (1-6): ");
            int slotChoice;
            int.TryParse(Console.ReadLine(), out slotChoice);
            if (slotChoice < 1 || slotChoice > 6)
            {
                Console.WriteLine("\nInvalid time slot!");
                return;
            }
            string newTime = TimeSlots[slotChoice - 1];
            // Check if new time slot is available
            if (!IsTimeSlotAvailable(doctorName, newDate, newTime))
            {
                Console.WriteLine("\nSorry! This time slot is already booked.");
                Console.WriteLine("Please select a different time slot or date.");
                return;
            }
            selectedAppt.Date = newDate;
            selectedAppt.TimeSlot = newTime;
            Console.WriteLine("\n✓ Appointment rescheduled successfully!");
            Console.WriteLine($"New Date: {newDate}");
            Console.WriteLine($"New Time: {newTime}");
        }
        // ---------- View Patient Appointments ----------
        public void ViewPatientAppointments(string patientName)
        {
            Console.WriteLine("\n========== MY APPOINTMENTS ==========");
            bool found = false;
            foreach (Appointment appt in appointments)
            {
                if (appt.PatientName == patientName)
                {
                    Console.WriteLine($"\nDoctor: Dr. {appt.DoctorName}");
                    Console.WriteLine($"Date: {appt.Date}");
                    Console.WriteLine($"Time: {appt.TimeSlot}");
                    Console.WriteLine($"Status: {appt.Status}");
                    Console.WriteLine("---------------------------------");
                    found = true;
                }
            }
            if (!found)
                Console.WriteLine("\nNo appointments found!");
        }
        // ---------- View Doctor Appointments ----------
        public void ViewDoctorAppointments(string doctorName)
        {
            Console.WriteLine("\n========== MY APPOINTMENTS ==========");
            bool found = false;
            foreach (Appointment appt in appointments)
            {
                if (appt.DoctorName == doctorName && appt.Status == "Confirmed")
                {
                    Console.WriteLine($"\nPatient: {appt.PatientName}");
                    Console.WriteLine($"Date: {appt.Date}");
                    Console.WriteLine($"Time: {appt.TimeSlot}");
                    Console.WriteLine("---------------------------------");
                    found = true;
                }
            }
            if (!found)
                Console.WriteLine("\nNo appointments scheduled!");
        }
        // ---------- View All Appointments ----------
        public void ViewAllAppointments()
        {
            Console.WriteLine("\n========== ALL APPOINTMENTS ==========");
            if (appointments.Count == 0)
            {
                Console.WriteLine("\nNo appointments yet!");
                return;
            }
            bool found = false;
            foreach (Appointment appt in appointments)
            {
                appt.DisplayInfo();
                found = true;
            }
            if (!found)
                Console.WriteLine("\nNo appointments yet!");
        }

        // Expose appointments list
        public List<Appointment> Appointments
        {
            get { return appointments; }
        }

        // Expose time slots list
        public static string[] GetTimeSlots()
        {
            return TimeSlots;
        }

        // API Method: Book Appointment
        public bool BookAppointmentApi(string patientName, string doctorName, string date, string timeSlot)
        {
            if (appointments.Count >= MaxAppointments)
                return false;
            if (!IsTimeSlotAvailable(doctorName, date, timeSlot))
                return false;

            Appointment newAppt = new Appointment(
                patientName, doctorName, date, timeSlot, "Confirmed"
            );
            appointments.Add(newAppt);
            return true;
        }

        // API Method: Cancel Appointment
        public bool CancelAppointmentApi(string patientName, string doctorName, string date, string timeSlot)
        {
            foreach (Appointment appt in appointments)
            {
                if (appt.PatientName == patientName &&
                    appt.DoctorName == doctorName &&
                    appt.Date == date &&
                    appt.TimeSlot == timeSlot &&
                    appt.Status == "Confirmed")
                {
                    appt.Status = "Cancelled";
                    return true;
                }
            }
            return false;
        }

        // API Method: Reschedule Appointment
        public bool RescheduleAppointmentApi(string patientName, string doctorName, string oldDate, string oldTimeSlot, string newDate, string newTimeSlot)
        {
            if (!IsTimeSlotAvailable(doctorName, newDate, newTimeSlot))
                return false;

            foreach (Appointment appt in appointments)
            {
                if (appt.PatientName == patientName &&
                    appt.DoctorName == doctorName &&
                    appt.Date == oldDate &&
                    appt.TimeSlot == oldTimeSlot &&
                    appt.Status == "Confirmed")
                {
                    appt.Date = newDate;
                    appt.TimeSlot = newTimeSlot;
                    return true;
                }
            }
            return false;
        }
    }
}
