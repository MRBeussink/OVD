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
        public bool insertConnectionGroup(GroupConfig newGroup, ref List<Exception> exceptions)
        {
            const string queryString =
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
        /// Creates the desired user group that will allow users to see their
        /// available connections.
        /// </summary>
        /// <returns><c>true</c>, if the usergroup was inserted, <c>false</c> otherwise.</returns>
        /// <param name="newGroup">New group.</param>
        /// <param name="exceptions">Exceptions.</param>
        public bool insertUserGroup(GroupConfig newGroup, ref List<Exception> exceptions)
        {
            const string entityQueryString =
                "INSERT INTO guacamole_entity (name, type) " +
                "VALUES (@groupname, 'USER_GROUP')";

            const string entityIdQueryString =
                "(SELECT entity_id FROM guacamole_entity " +
                "WHERE name = @groupname AND type = 'USER_GROUP')";

            const string groupQueryString =
                "INSERT INTO guacamole_user_group (entity_id) " +
                "VALUES (" + entityIdQueryString + ")";

            Queue<string> argNames = new Queue<string>();
            argNames.Enqueue("@groupname");

            Queue<string> args = new Queue<string>();
            args.Enqueue(newGroup.groupName);

            //Insert the usergroup into the entity table
            if (!insertQuery(entityQueryString, argNames, args, ref exceptions))
            {
                exceptions.Add(new UserInitializationException("The group with the name (" +
                        newGroup.groupName + ") could not be added to the entity table. Please " +
                            "check the status of the guacamole mysql database.\n\n"));
                return false;
            }

            //Insert the user group into the user group table
            if (!insertQuery(groupQueryString, argNames, args, ref exceptions))
            {
                exceptions.Add(new UserInitializationException("The group with the name (" +
                        newGroup.groupName + ") could not be added to the user table. Please " +
                            "check the status of the guacamole mysql database.\n\n"));
                return false;
            }
            return true;
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

            const string entityQueryString =
                "INSERT INTO guacamole_entity (name, type) " +
                "VALUES (@username, 'USER')";

            const string entityIdQueryString =
                "(SELECT entity_id FROM guacamole_entity " +
                "WHERE name = @username AND type = 'USER')";

            const string userQueryString =
                "INSERT INTO guacamole_user (entity_id, password_hash, password_date) " +
                "VALUES (" + entityIdQueryString + ", 'INVALID', CURRENT_TIMESTAMP)";
                
            Queue<string> argNames = new Queue<string>();
            argNames.Enqueue("@username");

            Queue<string> args = new Queue<string>();
            args.Enqueue(dawgtag);

            //Insert the user into the entity table
            if (!insertQuery(entityQueryString, argNames, args, ref exceptions))
            {
                exceptions.Add(new UserInitializationException("The user with the dawgtag (" +
                        dawgtag + ") could not be added to the entity table. Please check the status of " +
                        "the guacamole mysql database.\n\n"));
                return false;
            }

            //Insert user and fake hash into the user users table
            if(!insertQuery(userQueryString, argNames, args, ref exceptions))
            {
                exceptions.Add(new UserInitializationException("The user with the dawgtag (" +
                        dawgtag + ") could not be added to the user table. Please check the status of " +
                        "the guacamole mysql database.\n\n"));
                return false;
            }
            return true;
        }


        public bool insertVm(string vmName, string vmChioce, ref List<Exception> exceptions)
        {
            return true;
        }


        public bool insertVmIntoConnectionGroup(VirtualMachine vm, GroupConfig group, ref List<Exception> exceptions)
        {
            return true;
        }


        /// <summary>
        /// Inserts the user into the user group.
        /// </summary>
        /// <returns><c>true</c>, if user was inserted into the user group<c>false</c> otherwise.</returns>
        /// <param name="dawgtag">Dawgtag.</param>
        /// <param name="groupName">Group name.</param>
        /// <param name="exceptions">Exceptions.</param>
        public bool insertUserIntoUserGroup(string dawgtag, string groupName, ref List<Exception> exceptions)
        {
            const string groupIdQueryString =
                "(SELECT user_group_id FROM guacamole_user_group, guacamole_entity " +
                "WHERE name = @groupname AND type = 'USER_GROUP' " +
                "AND guacamole_user_group.entity_id=guacamole_entity.entity_id)";

            const string userIdQueryString =
                "(SELECT entity_id FROM guacamole_entity " +
                "WHERE name = @username AND type = 'USER')";

            const string memberQueryString =
                "INSERT INTO guacamole_user_group_member (user_group_id, member_entity_id) " +
                "VALUES (" + groupIdQueryString + "," + userIdQueryString + ")";

            Queue<string> argNames = new Queue<string>();
            argNames.Enqueue("@groupname");
            argNames.Enqueue("@username");

            Queue<string> args = new Queue<string>();
            args.Enqueue(groupName);
            args.Enqueue(dawgtag);

            //Insert the user into the entity table
            if (!insertQuery(memberQueryString, argNames, args, ref exceptions))
            {
                exceptions.Add(new UserInitializationException("The user with the dawgtag (" +
                        dawgtag + ") could not be added to the user group (" + groupName +
                        "). Please check the status of the guacamole mysql database.\n\n"));
                return false;
            }
            return true;
        }



        public bool insertConnectionGroupIntoUserGroup(string groupName, ref List<Exception> exceptions)
        {

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

            //Make a deep copy of the queues to ensure data consistancy
            Queue<String> copiedArgNames = new Queue<String>(argNames.ToArray());
            Queue<String> copiedArgs = new Queue<String>(args.ToArray());

            try
            {
                using (GuacamoleDatabaseConnector gdbc = new GuacamoleDatabaseConnector(ref exceptions))
                {
                    using (MySqlCommand query = new MySqlCommand(queryString, gdbc.getConnection()))
                    {
                        query.Prepare();

                        //Add the agrument names and values NOTE: ORDER MATTERS
                        while (copiedArgs.Count > 0 && copiedArgNames.Count > 0)
                        {
                            Console.Error.Write("Name = " + copiedArgNames.Peek() + " Arg = " + copiedArgs.Peek() + ".\n\n");
                            query.Parameters.AddWithValue(copiedArgNames.Dequeue(), copiedArgs.Dequeue());
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
