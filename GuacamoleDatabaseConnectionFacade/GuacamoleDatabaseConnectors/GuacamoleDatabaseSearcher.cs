using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace OVD.API.GuacamoleDatabaseConnectors
{
    public class GuacamoleDatabaseSearcher
    {
        /// <summary>
        /// Searchs for the name of a specified group.
        /// </summary>
        /// <returns><c>true</c>, if group name was found, <c>false</c> otherwise.</returns>
        /// <param name="groupName">Group name.</param>
        public bool searchGroupName(string groupName, ref List<Exception> exceptions)
        {
            const string queryString =
                "SELECT connection_group_name FROM guacamole_connection_group " +
                "WHERE connection_group_name=@input";

            Queue<string> queryResults = searchQuery(queryString, groupName, ref exceptions);
            return queryResults.Count != 0;
        }


        /// <summary>
        /// Searchs for the dawgtag of the user.
        /// </summary>
        /// <returns><c>true</c>, if user name was found, <c>false</c> otherwise.</returns>
        /// <param name="dawgtag">Dawgtag.</param>
        public bool searchUserName(string dawgtag, ref List<Exception> exceptions)
        {
            const string queryString =
                "SELECT name FROM guacamole_entity " +
                "WHERE type='USER' AND name=@input";

            Queue<string> queryResults = searchQuery(queryString, dawgtag, ref exceptions);
            return queryResults.Count != 0;
        }


        /// <summary>
        /// Searchs for the name of the vm.
        /// </summary>
        /// <returns><c>true</c>, if vm name was found, <c>false</c> otherwise.</returns>
        /// <param name="vmName">Vm name.</param>
        public bool searchVmName(string vmName, ref List<Exception> exceptions)
        {
            const string queryString =
                "SELECT connection_name FROM guacamole_connection " +
                 "WHERE connection_name=@input";

            Queue<string> queryResults = searchQuery(queryString, vmName, ref exceptions);
            return queryResults.Count != 0;
        }


        /// <summary>
        /// Gets the vm identifier number.
        /// </summary>
        /// <returns>The vm identifier number.</returns>
        public int getMaxVmId(ref List<Exception> exceptions)
        {
            const string queryString = "SELECT MAX(connection_id) FROM guacamole_connection";

            Queue<string> queryResults = searchQuery(queryString, null, ref exceptions);
            return Int32.Parse(queryResults.Dequeue());
        }


        /// <summary>
        /// Creates a list of every virtual machine connection.
        /// </summary>
        /// <returns>An ICollection<string> of all virtual machine names</returns>
        /// /// <param name="groupName">Group name.</param>
        public Queue<string> getAllGroupVmNames(string groupName, ref List<Exception> exceptions)
        {
            const string queryString =
                "SELECT connection_name FROM guacamole_connection, guacamole_connection_group " +
                "WHERE guacamole_connection.parent_id=guacamole_connection_group.connection_group_id " +
                "AND guacamole_connection_group.connection_group_name=@input";

            Queue<string> queryResults = searchQuery(queryString, groupName, ref exceptions);
            return queryResults;

        }


        /// <summary>
        /// Gets all group names.
        /// </summary>
        /// <returns>The all group names.</returns>
        /// <param name="exceptions">Exceptions.</param>
        public Queue<string> getAllGroupNames(ref List<Exception> exceptions)
        {
            const string queryString =
                "SELECT connection_group_name FROM guacamole_connection_group";

            Queue<string> queryResults = searchQuery(queryString, null, ref exceptions);
            return queryResults;
        }


        /// <summary>
        /// General format for running a search query on the guacamole database
        /// </summary>
        /// <returns><c>true</c>, if field was found, <c>false</c> otherwise.</returns>
        /// <param name="queryString">Query string.</param>
        /// <param name="arg">Argument.</param>
        /// <param name="exceptions">Exceptions.</param>
        public Queue<string> searchQuery(string queryString, string arg, ref List<Exception> exceptions)
        {
            try
            {
                using (GuacamoleDatabaseConnector gdbc = new GuacamoleDatabaseConnector(ref exceptions))
                {
                    using (MySqlCommand query = new MySqlCommand(queryString, gdbc.getConnection()))
                    {
                        query.Prepare();

                        //Add the agruments given if found
                        if (arg != null)
                        {
                            query.Parameters.AddWithValue("@input", arg);
                        }

                        //Collect the query result column
                        Queue<string> queryResults = new Queue<string>();
                        using (MySqlDataReader reader = query.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                queryResults.Enqueue((string)reader[0]);
                            }
                        }
                        return queryResults;
                    }
                }
            }
            catch (Exception e)
            {
                exceptions.Add(e);
                return null;
            }
        }
    }
}
