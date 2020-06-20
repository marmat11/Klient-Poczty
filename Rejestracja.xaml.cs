using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace AplikacjaPoczta
{
    /// <summary>
    /// Logika interakcji dla klasy Rejestracja.xaml
    /// </summary>
    public partial class Rejestracja : Window
    {
        public Rejestracja()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Metoda login sprawdz czy wartość Login_id istnieje w bazie, jeśli nie istnieje dodaje 1 dopóki takiej wartości nie ma, wtedy zwraca tę wartość.
        /// </summary>
        /// <returns>Zwraca int Login_id.</returns>
        public int LoginID()
        {
            int Login_id = 0;
            SqlDataReader sprawdz = null;



            string connectionString = @"Data source=.\SQLExpress;database=BazaPoczta;Trusted_Connection=True";
            SqlConnection connection = new SqlConnection(connectionString);


            do
            {
                Login_id++;

                connection.Close();
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                string commandText = "Select Login_id from Logowanie where Login_id = '" + Login_id + "'";

                command.CommandText = commandText;
                sprawdz = command.ExecuteReader();
                

                

            } while (sprawdz.HasRows == true);



            return Login_id;


        }

        /// <summary>
        /// Metoda przycisku Zarejestruj. Metoda ta dodaje podane informacje w textboxach do bazy danych.Wartość Login_id pobiera z metody LoginID()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BZarejestruj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int Login_id = LoginID();
                string Login = TbLogin.Text;
                string Hasło = TbHasło.Text;
                string Imię = TbImię.Text;
                string Nazwisko = TbNazwisko.Text;

                

                string connectionString = @"Data source=.\SQLExpress;database=BazaPoczta;Trusted_Connection=True";
                SqlConnection connection = new SqlConnection(connectionString);

                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                string commandText1 = "INSERT INTO Logowanie ([Login_id],[Login], [Hasło] ,[Imię] ,[Nazwisko]) VALUES (@Login_id, @Login, @Hasło, @Imię, @Nazwisko)";

                command.CommandText = commandText1;

                SqlParameter param = new SqlParameter("@Login_id", Login_id);
                SqlParameter param1 = new SqlParameter("@Login", Login);
                SqlParameter param2 = new SqlParameter("@Hasło", Hasło);
                SqlParameter param3 = new SqlParameter("@Imię", Imię);
                SqlParameter param4 = new SqlParameter("@Nazwisko", Nazwisko);
                command.Parameters.Add(param);
                command.Parameters.Add(param1);
                command.Parameters.Add(param2);
                command.Parameters.Add(param3);
                command.Parameters.Add(param4);
                int result = command.ExecuteNonQuery();

                connection.Close();

                MessageBox.Show("Rejestracja udana. Cofnij się do okna logowania.");
            }
            catch(SqlException er)
            {
                MessageBox.Show("Error" + LoginID());
                MessageBox.Show(er.Message);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error" + LoginID());
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Metoda przycisku Cofnij. Otwiera poprzednie okno MainWindow i zamyka obecne.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BCofnij_Click(object sender, RoutedEventArgs e)
        {
            MainWindow pokaż = new MainWindow();
            pokaż.Show();
            this.Close();
        }

        /// <summary>
        /// Metoda przycisku Zamknij. Zamyka aplikację.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BZamknij_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
