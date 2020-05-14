using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Data.SQLite;
using System.Timers;
using System.Net.Mail;
using System.Net;
using System.Linq;

namespace PPIU_Lab3.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        bool samePassword = false;
        bool correctEmail = false;
        bool correctLogin = false;
        DataBase databaseobj = new DataBase();

        DateTime dateTime = DateTime.Now;
        public Register()
        {
            InitializeComponent();
        }

        private void tbBack_Click(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).rootFrame.Navigate(new Pages.Login());
        }

        private void tbRegister_Click(object sender, RoutedEventArgs e)
        {
            //https://stackoverflow.com/questions/29684210/most-efficient-way-to-see-if-any-of-textboxes-are-empty-c-sharp
            var tbCollection = new[] { tbName, tbSurname, tbEmail, tbJob, tbPassword, tbPasswordRepeat, tbLogin };
            bool isEmpty = !tbCollection.Any(t => String.IsNullOrWhiteSpace(t.Text));

            if (isEmpty && correctEmail && correctLogin && samePassword)
            {

                sendtoDataBase();
                sendEmail(tbEmail.Text, tbLogin.Text, tbPassword.Text);
                lblEmpty.Content = "Pomyślnie zarejestrowano.";
            }
            else
            {
                lblEmpty.Content = "Sprawdź wszyskie pola.";
                lblEmpty.Foreground = Brushes.Red;
            }
        }

        void sendtoDataBase()
        {
            string query = "INSERT INTO User('Imie','Nazwisko','Stanowisko','Plec','Email','Login','Haslo','Aktywne','DataRejestracji','Uprawnienia') VALUES (@Imie,@Nazwisko,@Stanowisko,@Plec,@Email,@Login,@Haslo,@Aktywne,@DataRejestracji,@Uprawnienia)";
            SQLiteCommand command = new SQLiteCommand(query, databaseobj.Connection);
            databaseobj.Open();
            command.Parameters.AddWithValue("@Imie", tbName.Text);
            command.Parameters.AddWithValue("@Nazwisko", tbSurname.Text);
            command.Parameters.AddWithValue("@Stanowisko", tbJob.Text);
            command.Parameters.AddWithValue("@Plec", cbSex.SelectedValue);
            command.Parameters.AddWithValue("@Email", tbEmail.Text);
            command.Parameters.AddWithValue("@Login", tbLogin.Text);
            command.Parameters.AddWithValue("@Haslo", tbPassword.Text);
            command.Parameters.AddWithValue("@Aktywne", "true");
            command.Parameters.AddWithValue("@DataRejestracji", dateTime.ToString("dd/MM/yyyy"));
            command.Parameters.AddWithValue("@Uprawnienia", "user");
            command.ExecuteNonQuery();
            databaseobj.Close();

        }
        public static void sendEmail(string mail, string login,string password)
        {
            string to = mail;
            string from = "pszczola.ppiu@gmail.com";
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Potwierdzenie rejestracji";
            message.Body = "Weryfikacja rejestracji w aplikacji PPIU-Lab3 Login: "+login+" Hasło: "+password;

            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential("pszczola.ppiu@gmail.com", "ppiu-lab3");
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.Send(message);


        }

        private void tbEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            databaseobj.Open();
            if(tbEmail.Text == "")
            {
                tbEmail.BorderBrush = Brushes.Red;
                correctEmail = false;
            }
            Regex rgx = new Regex(@"\w*@{1}\w+\.{1}\w+");
            bool match = rgx.IsMatch(tbEmail.Text);
            if (match)
            {
                SQLiteCommand query = new SQLiteCommand(databaseobj.Connection);
                query.CommandText = "SELECT Email FROM User Where Email='" + tbEmail.Text + "'";
                query.ExecuteNonQuery();
                if (query.ExecuteScalar() != null)
                {                 
                    tbEmail.BorderBrush = Brushes.Red;
                    lblEmail.Content = "Taki email już istnieje.";
                    correctEmail = false;
                }
                else
                {
                    tbEmail.BorderBrush = Brushes.Green;
                    lblEmail.Content = "";
                    correctEmail = true;
                }
          
            }
            else
            {
                tbEmail.BorderBrush = Brushes.Red;
                lblEmail.Content = "Podaj poprawny email";
                correctEmail = false;
            }
            databaseobj.Close();
        }

        private void tbLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            databaseobj.Open();
            if (tbLogin.Text == "")
            {
                tbLogin.BorderBrush = Brushes.Red;
                correctLogin = false;
            }
            else
            {
                SQLiteCommand query = new SQLiteCommand(databaseobj.Connection);
                query.CommandText = "SELECT Login FROM User Where Login='" + tbLogin.Text + "'";
                query.ExecuteNonQuery();
                if (query.ExecuteScalar() != null)
                {                   
                    tbLogin.BorderBrush = Brushes.Red;
                    lblLogin.Content = "Podany login już istnieje";
                    correctLogin = false;
                }
                else
                {
                    tbLogin.BorderBrush = Brushes.Green;
                    lblLogin.Content = "";
                    correctLogin = true;
                }
            }
            databaseobj.Close();
        }

        private void tbPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if(tbPassword.Text == "")
            {
                tbPassword.BorderBrush = Brushes.Red;
            }
            else
            {
                tbPassword.BorderBrush = Brushes.Green;
            }
            
        }

        private void tbPasswordRepeat_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbPasswordRepeat.Text == "" || tbPassword.Text != tbPasswordRepeat.Text)
            {
                tbPasswordRepeat.BorderBrush = Brushes.Red;
                lblPasswordRepeat.Content = "Oba hasła muszą być identyczne";
                samePassword = false;
            }
            else
            {
                tbPasswordRepeat.BorderBrush = Brushes.Green;
                lblPasswordRepeat.Content = "";
                samePassword = true;
            }
        }

        private void tbJob_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbJob.Text == "")
            {
                tbJob.BorderBrush = Brushes.Red;
            }
            else
            {
                tbJob.BorderBrush = Brushes.Green;
            }
        }

        private void tbSurname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbSurname.Text == "")
            {
                tbSurname.BorderBrush = Brushes.Red;
            }
            else
            {
                tbSurname.BorderBrush = Brushes.Green;
            }
        }

        private void tbName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbName.Text == "")
            {
                tbName.BorderBrush = Brushes.Red;
            }
            else
            {
                tbName.BorderBrush = Brushes.Green;
            }
        }



    }
}
