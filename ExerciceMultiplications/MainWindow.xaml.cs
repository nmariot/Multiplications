using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;


namespace ExerciceMultiplications
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool _encours = false;
        private Timer _tmrNouvelleMulti;
        private Random _rnd = new Random();
        private int _nbMulti = 0;
        private int _nbMultiReussies = 0;
        private int _m1, _m2;
        private int _nbReussitesConsecutives = 0;
        private int _nbMaxReussitesConsecutives = 0;

        public MainWindow()
        {
            log4net.Config.XmlConfigurator.Configure();
            _log.Info("==== Démarrage de l'exécutable ====");

            InitializeComponent();

            txtMulti.Text = string.Empty;
            txtRes.Text = string.Empty;

        }

        private void btnDemarrer_Click(object sender, RoutedEventArgs e)
        {
            if (_encours)
            {
                // Arreter la partie
                btnDemarrer.Content = "Démarrer";
                _tmrNouvelleMulti.Stop();
                _log.InfoFormat("Fin de la session. {0} / {1} multiplications réussies. Meilleure suite : {2}", _nbMultiReussies, _nbMulti, _nbMaxReussitesConsecutives);                
            }
            else
            {
                // Démarrer la partie
                btnDemarrer.Content = "Arrêter";
                _nbMulti = 0;
                _nbMultiReussies = 0;
                _nbMaxReussitesConsecutives = 0;
                lblResultatCourant.Content = string.Empty;
                AfficherBilan();
                ChoisirUneMultiplication();
                _tmrNouvelleMulti = new Timer(TimeSpan.FromSeconds(sldDuree.Value).TotalMilliseconds);
                _tmrNouvelleMulti.Elapsed += new ElapsedEventHandler(_tmrNouvelleMulti_Elapsed);
                _tmrNouvelleMulti.Start();

                txtRes.Focus();

                _log.Info("Démarrage de la session.");
            }
            _encours = !_encours;
            btnValider.IsEnabled = _encours;

        }

        void _tmrNouvelleMulti_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate
            {
                _nbReussitesConsecutives = 0;
                lblResultatCourant.Content = "Temps écoulé : la réponse était " + (_m1 * _m2).ToString();
                _log.Info("Temps écoulé : " + txtRes.Text);
                lblResultatCourant.Foreground = Brushes.Red;
                AfficherBilan();
                ChoisirUneMultiplication();
                _tmrNouvelleMulti.Interval = TimeSpan.FromSeconds(sldDuree.Value).TotalMilliseconds;
                _tmrNouvelleMulti.Start();
            });

        }

        private void btnValider_Click(object sender, RoutedEventArgs e)
        {
            ValiderResultat();
        }

        private void ValiderResultat()
        {
            int res;

            if (int.TryParse(txtRes.Text, out res) && _m1 * _m2 == res)
            {
                _nbReussitesConsecutives++;
                _nbMaxReussitesConsecutives = Math.Max(_nbMaxReussitesConsecutives, _nbReussitesConsecutives);
                lblResultatCourant.Content = string.Format("Résultat correct : {0} bonnes réponses consécutives", _nbReussitesConsecutives);
                _log.InfoFormat(lblResultatCourant.Content.ToString());
                lblResultatCourant.Foreground = Brushes.Green;

                _nbMultiReussies++;

            }
            else
            {
                _nbReussitesConsecutives = 0;
                _log.Info("Résultat incorrect : " + txtRes.Text);
                lblResultatCourant.Content = "Résultat incorrect : la réponse était " + (_m1 * _m2).ToString();
                lblResultatCourant.Foreground = Brushes.Red;
            }
            AfficherBilan();
            ChoisirUneMultiplication();
            _tmrNouvelleMulti.Interval = TimeSpan.FromSeconds(sldDuree.Value).TotalMilliseconds;
            _tmrNouvelleMulti.Start();
        }

        private void AfficherBilan()
        {
            lblBilan.Content = string.Format("Session en cours : {0} / {1} multiplications réussies. Meilleure suite : {2}", _nbMultiReussies, _nbMulti, _nbMaxReussitesConsecutives);
        }

        private void ChoisirUneMultiplication()
        {
            _nbMulti++;
            int maxTable = (int)sldTables.Value;
            _m1 = _rnd.Next(9) + 1;
            int nbMin = 1;
            if (_nbReussitesConsecutives >= 10)
            {
                nbMin = maxTable * 3 / 4 + 1;
            }
            else if (_nbReussitesConsecutives >= 5)
            {
                nbMin = maxTable * 2 / 4 + 1;
            }
            else if (_nbReussitesConsecutives >= 2)
            {
                nbMin = maxTable / 4 + 1;
            }
            _log.DebugFormat("Nb min = {0}", nbMin);
            _m2 = _rnd.Next(maxTable - nbMin + 1) + nbMin;

            txtMulti.Text = string.Format("{0} * {1} = ", _m1, _m2);
            txtRes.Text = string.Empty;
            txtRes.Focus();
            _log.Info(txtMulti.Text);
        }

        private void txtRes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && btnValider.IsEnabled)
            {
                ValiderResultat();
            }
        }
    }
}
