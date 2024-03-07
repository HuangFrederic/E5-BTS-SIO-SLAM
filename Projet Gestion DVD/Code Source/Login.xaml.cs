using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Input;


namespace LocationDVD
{
    public partial class Login
    {
        public Login() 
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void BtnMini_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPassword.Password;

            string connectionString = "server=localhost;database=dvd;uid=root;pwd=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Utilisez une requête paramétrée pour éviter les attaques par injection SQL
                    string query = "SELECT COUNT(*) FROM user WHERE Username = @Username AND Password = @Password";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        int count = Convert.ToInt32(command.ExecuteScalar());

                        if (count > 0)
                        {
                            MainWindow menu = new MainWindow();
                            menu.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Échec de la connexion. Veuillez vérifier vos informations d'identification.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur de connexion à la base de données : " + ex.Message);
                }
            }
        }
    }
}
