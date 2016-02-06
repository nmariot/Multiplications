using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExerciceMultiplications.MVVM.Properties;
using System.Windows.Controls;

namespace ExerciceMultiplications.MVVM
{
    class MainModel
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Random _rnd = new Random();
        /// <summary>
        /// Nombre de tests de multiplications effectuées
        /// </summary>
        public int NombreMultiplicationsEffectuees { get; private set; }
        /// <summary>
        /// Nombre de multiplications réussies
        /// </summary>
        public int NombreMultiplicationsReussies { get; private set; }
        /// <summary>
        /// Meilleure série de bonne réponse
        /// </summary>
        public int MeilleureSerie { get; private set; }
        /// <summary>
        /// Max de la table
        /// </summary>
        public int TableMax { get; set; }
        /// <summary>
        /// Temps de réflexion
        /// </summary>
        public int DureeReflexion { get; set; }
        /// <summary>
        /// Multiplicande
        /// </summary>
        public int Multiplicande { get; private set; }
        /// <summary>
        /// Multiplicateur
        /// </summary>
        public int Multiplicateur { get; private set; }
        /// <summary>
        /// Résultat attendu de la multiplication
        /// </summary>
        public int ReponseAttendue
        {
            get
            {
                return this.Multiplicande * this.Multiplicateur;
            }
        }
        /// <summary>
        /// A-t-on démarré une série d'exercices ?
        /// </summary>
        public bool ExerciceEnCours { get; private set; }
        /// <summary>
        /// Réponse fournie
        /// </summary>
        public int? ReponseFournie { get; set; }
        /// <summary>
        /// La réponse est elle correcte ?
        /// </summary>
        public bool ReponseCorrecte { get; private set; }
        /// <summary>
        /// Nombre de réponses consécutives correctes
        /// </summary>
        public int SerieEnCours { get; private set; }

        public MainModel()
        {
            _log.Info("****** Instanciantion d'un Model ******");

            this.TableMax = Settings.Default.TableMaxDefaut;
            this.DureeReflexion = Settings.Default.DureeReflexionDefaut;
            this.ExerciceEnCours = false;            
        }

        /// <summary>
        /// Démarrage d'une série d'exercices
        /// </summary>
        public void Demarrer()
        {
            this.ReponseFournie = null;
            this.NombreMultiplicationsEffectuees = 0;
            this.NombreMultiplicationsReussies = 0;
            this.SerieEnCours = 0;
            this.MeilleureSerie = 0;
            this.ExerciceEnCours = true;

            ChoisirMultiplication();
        }

        /// <summary>
        /// Arrêt de la série en cours
        /// </summary>
        public void Arreter()
        {
            _log.InfoFormat("Fin de la session. {0} / {1} multiplications réussies. Meilleure suite : {2}", this.NombreMultiplicationsReussies, this.NombreMultiplicationsEffectuees, this.MeilleureSerie);
            this.NombreMultiplicationsEffectuees = 0;
            this.NombreMultiplicationsReussies = 0;
            this.ExerciceEnCours = false;
            this.MeilleureSerie = 0;
        }

        /// <summary>
        /// Validation de la réponse à l'exercice
        /// </summary>
        public void Valider()
        {
            this.NombreMultiplicationsEffectuees++;
            if (this.ReponseFournie == this.ReponseAttendue)
            {
                this.NombreMultiplicationsReussies++;                
                this.SerieEnCours++;
                this.ReponseCorrecte = true;
                this.MeilleureSerie = Math.Max(this.MeilleureSerie, this.SerieEnCours);
                _log.InfoFormat("Résultat correct : {0} bonnes réponses consécutives", this.SerieEnCours);
            }
            else
            {
                this.ReponseCorrecte = false;
                this.SerieEnCours = 0;
                _log.Info("Résultat incorrect : " + this.ReponseFournie);
            }

            this.ChoisirMultiplication();
        }

        /// <summary>
        /// Définit le multiplicande et le multiplicateur ainsi que le résultat attendu
        /// </summary>
        private void ChoisirMultiplication()
        {
            this.Multiplicateur = _rnd.Next(9) + 1;
            int nbMin = 1;
            if (this.MeilleureSerie >= 10)
            {
                nbMin = this.TableMax * 3 / 4 + 1;
            }
            else if (this.MeilleureSerie >= 5)
            {
                nbMin = this.TableMax * 2 / 4 + 1;
            }
            else if (this.MeilleureSerie >= 2)
            {
                nbMin = this.TableMax / 4 + 1;
            }
            _log.DebugFormat("Nb min = {0}", nbMin);
            this.Multiplicande = _rnd.Next(this.TableMax - nbMin + 1) + nbMin;

            _log.Info(string.Format("{0} * {1} = ", this.Multiplicateur, this.Multiplicande));
        }
    }
}
