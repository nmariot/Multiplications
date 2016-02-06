using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace ExerciceMultiplications.MVVM
{
    public class NombreBonnesReponsesVersChaineConverter : IMultiValueConverter
    {
        /// <summary>
        /// Transforme le nombre de bonnes réponses vers une chaine de caractères
        /// </summary>
        /// <param name="value">Liste des valeurs bindées. dans l'ordre ReponseCorrecte, SerieEnCours, Multiplicateur, Multiplicande, ReponseAttendue</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>        
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool reponseCorrecte = (bool) values[0];
            int serieEnCours = (int)values[1];
            int multiplicateur = (int)values[2];
            int multiplicande = (int)values[3];
            int reponseAttendue = (int)values[4];

            if (reponseAttendue == 0)
            {
                // Phase de démarrage : on affiche rien
                return string.Empty;
            }
            if (reponseCorrecte)
            {
                if (serieEnCours == 1)
                {
                    return "Résultat correct : 1ère bonne réponse";
                }
                else
                {
                    return string.Format("Résultat correct : {0} bonnes réponses consécutives", serieEnCours);
                }

            }
            else
            {
                return string.Format("Résultat incorrect. {0} * {1} = {2}", multiplicateur, multiplicande, reponseAttendue);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
