using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LocationDVD.Location
{
    public class LocationController
    {
            public ObservableCollection<Locations> AllLocation { get; set; }
            public LocationController()
            {
                AllLocation = GetAllLocation();
            }
        public ObservableCollection<Locations> GetAllLocation()
        {
            ObservableCollection<Locations> LocationList = new ObservableCollection<Locations>();
            string connectionString = "server=localhost;database=dvd;uid=root;pwd=;";
            string query = "SELECT location.LocationId, location.LeClient, location.LeDVD, location.dateRented, location.dateReturned, client.Nom as ClientName, client.Prenom as ClientPrenom, dvd.Title as DVDTitle " +
                           "FROM location " +
                           "INNER JOIN client ON location.LeClient = client.ClientId " +
                           "INNER JOIN dvd ON location.LeDVD = dvd.DVDId";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int LocationId = reader.GetInt32(0);
                            int client = reader.GetInt32(1);
                            int DVD = reader.GetInt32(2);
                            DateTime rented = reader.GetDateTime(3);
                            DateTime? returned = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4);
                            string clientName = reader.GetString(5);
                            string clientPrenom = reader.GetString(6);
                            string dvdTitle = reader.GetString(7);

                            Locations loue = new Locations
                            {
                                LocationId = LocationId,
                                LeClient = client,
                                LeDVD = DVD,
                                dateRented = rented,
                                dateReturned = returned,
                                Nom = clientName,
                                Prenom = clientPrenom,
                                Title = dvdTitle
                            };
                            LocationList.Add(loue);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                }
            }
            return LocationList;
        }


        //POUR ADDLOCATION 
        public ObservableCollection<string> GetClientNames()
        {
            ObservableCollection<string> clientNames = new ObservableCollection<string>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;database=dvd;uid=root;pwd=;"))
                {
                    connection.Open();

                    string query = "SELECT CONCAT(Nom, ' ', Prenom) as FullName FROM client";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string fullName = reader["FullName"].ToString();
                            clientNames.Add(fullName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return clientNames;
        }

        //POUR ADDLOCATION
        public ObservableCollection<string> GetAvailableDVDTitles()
        {
            ObservableCollection<string> dvdTitles = new ObservableCollection<string>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;database=dvd;uid=root;pwd=;"))
                {
                    connection.Open();

                    string query = "SELECT Title FROM dvd WHERE IsAvailable = 1";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string title = reader["Title"].ToString();
                            dvdTitles.Add(title);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return dvdTitles;
        }

        public bool AjoutLocation(string clientName, string dvdTitle, DateTime dateRented, DateTime? dateReturned)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;database=dvd;uid=root;pwd=;"))
                {
                    connection.Open();

                    // Récupérer l'ID client
                    string getClientIdQuery = "SELECT ClientId FROM client WHERE CONCAT(Nom, ' ', Prenom) = @ClientName";

                    int clientId;
                    using (MySqlCommand getClientIdCommand = new MySqlCommand(getClientIdQuery, connection))
                    {
                        getClientIdCommand.Parameters.AddWithValue("@ClientName", clientName);
                        clientId = Convert.ToInt32(getClientIdCommand.ExecuteScalar());
                    }

                    // Récupérer l'ID dvd
                    string getDVDIdQuery = "SELECT DVDId FROM dvd WHERE Title = @DVDTitle";

                    int dvdId;
                    using (MySqlCommand getDVDIdCommand = new MySqlCommand(getDVDIdQuery, connection))
                    {
                        getDVDIdCommand.Parameters.AddWithValue("@DVDTitle", dvdTitle);
                        dvdId = Convert.ToInt32(getDVDIdCommand.ExecuteScalar());
                    }

                    // Ajouter la location
                    string insertLocationQuery = "INSERT INTO location (LeClient, LeDVD, dateRented, dateReturned) VALUES (@LeClient, @LeDVD, @DateRented, @DateReturned)";

                    using (MySqlCommand insertLocationCommand = new MySqlCommand(insertLocationQuery, connection))
                    {
                        insertLocationCommand.Parameters.AddWithValue("@LeClient", clientId);
                        insertLocationCommand.Parameters.AddWithValue("@LeDVD", dvdId);
                        insertLocationCommand.Parameters.AddWithValue("@DateRented", dateRented);
                        insertLocationCommand.Parameters.AddWithValue("@DateReturned", dateReturned);

                        insertLocationCommand.ExecuteNonQuery();

                        // Mettre à jour l'état du DVD
                        string updateDVDQuery = "UPDATE dvd SET IsAvailable = 0 WHERE DVDId = @DVDId";

                        using (MySqlCommand updateDVDCommand = new MySqlCommand(updateDVDQuery, connection))
                        {
                            updateDVDCommand.Parameters.AddWithValue("@DVDId", dvdId);
                            updateDVDCommand.ExecuteNonQuery();
                        }

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool DeleteLocation(int locationId)
        {
            string connectionString = "server=localhost;database=dvd;uid=root;pwd=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Vérifier l'existence de la location
                    string checkLocationQuery = $"SELECT COUNT(*) FROM location WHERE LocationId = {locationId}";
                    using (MySqlCommand checkLocationCommand = new MySqlCommand(checkLocationQuery, connection))
                    {
                        int locationCount = Convert.ToInt32(checkLocationCommand.ExecuteScalar());

                        if (locationCount == 0)
                        {
                            Console.WriteLine($"La location avec l'ID {locationId} n'existe pas.");
                            return false;
                        }
                    }

                    // Supprimer la location et mettre à jour le statut IsAvailable du DVD
                    string deleteLocationQuery = @"
                DELETE FROM location WHERE LocationId = @LocationId;
                UPDATE dvd SET IsAvailable = 1 WHERE DVDId = (SELECT LeDVD FROM location WHERE LocationId = @LocationId);
            ";

                    using (MySqlCommand deleteLocationCommand = new MySqlCommand(deleteLocationQuery, connection))
                    {
                        deleteLocationCommand.Parameters.AddWithValue("@LocationId", locationId);

                        using (MySqlTransaction transaction = connection.BeginTransaction())
                        {
                            deleteLocationCommand.Transaction = transaction;

                            try
                            {
                                int rowsAffected = deleteLocationCommand.ExecuteNonQuery();

                                transaction.Commit();
                                return rowsAffected > 0;
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                Console.WriteLine("Erreur : " + ex.Message);

                                System.Windows.MessageBox.Show($"Erreur lors de la suppression : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

                                return false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);

                    System.Windows.MessageBox.Show($"Erreur lors de la suppression : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

                    return false;
                }
            }
        }



        public ObservableCollection<Locations> SearchLocation(string termLocation)
        {
            ObservableCollection<Locations> searchResultsLocation = new ObservableCollection<Locations>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;database=dvd;uid=root;pwd=;"))
                {
                    connection.Open();

                    string query = @"SELECT l.*, c.Nom, c.Prenom, d.Title 
                             FROM location l
                             INNER JOIN client c ON l.LeClient = c.ClientId
                             INNER JOIN dvd d ON l.LeDVD = d.DVDId
                             WHERE c.Nom LIKE @TermLocation OR c.Prenom LIKE @TermLocation 
                                OR l.dateRented LIKE @TermLocation OR l.dateReturned LIKE @TermLocation 
                                OR d.Title LIKE @TermLocation";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TermLocation", "%" + termLocation + "%");

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int locationId = reader.GetInt32(0);
                                string nomClient = reader.GetString(5); // Indice de la colonne Nom dans la table client
                                string prenomClient = reader.GetString(6); // Indice de la colonne Prenom dans la table client
                                string dvdTitle = reader.GetString(7);
                                DateTime emprunt = reader.GetDateTime(3);
                                DateTime? retour = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4);

                                Locations loue = new Locations { LocationId = locationId, Nom = nomClient, Prenom = prenomClient,
                                        Title = dvdTitle, dateRented = emprunt, dateReturned = retour };
                                searchResultsLocation.Add(loue);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            return searchResultsLocation;
        }


        //POUR MODIFIERLOCATION

        public int LeNomComplet(string fullName)//Pour get Nom Prenom a partir du ClientId
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;database=dvd;uid=root;pwd=;"))
                {
                    connection.Open();

                    string[] nameParts = fullName.Split(' ');
                    string query = "SELECT ClientId FROM client WHERE Nom = @Nom AND Prenom = @Prenom";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@Nom", nameParts[0]);
                    cmd.Parameters.AddWithValue("@Prenom", nameParts[1]);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        // Gérer le cas où le client n'est pas trouvé
                        return -1; // ou une autre valeur qui indique que le client n'est pas trouvé
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1; // ou une autre valeur qui indique une erreur
            }
        }

        public int LeTitre(string title)//Pour get DVDId a partir du Titre
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;database=dvd;uid=root;pwd=;"))
                {
                    connection.Open();

                    string query = "SELECT DVDId FROM dvd WHERE Title = @Title";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@Title", title);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        // Gérer le cas où le DVD n'est pas trouvé 
                        return -1; // ou une autre valeur qui indique que le DVD n'est pas trouvé
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1; 
            }
        }

        public void MajDispoDVD(string title, int isAvailable)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;database=dvd;uid=root;pwd=;"))
                {
                    connection.Open();

                    string query = "UPDATE dvd SET IsAvailable = @IsAvailable WHERE Title = @Title";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@IsAvailable", isAvailable);
                    cmd.Parameters.AddWithValue("@Title", title);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool ModifLocation(int locationId, string fullName, string dvdTitle, DateTime? dateReturned)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;database=dvd;uid=root;pwd=;"))
                {
                    connection.Open();

                    // Obtenir l'ID client
                    int clientId = LeNomComplet(fullName);

                    if (clientId == -1)
                    {
                        MessageBox.Show("Le client n'a pas été trouvé.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }

                    // Obtenir l'ID DVD
                    int dvdId = LeTitre(dvdTitle);

                    if (dvdId == -1)
                    {
                        MessageBox.Show("Le DVD n'a pas été trouvé.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }

                    // Mettre à jour la location
                    string updateQuery = "UPDATE location " +
                                        "SET LeClient = @LeClient, LeDVD = @LeDVD, dateReturned = @DateReturned " +
                                        "WHERE LocationId = @LocationId";

                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@LocationId", locationId);
                        command.Parameters.AddWithValue("@LeClient", clientId);
                        command.Parameters.AddWithValue("@LeDVD", dvdId);
                        command.Parameters.AddWithValue("@DateReturned", dateReturned);

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

        //POUR MODIFIERLOCATION, AFFICHER LISTE DVD DISPONIBLE ET DVD ACTUEL DANS COMBOBOX
        public List<string> GetTitlesForLocation(int locationId)
        {
            List<string> titles = new List<string>();

            // Requête pour obtenir le titre du DVD actuel
            string currentTitleQuery = $"SELECT dvd.Title, dvd.DVDId " +
                                       $"FROM location JOIN dvd ON location.LeDVD = dvd.DVDId " +
                                       $"WHERE location.LocationId = {locationId};";

            // Requête pour obtenir les titres des DVD disponibles
            string availableTitlesQuery = $"SELECT Title, DVDId " +
                                          $"FROM dvd " +
                                          $"WHERE IsAvailable = 1;";

            using (MySqlConnection connection = new MySqlConnection("server=localhost;database=dvd;uid=root;pwd=;"))
            {
                try
                {
                    connection.Open();

                    // Obtenir le titre du DVD actuel
                    MySqlCommand currentTitleCommand = new MySqlCommand(currentTitleQuery, connection);
                    MySqlDataReader currentTitleReader = currentTitleCommand.ExecuteReader();

                    if (currentTitleReader.Read())
                    {
                        string currentTitle = currentTitleReader["Title"].ToString();
                        int currentDvdId = Convert.ToInt32(currentTitleReader["DVDId"]);
                        //POUR DEBUG
                        Console.WriteLine($"Le titre pour l'emplacement {locationId} (DVD #{currentDvdId}) est : {currentTitle}");

                        titles.Add(currentTitle);
                    }
                    else
                    {
                        Console.WriteLine($"Aucun titre trouvé pour l'emplacement {locationId}");
                    }
                    currentTitleReader.Close();

                    // Obtenir les titres des DVD disponibles
                    MySqlCommand availableTitlesCommand = new MySqlCommand(availableTitlesQuery, connection);
                    MySqlDataReader availableTitlesReader = availableTitlesCommand.ExecuteReader();

                    while (availableTitlesReader.Read())
                    {
                        string availableTitle = availableTitlesReader["Title"].ToString();
                        int availableDvdId = Convert.ToInt32(availableTitlesReader["DVDId"]);

                        //POUR DEBUG
                        Console.WriteLine($"DVD disponible : {availableTitle} (DVD #{availableDvdId})");

                        titles.Add(availableTitle);
                    }
                    availableTitlesReader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                }
            }
            return titles;
        }
    }
}

