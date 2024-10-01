using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using MarkRestaurant.Models;

namespace MarkRestaurant
{
    public class User : IdentityUser
    {
        public User()
        {
            Email = "";
            PasswordHash = "";
            UserName = "";
            Name = "";
            Surname = "";
            MiddleName = "";
            Age = 0;
            PhoneNumber = "";
            ProfileImagePath = "/images/person.jpg";
            Order basket = new Order();
        }

        public User(string email, string passwordHash)
        {
            Email = email;
            PasswordHash = passwordHash;
            UserName = email;
            Name = "";
            Surname = "";
            MiddleName = "";
            Age = 0;
            PhoneNumber = "";
            ProfileImagePath = "/images/person.jpg";
        }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Surname { get; set; }

        [MaxLength(100)]
        public string MiddleName { get; set; }

        [MaxLength(100)]
        public int Age { get; set; }

        public string ProfileImagePath { get; set; }
    }
}
