using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace LocationDVD.Location
{
    /// <summary>
    /// Logique d'interaction pour AddLocation.xaml
    /// </summary>
    public partial class AddLocation : Window
    {
        readonly ClientController cController;
        readonly LocationController lController;

        public AddLocation(LocationController l, ClientController c)
        {
            InitializeComponent();
            lController = l;
            cController = c;

            ListeDeroulanteClients();
            ListeDeroulanteDVD();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Client.AddClient add = new Client.AddClient(cController);
            add.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Set Emprunt en CurrentDate
            dateEmprunt.SelectedDate = DateTime.Now;
        }

        private void ListeDeroulanteClients()
        {
            nbClient.ItemsSource = lController.GetClientNames();
        }

        private void ListeDeroulanteDVD()
        {
            nbDVD.ItemsSource = lController.GetAvailableDVDTitles();
        }

        private void DatePicker_Min(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePicker = sender as DatePicker;

            if (datePicker.SelectedDate.HasValue)
            {
                DateTime selectedDate = datePicker.SelectedDate.Value;

                if (datePicker == dateEmprunt)
                {
                    dateRetour.DisplayDateStart = selectedDate;
                    if (dateRetour.SelectedDate.HasValue && dateRetour.SelectedDate < selectedDate)
                    {
                        dateRetour.SelectedDate = selectedDate;
                    }
                }
            }
        }

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(nbClient.Text) || string.IsNullOrEmpty(nbDVD.Text))
            {
                MessageBox.Show("Veuillez sélectionner un client et un DVD.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validation du format de date
            if (!DateTime.TryParse(dateEmprunt.Text, out DateTime dateEmpruntValue))
            {
                MessageBox.Show("Format de date incorrect pour la date d'emprunt.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime? dateRetourValue = null;

            if (!string.IsNullOrEmpty(dateRetour.Text))
            {
                // Si non nul, essaye de convertir en DateTime
                if (!DateTime.TryParse(dateRetour.Text, out DateTime retourDate))
                {
                    MessageBox.Show("Format de date incorrect pour la date de retour.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                dateRetourValue = retourDate;
            }

            try
            {
                string clientName = nbClient.Text;
                string dvdTitle = nbDVD.Text;

                if (lController.AjoutLocation(clientName, dvdTitle, dateEmpruntValue, dateRetourValue))
                {
                    MessageBox.Show("Location ajoutée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Une erreur s'est produite lors de l'ajout de la location.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
