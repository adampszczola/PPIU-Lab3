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
using System.Data.SQLite;

namespace PPIU_Lab3.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy User.xaml
    /// </summary>
    public partial class User : Page
    {
        DataBase databaseobj;
        Session session;
     
        List<Events> events = new List<Events>();
       

        public User(Session session)
        {
            InitializeComponent();
            databaseobj = new DataBase();
            this.session = session;
            readEvents();
            
        }

        private void cbName_LostFocus(object sender, RoutedEventArgs e)
        {
            SQLiteCommand query = new SQLiteCommand(databaseobj.Connection);
            databaseobj.Open();
            var temp = cbName.SelectedIndex;
            if (temp >= 0)
            {
                query.CommandText = "SELECT * FROM User_Wydarzenia WHERE Id_User='" + session._id_user + "' AND Id_Wydarzenia='" + events[temp].EventId + "'";
                tbAgenda.Text = events[temp].EventAgenda;
                tbDate.Text = events[temp].EventDate;
                query.ExecuteNonQuery();               
                if(query.ExecuteScalar() != null)
                {
                    SQLiteDataReader reader = query.ExecuteReader();
                    btnJoin.IsEnabled = false;
                    btnJoin.Visibility = Visibility.Hidden;
                    btnLeave.IsEnabled = true;
                    btnLeave.Visibility = Visibility.Visible;
                    reader.Read();
                    cbFood.Text = reader.GetString(3);
                    cbParticipation.Text = reader.GetString(2);
                    cbFood.IsEnabled = false;
                    cbParticipation.IsEnabled = false;

                }
                else
                {
                    btnJoin.IsEnabled = true;
                    btnJoin.Visibility = Visibility.Visible;
                    btnLeave.IsEnabled = false;
                    btnLeave.Visibility = Visibility.Hidden;
                    cbFood.IsEnabled = true;
                    cbParticipation.IsEnabled = true;
                }
            }
            databaseobj.Close();
  
        }

        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            databaseobj.Open();
            string query = "INSERT INTO User_Wydarzenia('Id_User','Id_Wydarzenia','TypUczestnictwa','Wyzywienie') VALUES (@Id_User,@Id_Wydarzenia,@TypUczestnictwa,@Wyzywienie)";
            SQLiteCommand command = new SQLiteCommand(query, databaseobj.Connection);
             command.Parameters.AddWithValue("@Id_User", session._id_user);
             command.Parameters.AddWithValue("@Id_Wydarzenia", (cbName.SelectedIndex+1));
             command.Parameters.AddWithValue("@TypUczestnictwa", cbParticipation.Text);
             command.Parameters.AddWithValue("@Wyzywienie", cbFood.Text);
             command.ExecuteNonQuery();
     
           // MessageBox.Show(session._id_user + " " + (cbName.SelectedIndex + 1) + " " + cbParticipation.Text + " " + cbFood.Text);
            databaseobj.Close();

            btnJoin.IsEnabled = false;
            btnJoin.Visibility = Visibility.Hidden;
            btnLeave.IsEnabled = true;
            btnLeave.Visibility = Visibility.Visible;
            cbFood.IsEnabled = false;
            cbParticipation.IsEnabled = false;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).rootFrame.Navigate(new Pages.Login());
        }

        private void btnLeave_Click(object sender, RoutedEventArgs e)
        {
            databaseobj.Open();
            SQLiteCommand query = new SQLiteCommand(databaseobj.Connection);
            query.CommandText = "DELETE FROM User_Wydarzenia WHERE Id_User='" + session._id_user + "' AND Id_Wydarzenia='" + (cbName.SelectedIndex+1) + "'";
            query.ExecuteNonQuery();
            databaseobj.Close();
            btnJoin.IsEnabled = true;
            btnJoin.Visibility = Visibility.Visible;
            btnLeave.IsEnabled = false;
            btnLeave.Visibility = Visibility.Hidden;
            cbFood.IsEnabled = true;
            cbParticipation.IsEnabled = true;

        }

        void readEvents()
        {
            SQLiteCommand query = new SQLiteCommand(databaseobj.Connection);
            databaseobj.Open();
            query.CommandText = "SELECT * FROM Wydarzenia";
            query.ExecuteNonQuery();
            SQLiteDataReader reader = query.ExecuteReader();
            
            while (reader.Read())
            {
                events.Add(new Events() { EventId = reader.GetInt32(0), EventName = reader.GetString(1), EventAgenda = reader.GetString(2), EventDate = reader.GetString(3) });
            }
            databaseobj.Close();
            for(int i = 0; i < events.Count; i++)
            {
                cbName.Items.Add(events[i].EventName);
            }
        }

      
    }
}
