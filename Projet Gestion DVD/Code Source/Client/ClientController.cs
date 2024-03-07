using LocationDVD.Client;
using LocationDVD.Location;
using LocationDVD.Rapport;
using LocationDVD.Retour;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace LocationDVD
{
    public class ClientController
    {
        public ObservableCollection<Clients> AllClient { get; set; }
        public ClientController()
        {
            AllClient = GetAllClients();
        }
        public ObservableCollection<Clients> GetAllClients()
        {
            ObservableCollection<Clients> clientList = new ObservableCollection<Clients>();
            string connectionString = "server=localhost;database=dvd;uid=root;pwd=;";
            string query = "SELECT * FROM client";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    //Lis et execute la requete
                    {
                        while (reader.Read())
                        //prend les infos
                        {
                            int ClientId = reader.GetInt32(0);
                            string nom = reader.GetString(1);
                            string prenom = reader.GetString(2);
                            string adresse = reader.GetString(3);
                            string num = reader.GetString(4);

                            Clients client = new Clients {ClientId = ClientId, Nom = nom, Prenom = prenom, Adresse = adresse, NumTel = num };
                            clientList.Add(client);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                }
            }
            return clientList;
        }

        public bool AjoutClient(Clients client)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;database=dvd;uid=root;pwd=;"))
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM client WHERE NumTel = @NumTel";
                    using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@NumTel", client.NumTel);

                        int numExiste = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (numExiste > 0)
                        {
                            MessageBox.Show("Le numéro de téléphone existe déjà dans la base de données.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                    }

                    string query = "INSERT INTO client (Nom, Prenom, Adresse, NumTel) VALUES (@Nom, @Prenom, @Adresse, @NumTel)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nom", client.Nom);
                        command.Parameters.AddWithValue("@Prenom", client.Prenom);
                        command.Parameters.AddWithValue("@Adresse", client.Adresse);
                        command.Parameters.AddWithValue("@NumTel", client.NumTel);

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

        public bool DeleteClient(int ClientId)
        {
            string connectionString = "server=localhost;database=dvd;uid=root;pwd=;";
            string query = $"DELETE FROM client WHERE ClientId = '{ClientId}'";

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

        public ObservableCollection<Clients> SearchClients(string TermClient)
        {
            ObservableCollection<Clients> searchResultsClient = new ObservableCollection<Clients>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;database=dvd;uid=root;pwd=;"))
                {
                    connection.Open();

                    string query = "SELECT * FROM client WHERE Nom LIKE @TermClient OR Prenom LIKE @TermClient";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TermClient", "%" + TermClient + "%");

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int ClientId = reader.GetInt32(0);
                                string nom = reader.GetString(1);
                                string prenom = reader.GetString(2);
                                string adresse = reader.GetString(3);
                                string num = reader.GetString(4);

                                Clients client = new Clients { ClientId = ClientId, Nom = nom, Prenom = prenom, Adresse = adresse, NumTel = num };
                                searchResultsClient.Add(client);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            return searchResultsClient;
        }

        public bool ModifClient(Clients client)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;database=dvd;uid=root;pwd=;"))
                {
                    connection.Open();

                    //POUR LE TEL
                    string checkQuery = "SELECT COUNT(*) FROM client WHERE NumTel = @NumTel AND ClientId != @ClientId";
                    using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@NumTel", client.NumTel);
                        checkCommand.Parameters.AddWithValue("@ClientId", client.ClientId); // Ajout de l'ID du client

                        int numExiste = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (numExiste > 0)
                        {
                            MessageBox.Show("Le numéro de téléphone existe déjà dans la base de données.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                    }

                    string query = "UPDATE client " +
                        "SET Nom = @Nom, Prenom = @Prenom, Adresse = @Adresse, NumTel = @NumTel " +
                        "WHERE ClientId = @ClientId";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ClientId", client.ClientId);
                        command.Parameters.AddWithValue("@Nom", client.Nom);
                        command.Parameters.AddWithValue("@Prenom", client.Prenom);
                        command.Parameters.AddWithValue("@Adresse", client.Adresse);
                        command.Parameters.AddWithValue("@NumTel", client.NumTel);

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