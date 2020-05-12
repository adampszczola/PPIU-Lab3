﻿using System;
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
using System.Runtime;

namespace PPIU_Lab3.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        DataBase databaseobj;
        bool successLogin = false;
        bool successPassword = false;       
        int loginAttempt = 3;
        DateTime dateTime = new DateTime();

        public Login()
        {
            InitializeComponent();
            databaseobj = new DataBase();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {

        }
        void checkncreate()
        {
            SQLiteCommand query1 = new SQLiteCommand(databaseobj.Connection);
            SQLiteCommand query2 = new SQLiteCommand(databaseobj.Connection);
            databaseobj.Open();
            query1.CommandText = "SELECT * FROM User Where Login='admin' and Haslo='admin'";
            query2.CommandText = "SELECT * FROM User Where Login='user' and Haslo='user'";
            query1.ExecuteNonQuery();
            query2.ExecuteNonQuery();
            var count = query1.ExecuteScalar();

            if(count != null)
            {
                
                string createQuery = "INSERT INTO User (`Imie`, `Nazwisko`, `Stanowisko`, `Plec`, `Email`, `Login`, `Haslo`, `Aktywne`, `DataRejestracji`,`Uprawnienia`) VALUES (@Imie, @Nazwisko, @Stanowisko, @Plec, @Email, @Login, @Haslo, @Aktywne, @DataRejestracji, @Uprawnienia)";
                SQLiteCommand query3 = new SQLiteCommand(createQuery, databaseobj.Connection);
                query3.Parameters.AddWithValue("@Imie", "Admin");
                query3.Parameters.AddWithValue("@Nazwisko", "Admin");
                query3.Parameters.AddWithValue("@Stanowisko", "Admin");
                query3.Parameters.AddWithValue("@Plec", "Admin");
                query3.Parameters.AddWithValue("@Email", "Admin@admin.admin");
                query3.Parameters.AddWithValue("@Login", "admin");
                query3.Parameters.AddWithValue("@Haslo", "admin");
                query3.Parameters.AddWithValue("@Aktywne", "true");
                query3.Parameters.AddWithValue("@DataRejestracji", dateTime.ToString("dd/MM/yyyy"));
                query3.Parameters.AddWithValue("@Uprawnienia", "admin");
                
            }
            count = query2.ExecuteScalar();
            if(count != null)
            {
                string createQuery = "INSERT INTO User (`Imie`, `Nazwisko`, `Stanowisko`, `Plec`, `Email`, `Login`, `Haslo`, `Aktywne`, `DataRejestracji`,`Uprawnienia`) VALUES (@Imie, @Nazwisko, @Stanowisko, @Plec, @Email, @Login, @Haslo, @Aktywne, @DataRejestracji, @Uprawnienia)";
                SQLiteCommand query4 = new SQLiteCommand(createQuery, databaseobj.Connection);
                query4.Parameters.AddWithValue("@Imie", "user");
                query4.Parameters.AddWithValue("@Nazwisko", "user");
                query4.Parameters.AddWithValue("@Stanowisko", "user");
                query4.Parameters.AddWithValue("@Plec", "user");
                query4.Parameters.AddWithValue("@Email", "user@user.user");
                query4.Parameters.AddWithValue("@Login", "user");
                query4.Parameters.AddWithValue("@Haslo", "user");
                query4.Parameters.AddWithValue("@Aktywne", "true");
                query4.Parameters.AddWithValue("@DataRejestracji", dateTime.ToString("dd/MM/yyyy"));
                query4.Parameters.AddWithValue("@Uprawnienia", "user");
            }
            databaseobj.Close();
        }



    }
}