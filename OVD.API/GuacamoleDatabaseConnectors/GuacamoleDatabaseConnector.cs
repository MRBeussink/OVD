using System;
using MySql.Data.MySqlClient;

namespace test_OVD_clientless.GuacamoleDatabaseConnectors
{
    public class GuacamoleDatabaseConnector
    {

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
            openConnection();
        }


        /// <summary>
        /// Gets the MySqlDatabase connection.
        /// </summary>
        /// <returns>The Guacamole database connection.</returns>
        public MySqlConnection getConnection()
        {
            return this.connection;
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
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="T:test_OVD_clientless.GuacamoleDatabaseConnectors.GuacamoleDatabaseConnector"/> is reclaimed by
        /// garbage collection.
        /// </summary>
        ~GuacamoleDatabaseConnector()
        {
            closeConnection();
        }
    }
}
