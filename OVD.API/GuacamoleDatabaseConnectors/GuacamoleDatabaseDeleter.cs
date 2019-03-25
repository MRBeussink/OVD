using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using OVD.API.Exceptions;

namespace OVD.API.GuacamoleDatabaseConnectors
{
    public class GuacamoleDatabaseDeleter
    {

        /// <summary>
        /// Deletes the user group associated with the given name.
        /// All members associated with the group are removed.
        /// </summary>
        /// <returns><c>true</c>, if user group was deleted, <c>false</c> otherwise.</returns>
        /// <param name="groupName">Group name.</param>
        /// <param name="exceptions">Exceptions.</param>
        public bool deleteUserGroup(string groupName, ref List<Exception> exceptions)
        {
            const string queryString =
                "DELETE FROM guacamole_entity " +
                "WHERE name=@groupname AND type='USER_GROUP'";

            Queue<string> argNames = new Queue<string>();
            argNames.Enqueue("@groupname");

            Queue<string> args = new Queue<string>();
            args.Enqueue(groupName);

            //Insert the usergroup into the entity table
            if (!deleteQuery(queryString, argNames, args, ref exceptions))
            {
                exceptions.Add(new UserInitializationException("The user group with the name (" +
                        groupName + ") could not be deleted. Please check the status of the " +
                        	"guacamole mysql database.\n\n"));
                return false;
            }
            return true;
        }


        /// <summary>
        /// Deletes the connection group associated with the given group name.
        /// All connections associated with this connection group are auto deleted
        /// </summary>
        /// <returns><c>true</c>, if connection group was deleted, <c>false</c> otherwise.</returns>
        /// <param name="groupName">Group name.</param>
        /// <param name="exceptions">Exceptions.</param>
        public bool deleteConnectionGroup(string groupName, ref List<Exception> exceptions)
        {
            const string queryString =
                "DELETE FROM guacamole_connection_group " +
                "WHERE connection_group_name=@groupname";

            Queue<string> argNames = new Queue<string>();
            argNames.Enqueue("@groupname");

            Queue<string> args = new Queue<string>();
            args.Enqueue(groupName);

            //Insert the usergroup into the entity table
            if (!deleteQuery(queryString, argNames, args, ref exceptions))
            {
                exceptions.Add(new UserInitializationException("The connection group with the name (" +
                        groupName + ") could not be deleted. Please check the status of the " +
                            "guacamole mysql database.\n\n"));
                return false;
            }
            return true;
        }


        /// <summary>
        /// General format for running a deletion query on the guacamole database
        /// </summary>
        /// <returns><c>true</c>, if the values were deleted, <c>false</c> otherwise.</returns>
        /// <param name="queryString">Query string.</param>
        /// <param name="argNames">Argument names.</param>
        /// <param name="args">Arguments.</param>
        /// <param name="exceptions">Exceptions.</param>
        public bool deleteQuery(string queryString, Queue<string> argNames, Queue<string> args, ref List<Exception> exceptions)
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
