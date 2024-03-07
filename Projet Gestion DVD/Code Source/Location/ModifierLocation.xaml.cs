using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LocationDVD.Location
{
    /// <summary>
    /// Logique d'interaction pour ModifierLocation.xaml
    /// </summary>
    public partial class ModifierLocation : Window
    {
        readonly LocationController lController;
        private readonly string TitreDVDAvantMaj;
        private readonly int locationId;

        public ModifierLocation(Locations loue, LocationController l, int LocationId)
        {
            lController = l;
            InitializeComponent();
            ListeDeroulanteClients();

            this.locationId = LocationId;
            this.TitreDVDAvantMaj = loue.Title;

            dateEmprunt.Text = loue.dateRented.ToString();
            dateRetour.Text = loue.dateReturned.ToString();

            nbClient.SelectedItem = $"{loue.Nom} {loue.Prenom}";

            // pour obtenir la liste de titres
            List<string> titresDVD = lController.GetTitlesForLocation(locationId);

            // Assigner la liste de titres à l'ItemsSource du ComboBox
            nbDVD.ItemsSource = titresDVD;

            // Sélectionner le premier élément de la liste (si disponible)
            if (titresDVD.Count > 0)
            {
                nbDVD.SelectedItem = titresDVD[0];
            }
        }

        private void ListeDeroulanteClients()
        {
            if (lController != null)
            {
                nbClient.ItemsSource = lController.GetClientNames();
            }
            else
            {
                MessageBox.Show("Erreur : lController est null.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void Modifier3_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(nbClient.Text) || string.IsNullOrEmpty(nbDVD.Text))
            {
                MessageBox.Show("Veuillez selectionner un objet.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
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
                // If not null, essaye de convertir en DateTime
                if (!DateTime.TryParse(dateRetour.Text, out DateTime retourDate))
                {
                    MessageBox.Show("Format de date incorrect pour la date de retour.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                dateRetourValue = retourDate;
            }

            try
            {

                if (lController.ModifLocation(locationId, nbClient.SelectedItem.ToString(), nbDVD.Text, dateRetourValue))
                {
                    // Mise à jour de la disponibilité des DVDs
                    lController.MajDispoDVD(TitreDVDAvantMaj, 1); // Ancien DVD disponible
                    lController.MajDispoDVD(nbDVD.Text, 0); // Nouveau DVD non disponible


                    MessageBox.Show("Information modifiée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}