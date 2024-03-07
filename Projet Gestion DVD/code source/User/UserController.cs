using LocationDVD.Retour;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationDVD.User
{
    public class UserController
        {
            public ObservableCollection<Users> GetUsers { get; set; }
            public UserController()
            {
                GetUsers = GetAllUser();
            }
            public ObservableCollection<Users> GetAllUser()
            {
                ObservableCollection<Users> UserList = new ObservableCollection<Users>();
                string connectionString = "server=localhost;database=dvd;uid=root;pwd=;";

                string query = " SELECT * FROM user ";  


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
                                int UserId = reader.GetInt32(0);
                                string user = reader.GetString(1);
                                string mdp = reader.GetString(2);
                                int IsAdmin = reader.GetInt32(3);


                                Users login = new Users
                                {
                                    UserId = UserId,
                                    Username = user,
                                    Password = mdp,
                                    IsAdmin = IsAdmin
                                };
                                UserList.Add(login);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erreur : " + ex.Message);
                    }
                }
                return UserList;
            }
    }
}
