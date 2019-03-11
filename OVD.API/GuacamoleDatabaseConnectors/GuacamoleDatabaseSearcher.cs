using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace test_OVD_clientless.GuacamoleDatabaseConnectors
{
    public class GuacamoleDatabaseSearcher
    {
        /// <summary>
        /// Searchs for the name of a specified group.
        /// </summary>
        /// <returns><c>true</c>, if group name was found, <c>false</c> otherwise.</returns>
        /// <param name="groupName">Group name.</param>
        public bool searchGroupName(string groupName)
        {
            try
            {
                GuacamoleDatabaseConnector gdbc = new GuacamoleDatabaseConnector();
                MySqlDataReader reader = null;
                string queryResults = string.Empty;

                MySqlCommand query = new MySqlCommand(
                    "SELECT connection_group_name FROM guacamole_connection_group " +
                    "WHERE connection_group_name=@groupname",
                    gdbc.getConnection());

                query.Prepare();
                query.Parameters.AddWithValue("@groupname", groupName);
                reader = query.ExecuteReader();

                while (reader.Read())
                {
                    queryResults = (string)reader["connection_group_name"];
                }

                // Returns true if the there are zero groups with the name give
                return (queryResults == string.Empty);
            }
            catch (Exception e)
            {
                Console.Error.Write(e.Message);
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
                GuacamoleDatabaseConnector gdbc = new GuacamoleDatabaseConnector();
                MySqlDataReader reader = null;
                ICollection<string> queryResults = new List<string>();

                MySqlCommand query = new MySqlCommand(
                    "SELECT connection_name FROM guacamole_connection, guacamole_connection_group " +
                    "WHERE guacamole_connection.parent_id=guacamole_connection_group.connection_group_id " +
                    "AND guacamole_connection_group.connection_group_name=@groupname", gdbc.getConnection());

                query.Prepare();
                query.Parameters.AddWithValue("@groupname", groupName);
                reader = query.ExecuteReader();

                while (reader.Read())
                {
                    queryResults.Add((string)reader["connection_name"]);
                }

                // Returns the list of virtual machine connection names
                return queryResults;
            }
            catch (Exception e)
            {
                Console.Error.Write(e.Message);
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
                GuacamoleDatabaseConnector gdbc = new GuacamoleDatabaseConnector();
                MySqlDataReader reader = null;
                ICollection<string> queryResults = new List<string>();

                MySqlCommand query = new MySqlCommand(
                    "SELECT connection_name FROM guacamole_connection ", gdbc.getConnection());

                query.Prepare();
                reader = query.ExecuteReader();

                while (reader.Read())
                {
                    queryResults.Add((string)reader["connection_name"]);
                }

                // Returns the list of virtual machine connection names
                return queryResults;
            }
            catch (Exception e)
            {
                Console.Error.Write(e.Message);
                return null;
            }
        }
    }
}
