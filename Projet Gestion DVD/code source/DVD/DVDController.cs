using LocationDVD.Location;
using LocationDVD.Rapport;
using LocationDVD.Retour;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LocationDVD.DVD
{
    public class DVDController
    {
        public ObservableCollection<DVDs> listDVD { get; set; }
        public DVDController()
        {
            listDVD = GetDVDs();
        }
        public ObservableCollection<DVDs> GetDVDs()
        {
            ObservableCollection<DVDs> mesDVD = new ObservableCollection<DVDs>();
            string connectionString = "server=localhost;database=dvd;uid=root;pwd=;";
            string query = "SELECT * FROM dvd";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int DVDId = reader.GetInt32(0);
                            string Title = reader.GetString(1);
                            string Director = reader.GetString(2);
                            string Genre = reader.GetString(3);
                            int ReleaseYear = reader.GetInt32(4);
                            int IsAvailable = reader.GetInt32(5);
                            string Image = reader.GetString(6);

                            // Charger l'image directement à partir du chemin complet
                            // Charger l'image à partir du nom de fichier relatif
                            BitmapImage ImageSource = LoadImageFromPath(Image);


                            DVDs dvd = new DVDs
                            {
                                Title = Title,
                                Director = Director,
                                DVDId = DVDId,
                                Genre = Genre,
                                ReleaseYear = ReleaseYear,
                                IsAvailable = IsAvailable,
                                Image = Image,
                                ImageSource = ImageSource
                            };

                            mesDVD.Add(dvd);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            return mesDVD;
        }

        public bool AjoutDVD(DVDs mesDVD)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;database=dvd;uid=root;pwd=;"))
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM dvd " +
                                        "WHERE Title = @Title AND Director = @Director";

                    using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Title", mesDVD.Title);
                        checkCommand.Parameters.AddWithValue("@Director", mesDVD.Director);
                        checkCommand.Parameters.AddWithValue("@ReleaseYear", mesDVD.ReleaseYear);

                        int DVDExiste = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (DVDExiste > 0)
                        {
                            MessageBox.Show("Le DVD existe déjà dans la liste.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    string imageName = System.IO.Path.GetFileName(mesDVD.Image);

                    string query = "INSERT INTO dvd (Title, Director, Genre, ReleaseYear, IsAvailable, Image) " +
                                   "VALUES (@Title, @Director, @Genre, @ReleaseYear, 1, @Image)";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", mesDVD.Title);
                        command.Parameters.AddWithValue("@Director", mesDVD.Director);
                        command.Parameters.AddWithValue("@Genre", mesDVD.Genre);
                        command.Parameters.AddWithValue("@ReleaseYear", mesDVD.ReleaseYear);
                        command.Parameters.AddWithValue("@Image", imageName);
                        command.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }


        private BitmapImage LoadImageFromPath(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
            {
                return null;
            }

            string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", imageName);

            if (!System.IO.File.Exists(imagePath))
            {
                Console.WriteLine("L'image n'existe pas.");
                return null;
            }

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(imagePath);
            image.EndInit();
            return image;
        }



        public bool DeleteDVD(int DVDId)
        {
            string connectionString = "server=localhost;database=dvd;uid=root;pwd=;";
            string query = $"DELETE FROM dvd WHERE DVDId = '{DVDId}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                    return false;
                }
            }
        }

        public ObservableCollection<DVDs> SearchDVD(string TermDVD)
        {
            ObservableCollection<DVDs> searchResultsDVD = new ObservableCollection<DVDs>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;database=dvd;uid=root;pwd=;"))
                {
                    connection.Open();

                    string query = "SELECT * FROM dvd " +
                        "WHERE Title LIKE @TermDVD OR ReleaseYear LIKE @TermDVD OR Director LIKE @TermDVD";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TermDVD", "%" + TermDVD + "%");

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int DVDId = reader.GetInt32(0);
                                string Title = reader.GetString(1);
                                string Director = reader.GetString(2);
                                string Genre = reader.GetString(3);
                                int ReleaseYear = reader.GetInt32(4);
                                int IsAvailable = reader.GetInt32(5);
                                string Image = reader.GetString(6);

                                BitmapImage ImageSource = LoadImageFromPath(Image);

                                DVDs dvd = new DVDs
                                {
                                    Title = Title,
                                    Director = Director,
                                    DVDId = DVDId,
                                    Genre = Genre,
                                    ReleaseYear = ReleaseYear,
                                    IsAvailable = IsAvailable,
                                    Image = Image,
                                    ImageSource = ImageSource
                                };
                                searchResultsDVD.Add(dvd);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            return searchResultsDVD;
        }

        public bool ModifDVD(DVDs mesDVD)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;database=dvd;uid=root;pwd=;"))
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM dvd " +
                    "WHERE Title = @Title AND Director = @Director AND DVDId != @DVDId";

                    using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@DVDId", mesDVD.DVDId);
                        checkCommand.Parameters.AddWithValue("@Title", mesDVD.Title);
                        checkCommand.Parameters.AddWithValue("@Director", mesDVD.Director);
                        checkCommand.Parameters.AddWithValue("@ReleaseYear", mesDVD.ReleaseYear);

                        int DVDExiste = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (DVDExiste > 0)
                        {
                            MessageBox.Show("Le DVD existe déjà dans la liste.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    // Extraire le nom du fichier de l'attribut Image
                    string imageName = System.IO.Path.GetFileName(mesDVD.Image);

                    string query = "UPDATE dvd " +
                    "SET Title = @Title, Director = @Director, Genre = @Genre, ReleaseYear = @ReleaseYear, Image = @Image " +
                    "WHERE DVDId = @DVDId";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DVDId", mesDVD.DVDId);
                        command.Parameters.AddWithValue("@Title", mesDVD.Title);
                        command.Parameters.AddWithValue("@Director", mesDVD.Director);
                        command.Parameters.AddWithValue("@Genre", mesDVD.Genre);
                        command.Parameters.AddWithValue("@ReleaseYear", mesDVD.ReleaseYear);
                        command.Parameters.AddWithValue("@Image", imageName); // Utilise seulement le nom du fichier

                        int rowsAffected = command.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
