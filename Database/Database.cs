
using currency_tracker.Models.Database;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace currency_tracker.Database
{
    public class Database
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public bool IsOpen { get; internal set; } = false;

        public Database()
        {
            Initialize();
        }

        private void Initialize()
        {
            server = "130.211.215.193";
            database = "currency";
            uid = "root";
            password = "trackcurrency";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        internal bool OpenConnection()
        {
            try
            {
                connection.Open();
                IsOpen = true;
                return true;
            }
            catch (MySqlException ex)
            {
                IsOpen = false;
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                IsOpen = false;
                return true;
            }
            catch (MySqlException ex)
            {
                IsOpen = false;
                return false;
            }
        }

        public void Insert(Currency input)
        {
            string query = "INSERT INTO currency (name, iso, value1, value2) VALUES(" + input.Name + ", " + input.Iso + ", " + input.Value1 + ", " + input.Value2 + ")";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.ExecuteNonQuery();

                this.CloseConnection();
            }
        }

        public void Update(string iso, double value1, double value2)
        {
            string query = "UPDATE currency SET value1='" + value1 + "', value2='" + value2 + "' WHERE iso='" + iso + "'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;

                cmd.ExecuteNonQuery();

                this.CloseConnection();
            }
        }

        public List<Currency> Select()
        {
            try
            {
                string query = "SELECT * FROM currency";

                List<Currency> list = new List<Currency>();

                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        list.Add(new Currency(
                            (string)dataReader["name"],
                            (string)dataReader["iso"],
                            (double)dataReader["value1"],
                            (double)dataReader["value2"]
                        ));
                        //    _==_ _
                        //  _,(",)|_|
                        //   \/. \-|
                        // __( :  )|_
                    }

                    dataReader.Close();

                    this.CloseConnection();
                    return list;
                }
                else
                {
                    return list;
                }
            }
            catch (System.Exception)
            {
                return new List<Currency>();
            }
        }
    }
}