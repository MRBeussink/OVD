using System;
using MySql.Data.MySqlClient;
using test_OVD_clientless.Models;

namespace test_OVD_clientless.GuacamoleDatabaseConnectors
{
    public class GuacamoleDatabaseInserter
    {

        private const int MAX_USER_CONNECTIONS = 1;


        /// <summary>
        /// Inserts a new group into the Guacamole mysql database
        /// </summary>
        /// <returns><c>true</c>, if group was inserted, <c>false</c> otherwise.</returns>
        /// <param name="newGroup">Group Object.</param>
        public bool insertGroup(Group newGroup)
        {
            try
            {
                GuacamoleDatabaseConnector gdbc = new GuacamoleDatabaseConnector();
                MySqlCommand query = new MySqlCommand(
                    "INSERT INTO guacamole_connection_group (connection_group_name, " +
                    "max_connections, max_connections_per_user)" +
                    "VALUES (@groupname, @maxconnections, @maxuserconnections)",
                    gdbc.getConnection());

                query.Prepare();
                query.Parameters.AddWithValue("@groupname", newGroup.name);
                query.Parameters.AddWithValue("@maxconnections", newGroup.config.maxNum);
                query.Parameters.AddWithValue("@maxuserconnections", MAX_USER_CONNECTIONS);

                int rowsEffected = query.ExecuteNonQuery();

                // Returns true if the there are zero groups with the name give
                return (rowsEffected > 0);
            }
            catch (Exception e)
            {
                Console.Error.Write(e.Message);
                return false;
            }
        }
    }
}
