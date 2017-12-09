using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RestaurantPro.Annotations;

namespace RestaurantPro.Models
{
    public class MVUser : INotifyPropertyChanged, IDataErrorInfo
    {
        private string username;
        private string firstName;
        private string lastName;
        
        
        /// <summary>
        /// Initialized a news instance of User
        /// </summary>
        public MVUser(string userUsername, string userFirstName, string userLastName)
        {
            Username = userUsername;
            FirstName = userFirstName;
            LastName = userLastName;
        }

        public MVUser(){ }
        
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged();
            }
        }
        
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged();
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

        #region IDataErrorInfo Members

        public string Error { get; private set; }

        public string this[string columnName]
        {
            get 
            {
                if (columnName == "Username")
                {
                    if (String.IsNullOrWhiteSpace(Username))
                    {
                        Error = "Username cannot be null";
                    }
                    else
                    {
                        Error = null;
                    }
                }

                return Error;
            }
        }

        

        #endregion
    }
}
