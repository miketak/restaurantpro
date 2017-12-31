using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RestaurantPro.Annotations;

namespace RestaurantPro
{
    /// <summary>
    /// Base class for INotifyPropetyChanged Implementation.
    /// </summary>
    public class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// INotified Property Setting Implementation
        /// Activated on Property Changed event fire.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="val"></param>
        /// <param name="propertyName"></param>
        /// <typeparam name="T"></typeparam>
        protected virtual void SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val))
                return;

            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Method invoked on property changed event
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }        

        /// <summary>
        /// Property Changed Event Handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
