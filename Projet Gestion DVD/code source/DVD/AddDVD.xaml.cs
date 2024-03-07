using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

namespace LocationDVD.DVD
{
    /// <summary>
    /// Logique d'interaction pour AddDVD.xaml
    /// </summary>
    public partial class AddDVD : Window
    {
        readonly private DVDController dController;
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        public AddDVD(DVDController d)
        {   
            dController = d;
            InitializeComponent();  
        }

        private string selectedImagePath;

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            // open file dialog
            OpenFileDialog open = new OpenFileDialog
            {
                Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png"
            };

            if (open.ShowDialog() == true)
            {
                try
                {
                    string sourceName = open.FileName;
                    string fileName = Path.GetFileName(sourceName);

                    // update label content
                    AffichageNomImage.Content = fileName;

                    string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                    string desitnation = @path + "\\img\\";
                    string destinationPath = System.IO.Path.Combine(desitnation, fileName);

                    // image file path
                    selectedImagePath = destinationPath; // Utilisez le chemin complet
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitre.Text) || string.IsNullOrEmpty(txtDirector.Text)
                || string.IsNullOrEmpty(txtGenre.Text) || string.IsNullOrEmpty(txtSortie.Text)
                || string.IsNullOrEmpty(selectedImagePath))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (!int.TryParse(txtSortie.Text, out int releaseYear))
                {
                    MessageBox.Show("Veuillez entrer une année valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Création d'un objet Client avec les données du formulaire
                DVDs mesDVD = new DVDs
                {
                    Title = txtTitre.Text,
                    Director = txtDirector.Text,
                    Genre = txtGenre.Text,
                    ReleaseYear = releaseYear,
                    Image = selectedImagePath,
                };

                // Appel de la méthode Add de votre ClientController
                if (dController.AjoutDVD(mesDVD))
                {
                    MessageBox.Show("DVD ajouté avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close(); // Fermer la fenêtre après un ajout réussi
                }
                else
                {
                    MessageBox.Show("Une erreur s'est produite lors de l'ajout du DVD.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
