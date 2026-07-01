using System;
using System.Collections.Generic;
using System.IO;
using CityCareClinic;

namespace ClinicManagment.Services
{
    public class ClinicService
    {
        public DoctorManager DoctorManager { get; } = new DoctorManager();
        public PatientManager PatientManager { get; } = new PatientManager();
        public AppointmentManager AppointmentManager { get; } = new AppointmentManager();

        private const string PatientsFile = "patients.txt";
        private const string DoctorsFile = "doctors.txt";
        private const string AppointmentsFile = "appointments.txt";
        private const string HistoryFile = "activity_history.txt";

        public ClinicService()
        {
            Initialize();
        }

        public void Initialize()
        {
            DoctorManager.LoadFromFile(DoctorsFile);
            PatientManager.LoadFromFile(PatientsFile);
            AppointmentManager.LoadFromFile(AppointmentsFile);
        }

        public void SaveDoctors()
        {
            DoctorManager.SaveToFile(DoctorsFile);
        }

        public void SavePatients()
        {
            PatientManager.SaveToFile(PatientsFile);
        }

        public void SaveAppointments()
        {
            AppointmentManager.SaveToFile(AppointmentsFile);
        }

        public void LogActivity(string activity)
        {
            string formattedLog = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {activity}\n";
            FileManager.SaveHistory(formattedLog, HistoryFile);
        }

        public List<string> GetHistory()
        {
            List<string> logs = new List<string>();
            if (!File.Exists(HistoryFile))
            {
                return logs;
            }
            try
            {
                logs.AddRange(File.ReadAllLines(HistoryFile));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading history: {ex.Message}");
            }
            return logs;
        }
    }
}
