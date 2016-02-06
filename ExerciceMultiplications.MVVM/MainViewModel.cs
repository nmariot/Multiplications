using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace ExerciceMultiplications.MVVM
{
    class MainViewModel : INotifyPropertyChanged
    {
        private MainModel Model { get; set; }
        private bool _focusReponse = false;        
        private Timer _tmrAvancement;

        /// <summary>
        /// Délai en milliseconds entre 2 "ticks"
        /// </summary>
        private const int DELAI_INTERNE_ATTENTE = 10;

        public MainViewModel()
        {
            this.Model = new MainModel();
        }

        private double _avancement;
        /// <summary>
        /// Avancement du timer
        /// </summary>
        public double Avancement
        {
            get
            {
                return _avancement;
            }
            set
            {
                _avancement = value;
                OnPropertyChanged("Avancement");
            }
        }

        /// <summary>
        /// Une session d'exercices a-t-elle démarrée ?
        /// </summary>
        public bool ExerciceEnCours
        {
            get
            {
                return this.Model.ExerciceEnCours;
            }
        }

        /// <summary>
        /// Max de la table
        /// </summary>
        public int TableMax
        {
            get
            {
                return this.Model.TableMax;
            }
            set
            {
                this.Model.TableMax = value;
                OnPropertyChanged("TableMax");
            }
        }

        /// <summary>
        /// Temps de réflexion
        /// </summary>
        public int DureeReflexion
        {
            get
            {
                return this.Model.DureeReflexion;
            }
            set
            {
                this.Model.DureeReflexion = value;
                OnPropertyChanged("DureeReflexion");
            }
        }

        /// <summary>
        /// Propriété permettant de forcer le focus l'objet réponse
        /// </summary>
        public bool FocusReponse
        {
            get
            {
                return _focusReponse;
            }
            set
            {
                _focusReponse = value;
                OnPropertyChanged("FocusReponse");
            }
        }

        /// <summary>
        /// Multiplicande
        /// </summary>
        public int Multiplicande
        {
            get
            {
                return this.Model.Multiplicande;
            }
        }

        /// <summary>
        /// Multiplicateur
        /// </summary>
        public int Multiplicateur
        {
            get
            {
                return this.Model.Multiplicateur;
            }
        }

        /// <summary>
        /// Réponse fournie par l'utilisateur
        /// </summary>
        public int? ReponseFournie
        {
            get
            {
                return this.Model.ReponseFournie;
            }
            set
            {
                this.Model.ReponseFournie = value;
                OnPropertyChanged("ReponseFournie");
            }
        }

        /// <summary>
        /// Meilleure série de Nombre d'exercices consécutifs réussis
        /// </summary>
        public int MeilleureSerie
        {
            get
            {
                return this.Model.MeilleureSerie;
            }
        }

        /// <summary>
        /// La réponse fournie est-elle correcte ?
        /// </summary>
        public bool ReponseCorrecte
        {
            get
            {
                return this.Model.ReponseCorrecte;
            }
        }

        /// <summary>
        /// La bonne réponse attendue
        /// </summary>
        public int ReponseAttendue
        {
            get
            {
                return this.Model.ReponseAttendue;
            }
        }


        /// <summary>
        /// Nombre de multiplications effectuées
        /// </summary>
        public int NombreMultiplicationsEffectuees
        {
            get
            {
                return this.Model.NombreMultiplicationsEffectuees;
            }
        }

        /// <summary>
        /// Nombre de multiplications reussies
        /// </summary>
        public int NombreMultiplicationsReussies
        {
            get
            {
                return this.Model.NombreMultiplicationsReussies;
            }
        }

        /// <summary>
        /// Série en cours
        /// </summary>
        public int SerieEnCours
        {
            get
            {
                return this.Model.SerieEnCours;
            }
        }

        public ICommand _arreterCommand;
        /// <summary>
        /// Commande démarrer
        /// </summary>
        public ICommand ArreterCommand
        {
            get
            {
                if (_arreterCommand == null)
                {
                    _arreterCommand = new RelayCommand(param => this.Arreter(),
                        param => this.ExerciceEnCours);
                }
                return _arreterCommand;
            }
        }

        public ICommand _demarrerCommand;
        /// <summary>
        /// Commande démarrer
        /// </summary>
        public ICommand DemarrerCommand
        {
            get
            {
                if (_demarrerCommand == null)
                {
                    _demarrerCommand = new RelayCommand(param => this.Demarrer(),
                        param => !this.ExerciceEnCours);
                }
                return _demarrerCommand;
            }
        }

        public ICommand _validerCommand;
        /// <summary>
        /// Commande Valider
        /// </summary>
        public ICommand ValiderCommand
        {
            get
            {
                if (_validerCommand == null)
                {
                    _validerCommand = new RelayCommand(param => this.Valider(),
                        param => this.ExerciceEnCours);
                }
                return _validerCommand;
            }
        }

        private void OnPropertyChanged(string property)
        {
            this.VerifyPropertyName(property);

            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Demarrer()
        {
            this.Model.Demarrer();
            this.OnPropertyChanged("ExerciceEnCours");
            this.OnPropertyChanged("Multiplicateur");
            this.OnPropertyChanged("Multiplicande");
            this.OnPropertyChanged("SerieEnCours");
            this.FocusReponse = true;

            this.DemarrerAttente();
        }

        private void Arreter()
        {
            ArreterAttente();

            this.Model.Arreter();
            this.OnPropertyChanged("ExerciceEnCours");
            this.OnPropertyChanged("Multiplicateur");
            this.OnPropertyChanged("Multiplicande");
            this.OnPropertyChanged("NombreMultiplicationsReussies");
            this.OnPropertyChanged("NombreMultiplicationsEffectuees");
            this.OnPropertyChanged("MeilleureSerie");
            this.OnPropertyChanged("ReponseCorrecte");
            this.OnPropertyChanged("SerieEnCours");
            this.OnPropertyChanged("ReponseAttendue");
            this.FocusReponse = false;
        }

        private void Valider()
        {
            ArreterAttente();

            this.Model.Valider();
            this.OnPropertyChanged("Multiplicateur");
            this.OnPropertyChanged("Multiplicande");
            this.OnPropertyChanged("NombreMultiplicationsReussies");
            this.OnPropertyChanged("NombreMultiplicationsEffectuees");
            this.OnPropertyChanged("MeilleureSerie");
            this.OnPropertyChanged("ReponseCorrecte");
            this.OnPropertyChanged("SerieEnCours");
            this.OnPropertyChanged("ReponseAttendue");
            this.ReponseFournie = null;

            this.DemarrerAttente();
        }

        private void MAJAttente(object o)
        {
            DateTime dateDebut = (DateTime)o;
            DateTime dateFin = dateDebut + TimeSpan.FromSeconds(this.DureeReflexion);
            this.Avancement = 100 - (dateFin - DateTime.Now).TotalSeconds * 100 / this.DureeReflexion;
            if (this.Avancement >= 100)
            {
                // Si délai d'attente terminé, on valide la réponse quelque qu'elle soit
                this.Valider();
            }
        }

        private void DemarrerAttente()
        {
            _tmrAvancement = new Timer(new TimerCallback(MAJAttente), DateTime.Now, 0, DELAI_INTERNE_ATTENTE);
        }

        private void ArreterAttente()
        {
            _tmrAvancement.Dispose();
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                throw new Exception(msg);
            }
        }

    }
}