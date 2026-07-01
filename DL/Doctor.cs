using System;

namespace CityCareClinic
{
    // ==========================================
    //  Doctor Class (Inherits from User)
    // ==========================================
    public class Doctor : User
    {
        // Additional properties specific to Doctor
        public string Qualification { get; set; }
        public string Specialization { get; set; }
        public int Experience { get; set; }
        public string Username { get; set; }
        // Constructor
        public Doctor()
        {
            Qualification = "";
            Specialization = "";
            Experience = 0;
            Username = "";
        }
        // Parameterized Constructor
        public Doctor(string name, string qualification, string specialization,
                      int experience, string username, string password)
        {
            Name = name;
            Qualification = qualification;
            Specialization = specialization;
            Experience = experience;
            Username = username;
            Password = password;
        }
        // Polymorphism - overriding DisplayInfo
        public override void DisplayInfo()
        {
            Console.WriteLine($"\nName: {Name}");
            Console.WriteLine($"Specialization: {Specialization}");
            Console.WriteLine($"Experience: {Experience} years");
            Console.WriteLine($"Username: {Username}");
            Console.WriteLine("---------------------------------");
        }
    }
}
