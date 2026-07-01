using System;
using System.Collections.Generic;
using System.IO;

namespace CityCareClinic
{
    static class FileManager
    {
        // ---------- Load Doctors from File ----------
        public static List<Doctor> LoadDoctors(string filePath)
        {
            List<Doctor> doctors = new List<Doctor>();
            if (!File.Exists(filePath))
                return doctors;
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                int i = 0;
                while (i < lines.Length)
                {
                    string line = lines[i].Trim();
                    if (line == "1") // exists flag is true
                    {
                        if (i + 6 < lines.Length)
                        {
                            Doctor doc = new Doctor();
                            doc.Name = lines[i + 1].Trim();
                            doc.Qualification = lines[i + 2].Trim();
                            doc.Specialization = lines[i + 3].Trim();
                            doc.Username = lines[i + 4].Trim();
                            doc.Password = lines[i + 5].Trim();
                            int exp;
                            int.TryParse(lines[i + 6].Trim(), out exp);
                            doc.Experience = exp;
                            doctors.Add(doc);
                            i += 7;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        i++; // skip non-existing entry (just the "0" line)
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading doctors: {ex.Message}");
            }
            return doctors;
        }
        // ---------- Save Doctors to File ----------
        public static void SaveDoctors(List<Doctor> doctors, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (Doctor doc in doctors)
                    {
                        writer.WriteLine("1");
                        writer.WriteLine(doc.Name);
                        writer.WriteLine(doc.Qualification);
                        writer.WriteLine(doc.Specialization);
                        writer.WriteLine(doc.Username);
                        writer.WriteLine(doc.Password);
                        writer.WriteLine(doc.Experience);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving doctors: {ex.Message}");
            }
        }
        // ---------- Load Patients from File ----------
        public static List<Patient> LoadPatients(string filePath)
        {
            List<Patient> patients = new List<Patient>();
            if (!File.Exists(filePath))
                return patients;
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                int i = 0;
                while (i < lines.Length)
                {
                    string line = lines[i].Trim();
                    if (line == "1") // exists flag is true
                    {
                        if (i + 5 < lines.Length)
                        {
                            Patient pat = new Patient();
                            pat.Name = lines[i + 1].Trim();
                            pat.Password = lines[i + 2].Trim();
                            pat.Contact = lines[i + 3].Trim();
                            pat.Symptoms = lines[i + 4].Trim();
                            int age;
                            int.TryParse(lines[i + 5].Trim(), out age);
                            pat.Age = age;
                            patients.Add(pat);
                            i += 6;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        i++; // skip non-existing entry
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading patients: {ex.Message}");
            }
            return patients;
        }
        // ---------- Save Patients to File ----------
        public static void SavePatients(List<Patient> patients, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (Patient pat in patients)
                    {
                        writer.WriteLine("1");
                        writer.WriteLine(pat.Name);
                        writer.WriteLine(pat.Password);
                        writer.WriteLine(pat.Contact);
                        writer.WriteLine(pat.Symptoms);
                        writer.WriteLine(pat.Age);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving patients: {ex.Message}");
            }
        }
        // ---------- Load Appointments from File ----------
        public static List<Appointment> LoadAppointments(string filePath)
        {
            List<Appointment> appointments = new List<Appointment>();
            if (!File.Exists(filePath))
                return appointments;
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                int i = 0;
                while (i < lines.Length)
                {
                    string line = lines[i].Trim();
                    if (line == "1") // exists flag is true
                    {
                        if (i + 5 < lines.Length)
                        {
                            Appointment appt = new Appointment();
                            appt.PatientName = lines[i + 1].Trim();
                            appt.DoctorName = lines[i + 2].Trim();
                            appt.Date = lines[i + 3].Trim();
                            appt.TimeSlot = lines[i + 4].Trim();
                            appt.Status = lines[i + 5].Trim();
                            appointments.Add(appt);
                            i += 6;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        i++; // skip non-existing entry
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading appointments: {ex.Message}");
            }
            return appointments;
        }
        // ---------- Save Appointments to File ----------
        public static void SaveAppointments(List<Appointment> appointments, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (Appointment appt in appointments)
                    {
                        writer.WriteLine("1");
                        writer.WriteLine(appt.PatientName);
                        writer.WriteLine(appt.DoctorName);
                        writer.WriteLine(appt.Date);
                        writer.WriteLine(appt.TimeSlot);
                        writer.WriteLine(appt.Status);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving appointments: {ex.Message}");
            }
        }
        // ---------- Save History to File ----------
        public static void SaveHistory(string history, string filePath)
        {
            try
            {
                File.AppendAllText(filePath, history);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving history: {ex.Message}");
            }
        }
        // ---------- View History from File ----------
        public static void ViewHistory(string filePath)
        {
            Console.WriteLine("\n========== ACTIVITY HISTORY ==========");
            if (!File.Exists(filePath))
            {
                Console.WriteLine("No history available yet!");
                return;
            }
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading history: {ex.Message}");
            }
        }
    }
}
