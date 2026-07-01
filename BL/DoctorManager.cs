using System;
using System.Collections.Generic;

namespace CityCareClinic
{
    // ==========================================
    //  DoctorManager - Manages all Doctor operations
    // ==========================================
    public class DoctorManager
    {
        // Encapsulation - private list with public methods
        private List<Doctor> doctors;
        private const int MaxDoctors = 50;
        // Constructor
        public DoctorManager()
        {
            doctors = new List<Doctor>();
        }
        // Property to access doctors list (read-only)
        public List<Doctor> Doctors
        {
            get { return doctors; }
        }
        // ---------- Load doctors from file ----------
        public void LoadFromFile(string filePath)
        {
            doctors = FileManager.LoadDoctors(filePath);
        }
        // ---------- Save doctors to file ----------
        public void SaveToFile(string filePath)
        {
            FileManager.SaveDoctors(doctors, filePath);
        }
        // ---------- Add Doctor ----------
        public void AddDoctor()
        {
            if (doctors.Count >= MaxDoctors)
            {
                Console.WriteLine("\nDoctor limit reached!");
                return;
            }
            Doctor doc = new Doctor();
            Console.Write("Doctor Name: ");
            doc.Name = Console.ReadLine();
            Console.Write("Qualification: ");
            doc.Qualification = Console.ReadLine();
            Console.Write("Specialization: ");
            doc.Specialization = Console.ReadLine();
            Console.Write("Experience (years): ");
            int exp;
            int.TryParse(Console.ReadLine(), out exp);
            doc.Experience = exp;
            Console.Write("Assign Username: ");
            doc.Username = Console.ReadLine();
            Console.Write("Assign Password: ");
            doc.Password = Console.ReadLine();
            doctors.Add(doc);
            Console.WriteLine("\nDoctor added successfully!");
            Console.WriteLine($"Username: {doc.Username} | Password: {doc.Password}");
        }
        // ---------- Update Doctor ----------
        public void UpdateDoctor()
        {
            Console.Write("Enter doctor name: ");
            string name = Console.ReadLine();
            Doctor doc = FindByName(name);
            if (doc == null)
            {
                Console.WriteLine("\nDoctor not found!");
                return;
            }
            Console.Write("New Qualification: ");
            doc.Qualification = Console.ReadLine();
            Console.Write("New Specialization: ");
            doc.Specialization = Console.ReadLine();
            Console.Write("New Experience: ");
            int exp;
            int.TryParse(Console.ReadLine(), out exp);
            doc.Experience = exp;
            Console.WriteLine("\nRecord updated!");
        }
        // ---------- View All Doctors (Polymorphism in action) ----------
        public void ViewAllDoctors()
        {
            Console.WriteLine("\n========== ALL DOCTORS ==========");
            if (doctors.Count == 0)
            {
                Console.WriteLine("\nNo doctors available!");
                return;
            }
            bool found = false;
            foreach (Doctor doc in doctors)
            {
                doc.DisplayInfo(); // Polymorphic call
                found = true;
            }
            if (!found)
                Console.WriteLine("\nNo doctors available!");
        }
        // ---------- Delete Doctor ----------
        public void DeleteDoctor()
        {
            Console.Write("Enter doctor name: ");
            string name = Console.ReadLine();
            Doctor doc = FindByName(name);
            if (doc == null)
            {
                Console.WriteLine("\nDoctor not found!");
            }
            else
            {
                doctors.Remove(doc);
                Console.WriteLine("\nDoctor deleted!");
            }
        }
        // ---------- Find Doctor by Name ----------
        public Doctor FindByName(string name)
        {
            foreach (Doctor doc in doctors)
            {
                if (doc.Name == name)
                     return doc;
            }
            return null;
        }
        // ---------- Find Doctor by Username ----------
        public Doctor FindByUsername(string username)
        {
            foreach (Doctor doc in doctors)
            {
                if (doc.Username == username)
                    return doc;
            }
            return null;
        }
        // ---------- Authenticate Doctor ----------
        public Doctor AuthenticateDoctor(string username, string password)
        {
            foreach (Doctor doc in doctors)
            {
                if (doc.Username == username && doc.Password == password)
                    return doc;
            }
            return null;
        }
        // ---------- Display Available Doctors ----------
        public void DisplayAvailableDoctors()
        {
            foreach (Doctor doc in doctors)
            {
                Console.WriteLine($"{doc.Name} ({doc.Specialization})");
            }
        }
        // ---------- Recommend Doctor based on Symptoms ----------
        public void RecommendDoctor(string symptoms)
        {
            string spec;
            if (symptoms == "fever" || symptoms == "cold" || symptoms == "cough")
                spec = "General Physician";
            else if (symptoms == "chest pain" || symptoms == "heart problem")
                spec = "Cardiologist";
            else if (symptoms == "bone pain" || symptoms == "fracture")
                spec = "Orthopedic";
            else if (symptoms == "skin rash" || symptoms == "skin problem")
                spec = "Dermatologist";
            else
            {
                Console.WriteLine("\nConsult General Physician first.");
                spec = "General Physician";
            }
            Console.WriteLine($"\nRecommended: {spec}");
            bool found = false;
            foreach (Doctor doc in doctors)
            {
                if (doc.Specialization == spec)
                {
                    Console.WriteLine($"Dr. {doc.Name} - {doc.Experience} years experience");
                    found = true;
                }
            }
            if (!found)
                Console.WriteLine("No doctor available in this specialization.");
        }
        // ---------- Change Doctor Password ----------
        public void ChangePassword(string username)
        {
            Doctor doc = FindByUsername(username);
            if (doc == null)
            {
                Console.WriteLine("\nError: Doctor not found!");
                return;
            }
            Console.Write("\nEnter current password: ");
            string oldPass = Console.ReadLine();
            if (oldPass != doc.Password)
            {
                Console.WriteLine("\nIncorrect current password!");
                return;
            }
            Console.Write("Enter new password: ");
            string newPass = Console.ReadLine();
            Console.Write("Confirm new password: ");
            string confirmPass = Console.ReadLine();
            if (newPass != confirmPass)
            {
                Console.WriteLine("\nPasswords do not match!");
                return;
            }
            doc.Password = newPass;
            Console.WriteLine("\nPassword changed successfully!");
        }

        // API Method: Add Doctor
        public bool AddDoctor(Doctor doc)
        {
            if (doctors.Count >= MaxDoctors)
                return false;
            if (FindByUsername(doc.Username) != null)
                return false;
            doctors.Add(doc);
            return true;
        }

        // API Method: Update Doctor
        public bool UpdateDoctor(string name, string qualification, string specialization, int experience)
        {
            Doctor doc = FindByName(name);
            if (doc == null)
                return false;
            doc.Qualification = qualification;
            doc.Specialization = specialization;
            doc.Experience = experience;
            return true;
        }

        // API Method: Delete Doctor
        public bool DeleteDoctor(string name)
        {
            Doctor doc = FindByName(name);
            if (doc == null)
                return false;
            doctors.Remove(doc);
            return true;
        }

        // API Method: Change Password
        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            Doctor doc = FindByUsername(username);
            if (doc == null || doc.Password != oldPassword)
                return false;
            doc.Password = newPassword;
            return true;
        }

        // API Method: Get Doctor Recommendations
        public List<Doctor> RecommendDoctorApi(string symptoms)
        {
            string spec;
            string lowerSymptoms = symptoms.ToLower();
            if (lowerSymptoms.Contains("fever") || lowerSymptoms.Contains("cold") || lowerSymptoms.Contains("cough"))
                spec = "General Physician";
            else if (lowerSymptoms.Contains("chest") || lowerSymptoms.Contains("heart") || lowerSymptoms.Contains("cardio"))
                spec = "Cardiologist";
            else if (lowerSymptoms.Contains("bone") || lowerSymptoms.Contains("joint") || lowerSymptoms.Contains("ortho") || lowerSymptoms.Contains("fracture"))
                spec = "Orthopedic";
            else if (lowerSymptoms.Contains("skin") || lowerSymptoms.Contains("rash") || lowerSymptoms.Contains("derma"))
                spec = "Dermatologist";
            else
                spec = "General Physician";

            List<Doctor> recommended = new List<Doctor>();
            foreach (Doctor doc in doctors)
            {
                if (doc.Specialization.Equals(spec, StringComparison.OrdinalIgnoreCase))
                {
                    recommended.Add(doc);
                }
            }
            return recommended;
        }
    }
}
