using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementProject.Models
{
    public class User
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string Uid { get; set; }

        public string Password { get; set; }

        public User(string email, string name, string uid)
        {
            this.Email = email;
            this.Name = name;
            this.Uid = uid;
        }
    }
}
