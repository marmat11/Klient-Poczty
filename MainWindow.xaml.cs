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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace AplikacjaPoczta
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int liczLogowania = 0;//zmienna do liczenialogowania[przycisk BLogowanie]
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Metoda pobiera z bazy aktualną wartość dla zalogowanego użytkownika Login_id
        /// </summary>
        /// <returns>Zwraca int przekażlogin.Jest to aktualna wartość Login_id +1 dla zalogowanego użytkownika.</returns>
        public int Przekaż()
        {
            int przekażlogin = 0;
            string Login = TbLogin.Text;
            string Hasło = PbHasło.Password;

            SqlDataReader sprawdz = null;


            string connectionString = @"Data source=.\SQLExpress;database=BazaPoczta;Trusted_Connection=True";
            SqlConnection connection = new SqlConnection(connectionString);

            do
            {
                przekażlogin++;
                connection.Close();
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                string commandText = "Select Login_id From Logowanie Where Login ='" + Login + "' And Hasło ='" + Hasło + "' And Login_id = '" + przekażlogin + "'";

                command.CommandText = commandText;
                sprawdz = command.ExecuteReader();

            } while (sprawdz.HasRows == true);

            return przekażlogin;
        }

        /// <summary>
        /// Metoda przycisku Rejestracja.Wywołuje okno Rejestracji i zamyka obecne.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BRejestracja_Click(object sender, RoutedEventArgs e)
        {
            Rejestracja pokaż = new Rejestracja();
            pokaż.Show();
            this.Close();

        }

        /// <summary>
        /// Metoda przycisku Zaloguj.Sprawdza wartości wpisane w textboxie i passwordboxie z danymi w bazie danych jeśli są poprawne otwiera nowe okno WybórPoczty 
        /// A przy 3 błędnych logowaniach blokuje textboxa i possword boxa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BLogin_Click(object sender, RoutedEventArgs e)
        {
            
            string Login = TbLogin.Text;
            string Hasło = PbHasło.Password;

            SqlDataReader sprawdz = null;
           

            string connectionString = @"Data source=.\SQLExpress;database=BazaPoczta;Trusted_Connection=True";
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;

            

            string commandText = "Select * From Logowanie Where Login ='" +Login+ "' And Hasło ='" +Hasło+ "'";

            command.CommandText = commandText;
            sprawdz = command.ExecuteReader();

            if (sprawdz.HasRows == true)
            {
                //MessageBox.Show("Logowanie udane");

                WybórPoczty pokaż = new WybórPoczty();
                pokaż.Show();
                this.Close();
            }
            else
            {
                int próba = 3;
                próba = próba - liczLogowania;
                liczLogowania++;
                MessageBox.Show("Logowanie nieudane.Zostało prób = " +próba);
            }

            connection.Close();

            if (liczLogowania >= 3)
            {
                TbLogin.Clear();
                TbLogin.IsReadOnly = true;
                TbLogin.IsEnabled = false;
                PbHasło.Clear();
                PbHasło.IsEnabled = false;
            }

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
