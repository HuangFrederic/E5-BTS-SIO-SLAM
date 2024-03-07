using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Linq;

namespace LocationDVD.Client //Emplacement du AddClient
{
    /// <summary>
    /// Logique d'interaction pour AddClient.xaml
    /// </summary>
    public partial class AddClient : Window
    {
        readonly ClientController cController;
        private bool isUpdating = false;//POUR LE TEL

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)//POUR LE TEL
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");

            if (textBox.Text.Length >= 14 && e.Text.Length > 0)
            {
                e.Handled = true; // Ignorer l'entrée si la longueur maximale est atteinte
            }
        }

        public AddClient(ClientController c)
        {
            cController = c;
            InitializeComponent();
            txtNum.TextChanged += txtNum_TextChanged;//POUR LE TEL
        }

        private bool IsNumValid(string num)//POUR LE TEL
        {
            // Supprimer les espaces et vérif la longueur
            string cleaned = Regex.Replace(num, @"\s", "");

            // Compte uniquement les chiffres
            int digitCount = cleaned.Count(char.IsDigit);

            return digitCount == 10 && Regex.IsMatch(cleaned, @"^\d{10}$");
        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)//POUR LE TEL
        {
            if (isUpdating)
                return;

            var textBox = sender as TextBox;
            string formattedText = FormatPhoneNumber(textBox.Text);
            isUpdating = true;
            textBox.Text = formattedText;
            isUpdating = false;
            textBox.CaretIndex = formattedText.Length;
        }

        private string FormatPhoneNumber(string num)//POUR LE TEL
        {
            // Supprimer tous les espaces existants et formater avec des espaces tous les deux chiffres
            string cleaned = Regex.Replace(num, @"\s", "");
            return Regex.Replace(cleaned, @"(\d{2})(?=\d)", "$1 ");
        }

        // Dans la classe AddClient
        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNom.Text) || string.IsNullOrEmpty(txtPrenom.Text) || string.IsNullOrEmpty(txtAdresse.Text) || string.IsNullOrEmpty(txtNum.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validation du format du numéro de téléphone
            if (!IsNumValid(txtNum.Text))
            {
                MessageBox.Show("Le numéro de téléphone n'est pas valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Création d'un objet Client avec les données du formulaire
                Clients client = new Clients
                {
                    Nom = txtNom.Text,
                    Prenom = txtPrenom.Text,
                    Adresse = txtAdresse.Text,
                    NumTel = txtNum.Text
                };

                // Appel de la méthode Add de votre ClientController
                if (cController.AjoutClient(client))
                {
                    MessageBox.Show("Client ajouté avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close(); // Fermer la fenêtre après un ajout réussi
                }
                else
                {
                    MessageBox.Show("Une erreur s'est produite lors de l'ajout du client.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
