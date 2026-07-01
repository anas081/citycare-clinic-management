using System;

namespace CityCareClinic
{
    // ==========================================
    //  Patient Class (Inherits from User)
    // ==========================================
    public class Patient : User
    {
        // Additional properties specific to Patient
        public string Contact { get; set; }
        public string Symptoms { get; set; }
        public int Age { get; set; }
        // Constructor
        public Patient()
        {
            Contact = "";
            Symptoms = "";
            Age = 0;
        }
        // Parameterized Constructor
        public Patient(string name, string password, string contact,
                       string symptoms, int age)
        {
            Name = name;
            Password = password;
            Contact = contact;
            Symptoms = symptoms;
            Age = age;
        }
        // Polymorphism - overriding DisplayInfo
        public override void DisplayInfo()
        {
            Console.WriteLine($"\nName: {Name}");
            Console.WriteLine($"Age: {Age} | Contact: {Contact}");
            Console.WriteLine($"Symptoms: {Symptoms}");
            Console.WriteLine("---------------------------------");
        }
    }
}
