using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RestaurantPro.Auxilliary.Converters
{
    /// <summary>
    /// Controls the visibility of the 
    /// Add/Save Button on the Add/Edit View for WorkCycles
    /// </summary>
    public class NegatableBooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Initializes False Visibility
        /// </summary>
        public NegatableBooleanToVisibilityConverter()
        {
            FalseVisibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Covnerter Logic
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bVal;
            bool result = bool.TryParse(value.ToString(), out bVal);
            if (!result)
                return value;
            if (bVal && !Negate) return Visibility.Visible;
            if (bVal && Negate) return FalseVisibility;
            if (!bVal && Negate) return Visibility.Visible;
            if (!bVal && !Negate) return FalseVisibility;
            else return Visibility.Visible;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public bool Negate { get; set; }
        public Visibility FalseVisibility { get; set; }
    }
}