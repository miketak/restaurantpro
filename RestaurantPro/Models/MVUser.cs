using System.ComponentModel;
using System.Runtime.CompilerServices;
using RestaurantPro.Annotations;

namespace RestaurantPro.Models
{
    public class MVUser : INotifyPropertyChanged
    {
        /// <summary>
        /// Initialized a news instance of User
        /// </summary>
        public MVUser(string username, string firstName, string lastName, string password)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
        }

        public MVUser()
        {
            //Username = "";
            //FirstName = "";
            //LastName = "";
            //Password = "";
        }

        private string _Username;
        public string Username
        {
            get { return _Username; }
            set
            {
                _Username = value;
                OnPropertyChanged("Username");
            }
        }

        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                _FirstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set
            {
                _LastName = value;
                OnPropertyChanged("LastName");
            }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                OnPropertyChanged("Password");
            }
        }

        #region InotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


    }
}
