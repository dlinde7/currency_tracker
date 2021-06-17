
using currency_tracker.Models.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

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

        static HttpClientHandler handler = new HttpClientHandler();
        HttpClient client = new HttpClient(handler);
        public async Task UpdateAsync()
        {

            client.CancelPendingRequests();
            client.BaseAddress = new Uri("https://cdn.jsdelivr.net/gh/fawazahmed0/currency-api@1/latest/currencies/");
            HttpResponseMessage response = await client.GetAsync("zar.json");
            response.EnsureSuccessStatusCode();

            Dictionary<string, string> list = new Dictionary<string, string>();
            string jsonString = await response.Content.ReadAsStringAsync();
            jsonString = jsonString.Split('{')[2].Replace('}', ' ').Replace('\"', ' ');

            foreach (var item in jsonString.Split(','))
            {
                list.Add(item.Split(':')[0].Trim(), item.Split(':')[1].Trim());
            }

            foreach (var item in list)
            {
                string query = "UPDATE currency SET value2=" + item.Value + " WHERE iso='" + item.Key + "'";

                //Open connection
                if (OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    cmd.ExecuteNonQuery();
                }
                this.CloseConnection();
            }

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://cdn.jsdelivr.net/gh/fawazahmed0/currency-api@1/" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "/currencies/");
            response = await client.GetAsync("zar.json");
            response.EnsureSuccessStatusCode();

            list.Clear();
            jsonString = await response.Content.ReadAsStringAsync();
            jsonString = jsonString.Split('{')[2].Replace('}', ' ').Replace('\"', ' ');

            foreach (var item in jsonString.Split(','))
            {
                list.Add(item.Split(':')[0].Trim(), item.Split(':')[1].Trim());
            }

            foreach (var item in list)
            {
                string query = "UPDATE currency SET value1=" + item.Value + " WHERE iso='" + item.Key + "'";

                //Open connection
                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    cmd.ExecuteNonQuery();


                }
                this.CloseConnection();
            }
            return;

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