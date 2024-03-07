using LocationDVD.DVD;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LocationDVD.Client
{
    /// <summary>
    /// Logique d'interaction pour ModifierClient.xaml
    /// </summary>
    public partial class ModifierClient : Window
    {
        private readonly int ClientId;
        readonly ClientController cController;
        private bool isUpdating = false;//POUR LE TEL

        public ModifierClient(Clients client, ClientController c, int clientId)
        {
            InitializeComponent();

            this.ClientId = clientId;
            txtNom.Text = client.Nom;
            txtPrenom.Text = client.Prenom;
            txtAdresse.Text = client.Adresse;
            txtNum.Text = client.NumTel;

            txtNum.TextChanged += txtNum_TextChanged; // POUR LE TEL
            cController = c;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)//POUR LE TEL
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");

            if (textBox.Text.Length >= 14 && e.Text.Length > 0)
            {
                e.Handled = true; // Ignorer l'entrée si la longueur maximale est atteinte
            }
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


        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNom.Text) || string.IsNullOrEmpty(txtPrenom.Text) || string.IsNullOrEmpty(txtAdresse.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //POUR LE TEL
            if (!IsNumValid(txtNum.Text))
            {
                MessageBox.Show("Le numéro de téléphone n'est pas valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {              
                Clients client = new Clients
                {
                    ClientId = this.ClientId, 
                    Nom = txtNom.Text,
                    Prenom = txtPrenom.Text,
                    Adresse = txtAdresse.Text,
                    NumTel = txtNum.Text
                };

                if (cController.ModifClient(client))
                {
                    MessageBox.Show("Client modifié avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Une erreur s'est produite lors de la modfication du client.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
