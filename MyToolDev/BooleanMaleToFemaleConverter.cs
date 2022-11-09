using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyToolDev
{
    public  class BooleanMaleToFemaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // return (bool)value ? "male" : "female";
            return (bool)value ? "\u2642" : "\u2640";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().ToLower().Equals("male");
        }


    }
}
