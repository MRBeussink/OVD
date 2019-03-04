using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using test_OVD_clientless.Models;

namespace test_OVD_clientless.Helpers
{
    public class GuacamoleDatabaseConnector
    {
        private const int MAX_USER_CONNECTIONS = 1;

        private MySqlConnection connection;
        private const string SERVER = "172.17.0.3";     //Guacamole SQL Docker Container IP
        private const string PORT = "3306";             //Guacamole SQL Docker Port
        private const string DATABASE = "guacamole_db";
        private const string USER = "root";
        private const string PASSWORD = "secret";


        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:test_OVD_clientless.Guacamole_Connector.GuacamoleDatabaseConnector"/> class.
        /// </summary>
        public GuacamoleDatabaseConnector()
        {
            initialize();
        }


        /// <summary>
        /// Initialize this instance of the Guacamole mysql connection.
        /// </summary>
        private void initialize()
        {
            string connectionString;
            connectionString = "Server=" + SERVER + ";" + "Port=" + PORT + ";" + "Database=" +
            DATABASE + ";" + "UID=" + USER + ";" + "Password=" + PASSWORD + ";";

            connection = new MySqlConnection(connectionString);
        }


        /// <summary>
        /// Opens a connection to the Guacamole mysql database.
        /// </summary>
        /// <returns><c>true</c>, if connection was opened, <c>false</c> otherwise.</returns>
        private bool openConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException e)
            {
                Console.Error.Write(e.Message);
                return false;
            }
        }


        /// <summary>
        /// Closes the connection to the Guacamole mysql database.
        /// </summary>
        /// <returns><c>true</c>, if connection was closed, <c>false</c> otherwise.</returns>
        private bool closeConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException e)
            {
                Console.Error.Write(e.Message);
                return false;
            }
        }


        /// <summary>
        /// Inserts a new group into the Guacamole mysql database
        /// </summary>
        /// <returns><c>true</c>, if group was inserted, <c>false</c> otherwise.</returns>
        /// <param name="newGroup">Group Object.</param>
        public bool insertGroup(Group newGroup)
        {
            try
            {
                openConnection();

                MySqlCommand query = new MySqlCommand(
                    "INSERT INTO guacamole_connection_group (connection_group_name, " + 
                    "max_connections, max_connections_per_user)" +
                    "VALUES (@groupname, @maxconnections, @maxuserconnections)",
                    connection);

                query.Prepare();
                query.Parameters.AddWithValue("@groupname", newGroup.name);
                query.Parameters.AddWithValue("@maxconnections", newGroup.config.maxNum);
                query.Parameters.AddWithValue("@maxuserconnections", MAX_USER_CONNECTIONS);

                int rowsEffected = query.ExecuteNonQuery();

                // Returns true if the there are zero groups with the name give
                closeConnection();
                return (rowsEffected > 0);
            }
            catch (Exception e)
            {
                Console.Error.Write(e.Message);
                closeConnection();
                return false;
            }
        }


        /// <summary>
        /// Searchs for the name of a specified group.
        /// </summary>
        /// <returns><c>true</c>, if group name was found, <c>false</c> otherwise.</returns>
        /// <param name="groupName">Group name.</param>
        public bool searchGroupName(string groupName)
        {
            try
            {
                openConnection();
                MySqlDataReader reader = null;
                string queryResults = string.Empty;

                MySqlCommand query = new MySqlCommand(
                    "SELECT connection_group_name FROM guacamole_connection_group " +
                    "WHERE connection_group_name=@groupname",
                    connection);

                query.Prepare();
                query.Parameters.AddWithValue("@groupname", groupName);
                reader = query.ExecuteReader();

                while (reader.Read())
                {
                    queryResults = (string)reader["connection_group_name"];
                }

                // Returns true if the there are zero groups with the name give
                closeConnection();
                return (queryResults == string.Empty);
            }
            catch (Exception e)
            {
                Console.Error.Write(e.Message);
                closeConnection();
                return false;
            }
        }


        /// <summary>
        /// Creates a list of every virtual machine connection.
        /// </summary>
        /// <returns>An ICollection<string> of all virtual machine names</returns>
        /// /// <param name="groupName">Group name.</param>
        public ICollection<string> getAllGroupVmNames(string groupName)
        {
            try
            {
                openConnection();
                MySqlDataReader reader = null;
                ICollection<string> queryResults = new List<string>();

                MySqlCommand query = new MySqlCommand(
                    "SELECT connection_name FROM guacamole_connection, guacamole_connection_group " +
                    "WHERE guacamole_connection.parent_id=guacamole_connection_group.connection_group_id " +
                    "AND guacamole_connection_group.connection_group_name=@groupname", connection);

                query.Prepare();
                query.Parameters.AddWithValue("@groupname", groupName);
                reader = query.ExecuteReader();

                while (reader.Read())
                {
                    queryResults.Add((string)reader["connection_name"]);
                }

                // Returns the list of virtual machine connection names
                closeConnection();
                return queryResults;
            }
            catch (Exception e)
            {
                Console.Error.Write(e.Message);
                closeConnection();
                return null;
            }
        }


        /// <summary>
        /// Gets every virtual machine connection in a particular connection group.
        /// </summary>
        /// <returns>An ICollection<string> of all virtual machine names in the group</returns>
        public ICollection<string> getAllVmNames()
        {
            try
            {
                openConnection();
                MySqlDataReader reader = null;
                ICollection<string> queryResults = new List<string>();

                MySqlCommand query = new MySqlCommand(
                    "SELECT connection_name FROM guacamole_connection ", connection);

                query.Prepare();
                reader = query.ExecuteReader();

                while (reader.Read())
                {
                    queryResults.Add((string)reader["connection_name"]);
                }

                // Returns the list of virtual machine connection names
                closeConnection();
                return queryResults;
            }
            catch (Exception e)
            {
                Console.Error.Write(e.Message);
                closeConnection();
                return null;
            }
        }
    }
}
