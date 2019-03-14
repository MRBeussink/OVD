using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using test_OVD_clientless.Models;
using test_OVD_clientless.Exceptions;

namespace test_OVD_clientless.GuacamoleDatabaseConnectors
{
    public class GuacamoleDatabaseInserter
    {
        private const string MAX_USER_CONNECTIONS = "1";

        /// <summary>
        /// Inserts a new group into the Guacamole mysql database
        /// </summary>
        /// <returns><c>true</c>, if group was inserted, <c>false</c> otherwise.</returns>
        /// <param name="newGroup">Group Object.</param>
        public bool insertGroup(GroupConfig newGroup, ref List<Exception> exceptions)
        {
            string queryString =
                "INSERT INTO guacamole_connection_group (connection_group_name, max_connections, max_connections_per_user) " +
                "VALUES (@groupname, @maxconnections, @maxuserconnections)";

            Queue<string> argNames = new Queue<string>();
            argNames.Enqueue("@groupname");
            argNames.Enqueue("@maxconnections");
            argNames.Enqueue("@maxuserconnections");

            Queue<string> args = new Queue<string>();
            args.Enqueue(newGroup.groupName);
            args.Enqueue(newGroup.maxNum.ToString());
            args.Enqueue(MAX_USER_CONNECTIONS);

            return insertQuery(queryString, argNames, args, ref exceptions);
        }


        /// <summary>
        /// Inserts a new user into the guacamole entity table and the guacamole 
        /// user table. This method assumes that authentication is handled external
        /// of the guacamole database. This is why the placeholder of INVALID is used
        /// for the password hash field as required by the guacamole database.
        /// </summary>
        /// <returns><c>true</c>, if user was inserted, <c>false</c> otherwise.</returns>
        /// <param name="dawgtag">Dawgtag.</param>
        public bool insertUser(string dawgtag, ref List<Exception> exceptions)
        {
            const string HASH = "INVALID";
            int entityId = -1;

            string entityQueryString =
                "INSERT INTO guacamole_entity (name, type) " +
                "VALUES (@username, 'USER')";

            string entityIdQueryString =
                "SELECT entity_id FROM guacamole_entity " +
                "WHERE name=@username AND type = 'USER'";

            string userQueryString =
                "INSERT INTO guacamole_user (entity_id, password_hash, password_date) " +
                "VALUES(@entityid, @hash, CURRENT_TIMESTAMP)";
            try
            {
                using(GuacamoleDatabaseConnector gdbc = new GuacamoleDatabaseConnector(ref exceptions))
                {
                    MySqlCommand entityQuery = new MySqlCommand(entityQueryString, gdbc.getConnection());
                    MySqlCommand entityIdQuery = new MySqlCommand(entityIdQueryString, gdbc.getConnection());
                    MySqlCommand userQuery = new MySqlCommand(userQueryString, gdbc.getConnection());

                    //Add the user to the entity table
                    entityQuery.Prepare();
                    entityQuery.Parameters.AddWithValue("@username", dawgtag);
                    if (entityQuery.ExecuteNonQuery() <= 0)
                    {
                        exceptions.Add(new UserInitializationException("Could not add the user (" +
                            dawgtag + ") into the guacamole mysql database table guacamole_entity.\n\n"));
                        return false;
                    }

                    //Retreive the newly created entity id
                    entityIdQuery.Prepare();
                    entityIdQuery.Parameters.AddWithValue("@username", dawgtag);
                    using(MySqlDataReader reader = entityIdQuery.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            entityId = (int)reader["entity_id"];
                            if (entityId == -1)
                            {
                                exceptions.Add(new UserInitializationException("Could not find the user (" +
                            dawgtag + ") entity_id within the guacamole mysql database table guacamole_entity.\n\n"));
                                return false;
                            }
                        }
                    }

                    //Insert a value for the user and the hash
                    userQuery.Prepare();
                    userQuery.Parameters.AddWithValue("@entityid", entityId);
                    userQuery.Parameters.AddWithValue("@hash", HASH);
                    if (userQuery.ExecuteNonQuery() <= 0)
                    {
                        exceptions.Add(new UserInitializationException("Could not add the user (" +
                            dawgtag + ") into the guacamole mysql database table guacamole_user.\n\n"));
                        return false;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                exceptions.Add(e);
                return false;
            }
        }


        public bool insertVm(string vmName, string vmChioce, ref List<Exception> exceptions)
        {
            return true;
        }


        /// <summary>
        /// Inserts the vm connectino information into the group.
        /// </summary>
        /// <returns><c>true</c>, if vm was inserted in the group, <c>false</c> otherwise.</returns>
        /// <param name="vm">Vm.</param>
        /// <param name="group">Group.</param>
        /// <param name="exceptions">Exceptions.</param>
        public bool insertVmIntoGroup(VirtualMachine vm, GroupConfig group, ref List<Exception> exceptions)
        {
            string queryString =
                "INSERT INTO guacamole_connection_group (connection_group_name, max_connections, max_connections_per_user) " +
                "VALUES (@groupname, @maxconnections, @maxuserconnections)";
            try
            {
                using (GuacamoleDatabaseConnector gdbc = new GuacamoleDatabaseConnector(ref exceptions))
                {
                    MySqlCommand query = new MySqlCommand(queryString, gdbc.getConnection());
                    query.Prepare();
                    query.Parameters.AddWithValue("@groupname", group.groupName);
                    query.Parameters.AddWithValue("@maxconnections", group.maxNum);
                    query.Parameters.AddWithValue("@maxuserconnections", MAX_USER_CONNECTIONS);

                    return query.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception e)
            {
                exceptions.Add(e);
                return false;
            }
        }

        public bool insertUserIntoGroup(string dawgtag, string groupName, ref List<Exception> exceptions)
        {
            return true;
        }



        /// <summary>
        /// General format for running a insertion query on the guacamole database
        /// </summary>
        /// <returns><c>true</c>, if the values were inserted, <c>false</c> otherwise.</returns>
        /// <param name="queryString">Query string.</param>
        /// <param name="argNames">Argument names.</param>
        /// <param name="args">Arguments.</param>
        /// <param name="exceptions">Exceptions.</param>
        public bool insertQuery(string queryString, Queue<string> argNames, Queue<string> args, ref List<Exception> exceptions)
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
                        return (query.ExecuteNonQuery() > 0);
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
