using Microsoft.Win32;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
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
    /// Logique d'interaction pour ModiferDVD.xaml
    /// </summary>
    public partial class ModifierDVD : Window
    {
        private string currentImagePath;
        private readonly int DVDId;  //pour stocker l'ID
        readonly private DVDController dController;

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
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
                    AffichageNomImage.Content = Path.GetFileName(fileName);

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

        public ModifierDVD(DVDs mesDVD, DVDController d, int dvdId)
{
    InitializeComponent();

    this.DVDId = dvdId;  // Stock ID 

    txtTitre.Text = mesDVD.Title;
    txtDirector.Text = mesDVD.Director;
    txtGenre.Text = mesDVD.Genre;
    txtSortie.Text = mesDVD.ReleaseYear.ToString();
    AffichageNomImage.Content = mesDVD.Image.ToString();

    // Stockez le chemin de l'image actuel
    currentImagePath = mesDVD.Image;

    dController = d;
}



        private void Modifier2_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitre.Text) || string.IsNullOrEmpty(txtDirector.Text)
                || string.IsNullOrEmpty(txtGenre.Text) || string.IsNullOrEmpty(txtSortie.Text))
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

                string newImagePath = selectedImagePath;

                // Vérifiez si l'image a été modifiée
                if (string.IsNullOrEmpty(selectedImagePath))
                {
                    // Si l'image n'a pas été modifiée, utilisez le chemin de l'image actuel
                    newImagePath = currentImagePath;
                }

                DVDs mesDVD = new DVDs
                {
                    DVDId = this.DVDId,
                    Title = txtTitre.Text,
                    Director = txtDirector.Text,
                    Genre = txtGenre.Text,
                    ReleaseYear = releaseYear,
                    Image = newImagePath, // Utilisez le nouveau chemin de l'image ou le chemin actuel
                };

                if (dController.ModifDVD(mesDVD))
                {
                    MessageBox.Show("DVD modifié avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Une erreur s'est produite lors de la modification du DVD.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}