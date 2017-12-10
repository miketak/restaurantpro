using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RestaurantPro.Annotations;

namespace RestaurantPro.ViewModels
{
    internal class CentralDashboardViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;

        /// <summary>
        /// Gets or sets some information
        /// </summary>
        public String FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string FullName
        {
            get { return _firstName + " " + _lastName; }
        }


        #region INotifyPropertyChanged Members
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
