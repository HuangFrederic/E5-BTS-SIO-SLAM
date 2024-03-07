using System.Collections.ObjectModel;
using System.Windows;
using System;
using System.Runtime;
using LocationDVD.DVD;
using System.Windows.Controls;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using LocationDVD.Location;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;
using Xceed.Wpf.Toolkit;
using LocationDVD.Retour;
using System.Windows.Input;
using LocationDVD.User;
using System.Linq;
using LocationDVD.Rapport;

namespace LocationDVD
{
    public partial class MainWindow : Window
    {
        readonly ClientController cController;
        readonly DVDController dController;
        readonly LocationController lController;
        readonly RetourController rController;
        readonly UserController uController;
        readonly RapportController rpController;
        public string UserStatus { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            cController = new ClientController();
            DataContext = cController;
            LoadClient();

            dController = new DVDController();
            DataContext = dController;
            LoadDVD();

            lController = new LocationController();
            DataContext = lController;
            LoadLocation();

            rController = new RetourController();
            DataContext = rController;
            LoadRetour();

            rpController = new RapportController();
            DataContext = rpController;
            LoadRapport();

            uController = new UserController();
            var firstUser = uController.GetUsers.FirstOrDefault();

            if (firstUser != null)
            {
                UserStatus = $"Connecté en tant :\n {(firstUser.IsAdmin == 1 ? "Administrateur" : "Employé")}";
                DataContext = this;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void BtnMini_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TabControl_Load(object sender, SelectionChangedEventArgs e)
        {
            TabItem selectedTab = (TabItem)tab.SelectedItem;

            if (selectedTab != null)
            {
                switch (selectedTab.Header.ToString())
                {
                    case "DVDs":
                        LoadDVD();
                        break;
                    case "Locations":
                        LoadLocation();
                        break;
                    case "Retour":
                        LoadRetour();
                        break;
                    case "Rapport":
                        LoadRapport();
                        break;
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Client.AddClient add = new Client.AddClient(cController);
            add.ShowDialog();
            LoadClient();
        }

        private void BtnModif_Click(object sender, EventArgs e)
        {
            Button ModifClient = (Button)sender;
            Clients c = (Clients)ModifClient.DataContext;
            Client.ModifierClient modif = new Client.ModifierClient(c, cController, c.ClientId);
            modif.ShowDialog();
            LoadClient();
        }


        private void LoadClient()
        {
            dataGridClient.ItemsSource = cController.GetAllClients();
            //Maj auto de la liste des clients
        }

        private void SuppClient_Click(object sender, RoutedEventArgs e)
        {
            Button SuppClient = (Button)sender;

            Clients c = (Clients)SuppClient.DataContext;

            MessageBoxResult result = System.Windows.MessageBox.Show($"Êtes-vous sûr de vouloir supprimer le client {c.Nom} {c.Prenom} ?", "Confirmation de suppression", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                bool success = cController.DeleteClient(c.ClientId);

                if (success)
                {
                    LoadClient();
                }
                else
                {
                    System.Windows.MessageBox.Show("Erreur lors de la suppression du client.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RechercherClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string TermClient = txtSearchClient.Text;
                ObservableCollection<Clients> searchResultsClient = cController.SearchClients(TermClient);

                // Maj avec la recherche
                dataGridClient.ItemsSource = searchResultsClient;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
                                                        
                                                            //DVD//
        
        private void BtnAdd2_Click(object sender, EventArgs e)
        {
            AddDVD add = new AddDVD(dController);
            add.ShowDialog();
            LoadDVD();
        }

        private void LoadDVD()
        {
            itemsControlDVD.ItemsSource = dController.GetDVDs();
            //Maj auto de la liste des DVD
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Récupérer l'objet DVD associé au Border cliqué
            Border clickedBorder = sender as Border;
            if (clickedBorder != null)
            {
                // Vérifier si le DVD est sélectionné
                if (clickedBorder.DataContext is DVDs selectedDVD)
                {
                    // Appeler la fonction BtnModif2_Click
                    BtnModif2_Click(selectedDVD, EventArgs.Empty);
                }
            }
        }

        private void BtnModif2_Click(object sender, EventArgs e)
        {
            DVDs selectedDVD = sender as DVDs;

            if (selectedDVD != null)
            {
                ModifierDVD modif = new ModifierDVD(selectedDVD, dController, selectedDVD.DVDId);
                modif.ShowDialog();
                LoadDVD();
            }
        }

        private void SuppDVD_Click(object sender, RoutedEventArgs e)
        {
            Button SuppDVD = (Button)sender;

            DVDs d = (DVDs)SuppDVD.DataContext;

            MessageBoxResult result = System.Windows.MessageBox.Show($"Êtes-vous sûr de vouloir supprimer le DVD '{d.Title}' ?", "Confirmation de suppression", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                bool success = dController.DeleteDVD(d.DVDId);

                if (success)
                {
                    LoadDVD();
                }
                else
                {
                    System.Windows.MessageBox.Show("Erreur lors de la suppression du DVD.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RechercherDVD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string TermDVD = txtSearchDVD.Text;

                ObservableCollection<DVDs> searchResultsDVD = dController.SearchDVD(TermDVD);
                itemsControlDVD.ItemsSource = searchResultsDVD;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

                                                        //LOCATION//

        private void BtnAdd3_Click(object sender, EventArgs e)
        {
            Location.AddLocation add = new Location.AddLocation(lController, cController);
            add.ShowDialog();
            LoadLocation();
        }

        private void LoadLocation()
        {
            dataGridLocation.ItemsSource = lController.GetAllLocation();
            //Maj auto de la liste des locations
        }

        private void BtnModif3_Click(object sender, EventArgs e)
        {
            Button ModifLocation = (Button)sender;
            Locations l = (Locations)ModifLocation.DataContext;

            Location.ModifierLocation modif = new Location.ModifierLocation(l, lController, l.LocationId);
            modif.ShowDialog();
            LoadLocation();
        }

        private void SuppLocation_Click(object sender, RoutedEventArgs e)
        {
            Button SuppLocation = (Button)sender;

            Locations l = (Locations)SuppLocation.DataContext;

            MessageBoxResult result = System.Windows.MessageBox.Show($"Êtes-vous sûr de vouloir supprimer la location ?", "Confirmation de suppression", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                bool success = lController.DeleteLocation(l.LocationId);

                if (success)
                {                  
                    LoadLocation();
                }
                else
                {
                    System.Windows.MessageBox.Show("Erreur lors de la suppression.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RechercherLocation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string TermLocation = txtSearchLocation.Text;
                ObservableCollection<Locations> searchResultsLocation = lController.SearchLocation(TermLocation);
                dataGridLocation.ItemsSource = searchResultsLocation;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
                                                        //RETOUR//

        private void LoadRetour()
        {
            dataGridRetour.ItemsSource = rController.GetAllRetour();
        }

        private void RechercherRetour_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string TermRetour = txtSearchRetour.Text;
                ObservableCollection<Retours> searchResultsRetour = rController.SearchRetour(TermRetour);
                dataGridRetour.ItemsSource = searchResultsRetour;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


                                                        //USER//

        private void Logoff_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultat = System.Windows.MessageBox.Show("Êtes-vous sûr de vouloir vous déconnecter ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resultat == MessageBoxResult.Yes)
            {
                System.Windows.MessageBox.Show("Déconnexion réussie");
                
                Login log = new Login();
                log.Show();
                this.Close();   
            }
        }

                                                //RAPPORT//
        private void LoadRapport()
        {
            dataGridRapport.ItemsSource = rpController.GetAllRapport();
        }

        private void RechercherRapport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string TermRapport = txtSearchRapport.Text;
                ObservableCollection<Rapports> searchResultsRapport = rpController.SearchRapports(TermRapport);
                dataGridRapport.ItemsSource = searchResultsRapport;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}