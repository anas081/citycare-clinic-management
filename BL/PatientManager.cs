using System;
using System.Collections.Generic;

namespace CityCareClinic
{
    // ==========================================
    //  PatientManager - Manages all Patient operations
    // ==========================================
    public class PatientManager
    {
        // Encapsulation - private list with public methods
        private List<Patient> patients;
        private const int MaxPatients = 50;
        // Constructor
        public PatientManager()
        {
            patients = new List<Patient>();
        }
        // Property to access patients list (read-only)
        public List<Patient> Patients
        {
            get { return patients; }
        }
        // ---------- Load patients from file ----------
        public void LoadFromFile(string filePath)
        {
            patients = FileManager.LoadPatients(filePath);
        }
        // ---------- Save patients to file ----------
        public void SaveToFile(string filePath)
        {
            FileManager.SavePatients(patients, filePath);
        }
        // ---------- Add Patient ----------
        public void AddPatient()
        {
            if (patients.Count >= MaxPatients)
            {
                Console.WriteLine("\nPatient limit reached!");
                return;
            }
            Patient pat = new Patient();
            Console.Write("Patient Name: ");
            pat.Name = Console.ReadLine();
            Console.Write("Age: ");
            int age;
            int.TryParse(Console.ReadLine(), out age);
            pat.Age = age;
            Console.Write("Contact: ");
            pat.Contact = Console.ReadLine();
            Console.Write("Symptoms: ");
            pat.Symptoms = Console.ReadLine();
            Console.Write("Assign Password: ");
            pat.Password = Console.ReadLine();
            patients.Add(pat);
            Console.WriteLine("\nPatient added successfully!");
            Console.WriteLine($"Name: {pat.Name} | Password: {pat.Password}");
        }
        // ---------- Update Patient ----------
        public void UpdatePatient()
        {
            Console.Write("Enter patient name: ");
            string name = Console.ReadLine();
            Patient pat = FindByName(name);
            if (pat == null)
            {
                Console.WriteLine("\nPatient not found!");
                return;
            }
            Console.Write("New Age: ");
            int age;
            int.TryParse(Console.ReadLine(), out age);
            pat.Age = age;
            Console.Write("New Contact: ");
            pat.Contact = Console.ReadLine();
            Console.Write("New Symptoms: ");
            pat.Symptoms = Console.ReadLine();
            Console.WriteLine("\nRecord updated!");
        }
        // ---------- View All Patients (Polymorphism in action) ----------
        public void ViewAllPatients()
        {
            Console.WriteLine("\n========== ALL PATIENTS ==========");
            if (patients.Count == 0)
            {
                Console.WriteLine("\nNo patients available!");
                return;
            }
            bool found = false;
            foreach (Patient pat in patients)
            {
                pat.DisplayInfo(); // Polymorphic call
                found = true;
            }
            if (!found)
                Console.WriteLine("\nNo patients available!");
        }
        // ---------- Delete Patient ----------
        public void DeletePatient()
        {
            Console.Write("Enter patient name: ");
            string name = Console.ReadLine();
            Patient pat = FindByName(name);
            if (pat == null)
            {
                Console.WriteLine("\nPatient not found!");
            }
            else
            {
                patients.Remove(pat);
                Console.WriteLine("\nPatient deleted!");
            }
        }
        // ---------- Find Patient by Name ----------
        public Patient FindByName(string name)
        {
            foreach (Patient pat in patients)
            {
                if (pat.Name == name)
                    return pat;
            }
            return null;
        }
        // ---------- Authenticate Patient ----------
        public Patient AuthenticatePatient(string name, string password)
        {
            foreach (Patient pat in patients)
            {
                if (pat.Authenticate(name, password)) // Using inherited method
                    return pat;
            }
            return null;
        }

        // API Method: Add Patient
        public bool AddPatient(Patient pat)
        {
            if (patients.Count >= MaxPatients)
                return false;
            if (FindByName(pat.Name) != null)
                return false;
            patients.Add(pat);
            return true;
        }

        // API Method: Update Patient
        public bool UpdatePatient(string name, int age, string contact, string symptoms)
        {
            Patient pat = FindByName(name);
            if (pat == null)
                return false;
            pat.Age = age;
            pat.Contact = contact;
            pat.Symptoms = symptoms;
            return true;
        }

        // API Method: Delete Patient
        public bool DeletePatient(string name)
        {
            Patient pat = FindByName(name);
            if (pat == null)
                return false;
            patients.Remove(pat);
            return true;
        }

        // API Method: Change Password
        public bool ChangePassword(string name, string oldPassword, string newPassword)
        {
            Patient pat = FindByName(name);
            if (pat == null || pat.Password != oldPassword)
                return false;
            pat.Password = newPassword;
            return true;
        }
    }
}
