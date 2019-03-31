﻿using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using OVD.API.Exceptions;

namespace OVD.API.GuacamoleDatabaseConnectors
{
    public class GuacamoleDatabaseDeleter : IGuacDeleter
    {

        /// <summary>
        /// Deletes the user group associated with the given name.
        /// All members associated with the group are removed.
        /// </summary>
        /// <returns><c>true</c>, if user group was deleted, <c>false</c> otherwise.</returns>
        /// <param name="groupName">Group name.</param>
        /// <param name="excepts">Exceptions.</param>
        public bool DeleteUserGroup(string groupName, ref List<Exception> excepts)
        {
            const string queryString =
                "DELETE FROM guacamole_entity " +
                "WHERE name=@groupname AND type='USER_GROUP'";

            Queue<string> argNames = new Queue<string>();
            argNames.Enqueue("@groupname");

            Queue<string> args = new Queue<string>();
            args.Enqueue(groupName);

            //Insert the usergroup into the entity table
            return DeleteQuery(queryString, argNames, args, ref excepts);
        }


        /// <summary>
        /// Deletes the connection group associated with the given group name.
        /// All connections associated with this connection group are auto deleted
        /// </summary>
        /// <returns><c>true</c>, if connection group was deleted, <c>false</c> otherwise.</returns>
        /// <param name="groupName">Group name.</param>
        /// <param name="excepts">Exceptions.</param>
        public bool DeleteConnectionGroup(string groupName, ref List<Exception> excepts)
        {
            const string queryString =
                "DELETE FROM guacamole_connection_group " +
                "WHERE connection_group_name=@groupname";

            Queue<string> argNames = new Queue<string>();
            argNames.Enqueue("@groupname");

            Queue<string> args = new Queue<string>();
            args.Enqueue(groupName);

            //Insert the usergroup into the entity table
            return DeleteQuery(queryString, argNames, args, ref excepts);
        }


        /// <summary>
        /// Deletes the user from user group.
        /// </summary>
        /// <returns><c>true</c>, if user was deleted from the user group, <c>false</c> otherwise.</returns>
        /// <param name="groupName">Group name.</param>
        /// <param name="dawgtag">Dawgtag.</param>
        /// <param name="excepts">Excepts.</param>
        public bool DeleteUserFromUserGroup(string groupName, string dawgtag, ref List<Exception> excepts)
        {
            const string groupIdQueryString =
                "(SELECT user_group_id FROM guacamole_user_group, guacamole_entity " +
                "WHERE name = @groupname AND type = 'USER_GROUP' " +
                "AND guacamole_user_group.entity_id=guacamole_entity.entity_id)";

            const string userIdQueryString =
                "(SELECT entity_id FROM guacamole_entity " +
                "WHERE name = @username AND type = 'USER')";

            const string memberQueryString =
                "DELETE FROM guacamole_user_group_member " +
                "WHERE user_group_id=" + groupIdQueryString + "AND member_entity_id =" + userIdQueryString;

            Queue<string> argNames = new Queue<string>();
            argNames.Enqueue("@groupname");
            argNames.Enqueue("@username");

            Queue<string> args = new Queue<string>();
            args.Enqueue(groupName);
            args.Enqueue(dawgtag);

            //Insert the user into the entity table
            return DeleteQuery(memberQueryString, argNames, args, ref excepts);
        }


        /// <summary>
        /// General format for running a deletion query on the guacamole database
        /// </summary>
        /// <returns><c>true</c>, if the values were deleted, <c>false</c> otherwise.</returns>
        /// <param name="queryString">Query string.</param>
        /// <param name="argNames">Argument names.</param>
        /// <param name="args">Arguments.</param>
        /// <param name="excepts">Exceptions.</param>
        private bool DeleteQuery(string queryString, Queue<string> argNames, Queue<string> args, ref List<Exception> excepts)
        {
            const string exceptMessage = "The database arguments and argument names are not the same size.";

            //Validate if the arguments and names are the correct amount
            if (args.Count != argNames.Count)
            {
                excepts.Add(new GuacamoleDatabaseException(exceptMessage));
                return false;
            }

            //Make a deep copy of the queues to ensure data consistancy
            Queue<String> copiedArgNames = new Queue<String>(argNames.ToArray());
            Queue<String> copiedArgs = new Queue<String>(args.ToArray());

            try
            {
                using (GuacamoleDatabaseConnector gdbc = new GuacamoleDatabaseConnector(ref excepts))
                {
                    using (MySqlCommand query = new MySqlCommand(queryString, gdbc.getConnection()))
                    {
                        query.Prepare();

                        //Add the agrument names and values NOTE: ORDER MATTERS
                        while (copiedArgs.Count > 0 && copiedArgNames.Count > 0)
                        {
                            query.Parameters.AddWithValue(copiedArgNames.Dequeue(), copiedArgs.Dequeue());
                        }
                        return (query.ExecuteNonQuery() > 0);
                    }
                }
            }
            catch (Exception e)
            {
                excepts.Add(e);
                return false;
            }
        }
    }
}
