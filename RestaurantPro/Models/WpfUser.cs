using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using RestaurantPro.Annotations;

namespace RestaurantPro.Models
{
    /// <summary>
    /// Temporary Class for 
    /// </summary>
    public class WpfUser : ValidatableBindableBase
    {

        [Key]
        public Guid Id { get; set; }

        private string _username;
        private string _firstName;
        private string _lastName;
        private string _email;

        [Required]
        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value);}
        }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }



    }
}
