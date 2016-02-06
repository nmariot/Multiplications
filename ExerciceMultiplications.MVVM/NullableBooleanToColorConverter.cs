using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace ExerciceMultiplications.MVVM
{
    public class NullableBooleanToColorConverter : IValueConverter
    {
        /// <summary>
        /// Convertir un boolean nullable en une couleur (null -> Noir, Vrai -> Vert, Faux -> Rouge)
        /// </summary>
        /// <param name="value">La valeur</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((bool?)value)
            {
                case true:
                    return Brushes.Green;

                case false:
                    return Brushes.Red;

                case null:
                default:
                    return Brushes.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (((SolidColorBrush)value).ToString())
            {
                case "Green":
                    return true;

                case "Red":
                    return false;

                default:
                    return null;
            }

        }
    }
}
