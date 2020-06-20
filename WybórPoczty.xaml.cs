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
using System.Net;
using System.Net.Mail;

namespace AplikacjaPoczta
{
    /// <summary>
    /// Logika interakcji dla klasy WybórPoczty.xaml
    /// </summary>
    public partial class WybórPoczty : Window
    {
        public WybórPoczty()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Zwraca wartość IdPoczta po wcześniejszym sprawdzeniu czy ta wartość nie jest już w bazie danych.
        /// </summary>
        /// <returns>Zwraca int IdPoczyta</returns>
        public int PocztaID()
        {
            int IdPoczta = 0;
            SqlDataReader sprawdz = null;



            string connectionString = @"Data source=.\SQLExpress;database=BazaPoczta;Trusted_Connection=True";
            SqlConnection connection = new SqlConnection(connectionString);


            do
            {
                IdPoczta++;

                connection.Close();
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                string commandText = "Select IdPoczta from Poczta where IdPoczta = '" + IdPoczta + "'";

                command.CommandText = commandText;
                sprawdz = command.ExecuteReader();




            } while (sprawdz.HasRows == true);



            return IdPoczta;


        }
        /// <summary>
        /// Metoda przycisku Dodaj. Dodaje do bazy danych iformację o nowej skrzynce pocztowej.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BDodaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                int IdPoczta = PocztaID();
                string Serwis;
                if (RBOnetDodaj.IsChecked == true)
                {
                    Serwis = "Onet";
                }
                else if (RBInteriaDodaj.IsChecked == true)
                {
                    Serwis = "Interia";
                }
                else
                {
                    Serwis = "Błąd";
                }

                string connectionString = @"Data source=.\SQLExpress;database=BazaPoczta;Trusted_Connection=True";
                SqlConnection connection = new SqlConnection(connectionString);

                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                string commandText = "INSERT INTO Poczta (IdPoczta , [Email], [HasłoEmail] ,Poczta ) VALUES (@IdPoczta,'" + this.TbEmailDodaj.Text + "','" + this.PbHasłoDodaj.Password + "',@Serwis)";

                command.CommandText = commandText;

                SqlParameter param = new SqlParameter("@Serwis", Serwis);
                SqlParameter param1 = new SqlParameter("@IdPoczta", IdPoczta);

                command.Parameters.Add(param);
                command.Parameters.Add(param1);

                int result = command.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Pomyślnie dodano.");
            }
            catch(SqlException er)
            {
                MessageBox.Show("ERROR");
                MessageBox.Show(er.Message);
            }
        }

        /// <summary>
        /// Metoda przycisku Wyślij. Pobiera dane potrzebne do wysłania emaila [adres nadawcy,hasło nadawcy,adres odbiorcy,temat,treść] oraz dobiera odpowiednie 
        /// ustawienia do adresu nadawcy i wysyła wiadomość.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BWyślij_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int port;
                string poczta;
               
                if (RBOnet.IsChecked == true)
                {
                    port = 587;
                    poczta = "smtp.onet.pl";
                }
                else if (RBInteria.IsChecked == true)
                {
                    port = 587;
                    poczta = "poczta.interia.pl";
                }
                else
                {
                    string błąd = "Nie zaznaczyłeś poczty.";
                    MessageBox.Show(błąd);
                    return;
                }

                var message = new MailMessage();
                message.From = new MailAddress(TbEmail.Text);
                message.To.Add(new MailAddress(TbEmailOdbiorcy.Text));
                message.Subject = TbTemat.Text;
                message.Body = TbWiadomość.Text;

                SmtpClient smtp = new SmtpClient(poczta, port);

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(TbEmail.Text, PbHasło.Password);
                smtp.EnableSsl = false;

                smtp.Send(message);

                MessageBox.Show("Wiadomość została wysłana pomyślnie");
                    
            }
            catch(Exception en)
            {
                MessageBox.Show(en.Message);
            }
        }

        /// <summary>
        /// Metoda przycisku Wyloguj. Otwiera poprzednie okno MainWindow i zamyka obecne.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BWyloguj_Click(object sender, RoutedEventArgs e)
        {
            MainWindow cofnij = new MainWindow();
            cofnij.Show();
            this.Close();
        }
    }
}
