using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LogicielNettoyage
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DirectoryInfo winTemp;
        public DirectoryInfo appTemp;
        public static long total;
        public MainWindow()
        {
            InitializeComponent();
            winTemp = new DirectoryInfo(@"C:\\Windows\Temp");
            appTemp = new DirectoryInfo(System.IO.Path.GetTempPath());
            CheckActu();
            CheckLastDate();
        }

        /// <summary>
        /// Calcul de la taille des fichiers temporaires
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public long DirSize(DirectoryInfo dir)
        {
            return dir.GetFiles().Sum(fi => fi.Length) + dir.GetDirectories().Sum(di => DirSize(di));
        }

        /// <summary>
        /// Nettoie les fichiers temporaires
        /// </summary>
        /// <param name="di"></param>
        public void ClearTempData(DirectoryInfo di)
        {
            foreach (FileInfo fi in di.GetFiles())
            {
                try
                {

                    fi.Delete();
                    Console.Write(fi.FullName);
                    total++;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    continue;
                }
            }

            foreach(DirectoryInfo dir in di.GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                    Console.WriteLine(dir.FullName);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex);
                    continue;
                }
            }
        }

        /// <summary>
        /// Verifie les informations se trouvant dans le fichier d'actualité
        /// </summary>
        public
            void CheckActu()
        {
            string url = "http://localhost/test.txt";
            using(WebClient client = new WebClient())
            {
                try
                {
                    string actu = client.DownloadString(url);

                    if (actu != "")
                    {
                        news.Content = actu;
                        news.Visibility = Visibility.Visible;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                
            }
        }
        private void my_btn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// Boutton de nettoyage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clean_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Nettoyage en cours ...");
            clean.Content = "";
            clean.Content = "\n\n\nNettoyage...";
            Clipboard.Clear();

            try
            {
                ClearTempData(winTemp);
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            try
            {
                ClearTempData(appTemp);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            clean.Content = "\n\n\nTerminé";
            titre.Content = "Nettoyage terminé";
            space_to_clean.Content = "0 Mb";
        }

        /// <summary>
        /// Boutton de mise a jour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Le logiciel est à jour !","information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void histo_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// Fonction pour la redirection vers le site web.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void web_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("http://eliel243.pythonanywhere.com")
                {
                    UseShellExecute = true
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                
            }
        }
        /// <summary>
        /// Boutton d'analyse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Analyser(object sender, RoutedEventArgs e)
        {
            AnalyseFolder();
        }

        /// <summary>
        /// Fonction d'analyse du fichier
        /// </summary>
        public void AnalyseFolder()
        {
            Console.WriteLine("Debut de l'analyse...");
            long total_size = 0;
            try
            {
                total_size += DirSize(winTemp) / 1000000;
                total_size += DirSize(appTemp) / 1000000;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            space_to_clean.Content = total_size + "Mb";
            last_analyse.Content = DateTime.Today.ToString("dd-M-yyyy");
            titre.Content = "Analyse terminé";
            SaveLastDate();
        }
        /// <summary>
        /// Enregistre la derniere analyse.
        /// </summary>
        public void SaveLastDate()
        {
            string date = DateTime.Today.ToString("dd-M-yyyy");

            File.WriteAllText("LastChecking.txt", date);
        }
        /// <summary>
        /// Affiche la derniere analyse.
        /// </summary>
        public void CheckLastDate()
        {
            try
            {
                string date = File.ReadAllText("LastChecking.txt");
                last_analyse.Content = date;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
