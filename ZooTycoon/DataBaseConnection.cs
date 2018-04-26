using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooTycoon
{
    //Simple class that will hold all database technicalities
    class DataBaseConnection
    {
        private MySqlConnection connection;
        private string server;     //The server we will be connection to
        private string database;   //Name of the database
        private string uid;        //Username
        private string password;

        public DataBaseConnection()
        {
            Initialize();
        }

        //Initialize login info for the database
        private void Initialize()
        {
            server = "localhost";
            database = "zoo";
            uid = "root";
            password = "cheese07";
            String connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //Open the connection the database
        private bool Open()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Failed to open database...");
                return false;
            }
        }

        //Close the connection to the database
        private bool Close()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }

        //Searches the database for all animals from a certain location
        public List<String>[] Select(String location)
        {
            string query = "SELECT animal.name AS species, inhabitants.name, inhabitants.location FROM animal, inhabitants WHERE animal.ID = inhabitants.ID AND inhabitants.location = @location";
            List<String>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            if(this.Open() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@location", location);
                MySqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    list[0].Add(dataReader["species"] + "");
                    list[1].Add(dataReader["name"] + "");
                    list[2].Add(dataReader["location"] + "");
                }

                dataReader.Close();

                this.Close();

                return list;
            }
            else
            {
                Console.WriteLine("Opening the database has failed.");
                return list;
            }
        }

        //Add a new inhabitant to a certain location
        public bool Insert(String species, String name, String location)
        {
            int id = 0;
            String query = "SELECT id FROM animal WHERE name = @species";

            if (this.Open() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@species", species);
                MySqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read()) {
                    id = int.Parse(dataReader["id"]+"");
                }
                dataReader.Close();
                if(id == 0)
                {
                    this.Close();
                    return false;
                }
                query = "SELECT * FROM inhabitants WHERE @name = name AND @location = location";
                command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@location", location);
                dataReader = command.ExecuteReader();
                List<String>[] list = new List<string>[3];
                list[0] = new List<string>();
                list[1] = new List<string>();
                list[2] = new List<string>();
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["name"] + "");
                    list[1].Add(dataReader["location"] + "");
                    list[2].Add(dataReader["id"] + "");
                }
                dataReader.Close();
                this.Close();
                if(list[0].Count() !=0)
                {
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Opening the database has failed.");
                return false;
            }

            query = "INSERT INTO inhabitants VALUES (@name, @location, @id)";
            if (this.Open() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@location", location);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                this.Close();

                return true;
            }
            else
            {
                Console.WriteLine("Opening the database has failed.");
                return false;
            }
        }
    }
}
