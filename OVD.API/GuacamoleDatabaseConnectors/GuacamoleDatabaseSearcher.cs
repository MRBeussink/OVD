using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using test_OVD_clientless.Exceptions;

namespace test_OVD_clientless.GuacamoleDatabaseConnectors
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
            string queryString =
                "SELECT connection_group_name FROM guacamole_connection_group " +
                "WHERE connection_group_name=@groupname";

            Queue<string> argNames = new Queue<string>();
            argNames.Enqueue("@groupname");

            Queue<string> args = new Queue<string>();
            args.Enqueue(groupName);

            return searchQuery(queryString, argNames, args, ref exceptions);
        }


        /// <summary>
        /// Searchs for the dawgtag of the user.
        /// </summary>
        /// <returns><c>true</c>, if user name was found, <c>false</c> otherwise.</returns>
        /// <param name="dawgtag">Dawgtag.</param>
        public bool searchUserName(string dawgtag, ref List<Exception> exceptions)
        {
            string queryString =
                "SELECT name FROM guacamole_entity " +
                "WHERE type='USER' AND name=@username";

            Queue<string> argNames = new Queue<string>();
            argNames.Enqueue("@username");

            Queue<string> args = new Queue<string>();
            args.Enqueue(dawgtag);

            return searchQuery(queryString, argNames, args, ref exceptions);
        }


        /// <summary>
        /// Searchs for the name of the vm.
        /// </summary>
        /// <returns><c>true</c>, if vm name was found, <c>false</c> otherwise.</returns>
        /// <param name="vmName">Vm name.</param>
        public bool searchVmName(string vmName, ref List<Exception> exceptions)
        {
            string queryString =
                "SELECT connection_name FROM guacamole_connection " +
                 "WHERE connection_name=@vmname";

            Queue<string> argNames = new Queue<string>();
            argNames.Enqueue("@vmname");

            Queue<string> args = new Queue<string>();
            args.Enqueue(vmName);

            return searchQuery(queryString, argNames, args, ref exceptions);
        }


        /// <summary>
        /// Gets the vm identifier number.
        /// </summary>
        /// <returns>The vm identifier number.</returns>
        public int getMaxVmId(ref List<Exception> exceptions)
        {
            string queryString = "SELECT MAX(connection_id) FROM guacamole_connection";
            try
            {
                using (GuacamoleDatabaseConnector gdbc = new GuacamoleDatabaseConnector(ref exceptions))
                {
                    MySqlCommand query = new MySqlCommand(queryString, gdbc.getConnection());
                    query.Prepare();
                    return Convert.ToInt32(query.ExecuteScalar());
                }
            }
            catch (Exception e)
            {
                exceptions.Add(e);
                return -1;
            }
        }


        /// <summary>
        /// Creates a list of every virtual machine connection.
        /// </summary>
        /// <returns>An ICollection<string> of all virtual machine names</returns>
        /// /// <param name="groupName">Group name.</param>
        public ICollection<string> getAllGroupVmNames(string groupName, ref List<Exception> exceptions)
        {
            string queryString =
                "SELECT connection_name FROM guacamole_connection, guacamole_connection_group " +
                "WHERE guacamole_connection.parent_id=guacamole_connection_group.connection_group_id " +
                "AND guacamole_connection_group.connection_group_name=@groupname";
            try
            {
                ICollection<string> queryResults = new List<string>();

                using(GuacamoleDatabaseConnector gdbc = new GuacamoleDatabaseConnector(ref exceptions))
                {
                    MySqlCommand query = new MySqlCommand(queryString, gdbc.getConnection());
                    query.Prepare();
                    query.Parameters.AddWithValue("@groupname", groupName);

                    using (MySqlDataReader reader = query.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            queryResults.Add((string)reader["connection_name"]);
                        }
                    }
                }
                // Returns the list of virtual machine connection names
                return queryResults;
            }
            catch (Exception e)
            {
                exceptions.Add(e);
                return null;
            }
        }


        /// <summary>
        /// General format for running a search query on the guacamole database
        /// </summary>
        /// <returns><c>true</c>, if field was found, <c>false</c> otherwise.</returns>
        /// <param name="queryString">Query string.</param>
        /// <param name="argNames">Argument names.</param>
        /// <param name="args">Arguments.</param>
        /// <param name="exceptions">Exceptions.</param>
        public bool searchQuery(string queryString, Queue<string> argNames, Queue<string> args, ref List<Exception> exceptions)
        {
            //Validate if the arguments and names are the correct amount
            if (args.Count != argNames.Count)
            {
                exceptions.Add(new InvalidDatabaseArgumentException("The database arguments and argument names " +
                    "do not match. Ensure that the query is sending the proper arguments.\n\n"));
                return false;
            }

            try
            {
                using (GuacamoleDatabaseConnector gdbc = new GuacamoleDatabaseConnector(ref exceptions))
                {
                    using (MySqlCommand query = new MySqlCommand(queryString, gdbc.getConnection()))
                    {
                        query.Prepare();

                        //Add the agrument names and values NOTE: ORDER MATTERS
                        while (args.Count > 0 && argNames.Count > 0)
                        {
                            query.Parameters.AddWithValue(argNames.Dequeue(), args.Dequeue());
                        }
                        return (query.ExecuteScalar() != null);
                    }
                }
            }
            catch (Exception e)
            {
                exceptions.Add(e);
                return false;
            }
        }
    }
}
