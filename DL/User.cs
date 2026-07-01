using System;

namespace CityCareClinic
{
    // ==========================================
    //  Abstract Base Class - User (Abstraction & Inheritance)
    // ==========================================
    public abstract class User
    {
        // Encapsulation - private fields with public properties
        private string name;
        private string password;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        // Authentication method shared by all users
        public bool Authenticate(string inputName, string inputPassword)
        {
            return Name == inputName && Password == inputPassword;
        }
        // Polymorphism - abstract method to be overridden by derived classes
        public abstract void DisplayInfo();
    }
}
