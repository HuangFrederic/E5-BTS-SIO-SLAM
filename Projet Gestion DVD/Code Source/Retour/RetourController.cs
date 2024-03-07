using LocationDVD.Location;
using LocationDVD.Rapport;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LocationDVD.Retour
{
    public class RetourController
    {
        public ObservableCollection<Retours> GetRetours { get; set; }
        public string UserStatus { get; set; }
        public ObservableCollection<Rapports> AllRapport { get; set; }
        public RetourController()
        {
            GetRetours = GetAllRetour();
        }
        public ObservableCollection<Retours> GetAllRetour()
        {
            ObservableCollection<Retours> RetourList = new ObservableCollection<Retours>();
            string connectionString = "server=localhost;database=dvd;uid=root;pwd=;";

            string query = "SELECT retour.RetourId, retour.LaLocation, retour.DateReturned, retour.LocationPrix, retour.Retourner, client.Nom as ClientName, client.Prenom as ClientPrenom, dvd.Title as DVDTitle " +
               "FROM retour " +
               "INNER JOIN location ON retour.LaLocation = location.LocationId " +
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
                            int RetourId = reader.GetInt32(0);
                            int LaLoc = reader.GetInt32(1);
                            DateTime? returned = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2);
                            decimal lePrix = reader.GetDecimal(3); 
                            int rendu = reader.GetInt32(4);
                            string clientName = reader.GetString(5);
                            string clientPrenom = reader.GetString(6);
                            string dvdTitle = reader.GetString(7);

                            Retours retour = new Retours
                            {
                                RetourId = RetourId,
                                LaLocation = LaLoc,
                                DateReturned = returned,
                                LocationPrix = lePrix,
                                Retourner = rendu,
                                Nom = clientName,
                                Prenom = clientPrenom,
                                Title = dvdTitle
                            };
                            RetourList.Add(retour);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                }
            }
            return RetourList;
        }

        public ObservableCollection<Retours> SearchRetour(string termRetour)
        {
            ObservableCollection<Retours> searchResultsRetour = new ObservableCollection<Retours>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;database=dvd;uid=root;pwd=;"))
                {
                    connection.Open();

                    string query = @"SELECT r.*, c.Nom as ClientName, c.Prenom as ClientPrenom, d.Title as DVDTitle 
                FROM retour r
                INNER JOIN location l ON r.LaLocation = l.LocationId
                INNER JOIN client c ON l.LeClient = c.ClientId 
                INNER JOIN dvd d ON l.LeDVD = d.DVDId 
                WHERE c.Nom LIKE @TermRetour OR c.Prenom LIKE @TermRetour
                OR r.DateReturned LIKE @TermRetour OR r.LocationPrix LIKE @TermRetour 
                OR r.Retourner LIKE @TermRetour 
                OR d.Title LIKE @TermRetour";


                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TermRetour", "%" + termRetour + "%");

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int RetourId = reader.GetInt32(0);
                                int LaLoc = reader.GetInt32(1);
                                DateTime? returned = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2);
                                decimal lePrix = reader.GetDecimal(3);
                                int rendu = reader.GetInt32(4);
                                string clientName = reader.GetString(5);
                                string clientPrenom = reader.GetString(6);
                                string dvdTitle = reader.GetString(7);

                                // Ajoutez des instructions de débogage ici
                                Debug.WriteLine($"RetourId: {RetourId}, LaLoc: {LaLoc}, DateReturned: {returned}, LocationPrix: {lePrix}, Retourner: {rendu}, ClientName: {clientName}, ClientPrenom: {clientPrenom}, DVDTitle: {dvdTitle}");

                                Retours rent = new Retours
                                {
                                    RetourId = RetourId,
                                    LaLocation = LaLoc,
                                    DateReturned = returned,
                                    LocationPrix = lePrix,
                                    Retourner = rendu,
                                    Nom = clientName,
                                    Prenom = clientPrenom,
                                    Title = dvdTitle
                                };
                                searchResultsRetour.Add(rent);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erreur : " + ex.ToString());
            }

            return searchResultsRetour;
        }
    }
}
