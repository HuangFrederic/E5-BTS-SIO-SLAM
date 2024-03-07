using LocationDVD.Retour;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace LocationDVD.Rapport
{
    public class RapportController
    {
        public ObservableCollection<Rapports> AllRapport { get; set; }
        public string UserStatus { get; set; }
        public RapportController()
        {
            AllRapport = GetAllRapport();
        }
        public ObservableCollection<Rapports> GetAllRapport()
        {
            ObservableCollection<Rapports> rapportList = new ObservableCollection<Rapports>();
            string connectionString = "server=localhost;database=dvd;uid=root;pwd=;";
            string query = "SELECT * FROM rapport ORDER BY DateGenerated DESC";

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
                            int RapportId = reader.GetInt32(0);
                            DateTime dateRapport = reader.GetDateTime(1);
                            string content = reader.GetString(2);

                            Rapports rapport = new Rapports
                            {
                                RapportId = RapportId,
                                DateGenerated = dateRapport,
                                Content = content
                            };
                            rapportList.Add(rapport);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                }
            }
            return rapportList;
        }

        public ObservableCollection<Rapports> SearchRapports(string TermRapport)
        {
            ObservableCollection<Rapports> searchResultsRapport = new ObservableCollection<Rapports>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;database=dvd;uid=root;pwd=;"))
                {
                    connection.Open();

                    string query = "SELECT * FROM rapport WHERE DateGenerated LIKE @TermRapport OR Content LIKE @TermRapport";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TermRapport", "%" + TermRapport + "%");

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int RapportId = reader.GetInt32(0);
                                DateTime dateRapport = reader.GetDateTime(1);
                                string content = reader.GetString(2);

                                Rapports rapport = new Rapports
                                {
                                    RapportId = RapportId,
                                    DateGenerated = dateRapport,
                                    Content = content
                                };
                                searchResultsRapport.Add(rapport);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            return searchResultsRapport;
        }

    }
}       
