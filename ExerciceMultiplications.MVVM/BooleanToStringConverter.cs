using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace ExerciceMultiplications.MVVM
{
    public class BooleanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string[] valuesList = CheckParameter(parameter);
            
            var val = System.Convert.ToBoolean(value);
            return val ? valuesList[1] : valuesList[0];            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string[] valuesList = CheckParameter(parameter);

            var val = value.ToString();
            return (val == valuesList[1]);            
        }

        /// <summary>
        /// Renvoie un tableau de 2 chaines de caractères en fonction du paramètre
        /// </summary>
        /// <param name="parameter">une chaine de caractère séparé par :</param>
        /// <returns></returns>
        private string[] CheckParameter(object parameter)
        {
            string param = parameter.ToString();
            string[] valuesList = param.Split(';');
            if (valuesList.Length != 2)
            {
                throw new ArgumentException("Le paramètre attendu doit être de la forme chaine1:chaine2");
            }

            return valuesList;
        }
    }
}
