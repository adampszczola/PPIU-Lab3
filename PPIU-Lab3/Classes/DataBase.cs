using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;

namespace PPIU_Lab3
{
    class DataBase
    {
        public SQLiteConnection Connection;
        public DataBase()
        {
            Connection = new SQLiteConnection("Data Source = database.sqlite3");
            if (!File.Exists("./database.sqlite3"))
            {
                SQLiteConnection.CreateFile("database.sqlite3");
            }
        }
        public void Open()
        {
            if(Connection.State != System.Data.ConnectionState.Open)
            {
                Connection.Open();
            }
        }
        public void Close()
        {
            if(Connection.State != System.Data.ConnectionState.Closed)
            {
                Connection.Close();
            }
        }
    }
}
