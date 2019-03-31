using System;
using System.Collections.Generic;
using System.Xml;
using OVD.API.Dtos;
using OVD.API.Helpers;
using OVD.API.GuacamoleDatabaseConnectors;

namespace OVD.API.Controllers
{
    public class GroupController
    {

        private const string CONFIG_FILE_LOC = "./ConfigurationFiles/ConnectionTypes.xml";


        /// <summary>
        /// Gets the connection types available from the connection types
        /// configuration file.
        /// </summary>
        /// <returns>The connection types.</returns>
        /// <param name="userId">User identifier.</param>
        public ICollection<string> GetConnectionTypes(string userId)
        {
            ICollection<string> connectionTypes = new List<string>();

            //Read in the xml connection type configuration file
            XmlDocument config = new XmlDocument();
            config.Load(CONFIG_FILE_LOC);

            XmlNodeList elemList = config.GetElementsByTagName("type");
            for (int i = 0; i < elemList.Count; i++)
            {
                connectionTypes.Add(elemList[i].InnerXml);
            }
            return connectionTypes;
        }


        /// <summary>
        /// Deletes the specified connection group and user group.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="groupName">Group name.</param>
        public void DeleteGroup(string userId, string groupName)
        {
            List<Exception> excepts = new List<Exception>();
            GuacamoleDatabaseDeleter deleter = new GuacamoleDatabaseDeleter();
            deleter.DeleteUserGroup(groupName, ref excepts);
            deleter.DeleteConnectionGroup(groupName, ref excepts);

            if(excepts.Count == 0)
            {
                return;
            }
            else
            {
                Console.Write("Error");
                return;
            }
        }


        /// <summary>
        /// Formats the given user inputs to ensure data consistancy when stored.
        /// </summary>
        /// <returns><c>true</c>, if the input was formated, <c>false</c> otherwise.</returns>
        /// <param name="groupForCreationDto">Group for creation dto.</param>
        /// <param name="excepts">Excepts.</param>
        protected bool FormatInput(GroupForCreationDto groupForCreationDto, ref List<Exception> excepts)
        {
            // Format User Text Input to be standardized to the following:
            //EX. test_group_1, ubuntu_16.04
            using (Formatter styler = new Formatter())
            {
                groupForCreationDto.Name = styler.FormatGroupName(groupForCreationDto.Name);
                groupForCreationDto.Protocol = styler.FormatName(groupForCreationDto.Protocol);

                for (int i = 0; i < groupForCreationDto.Dawgtags.Count; i++)
                {
                    groupForCreationDto.Dawgtags = styler.FormatDawgtagList(groupForCreationDto.Dawgtags);
                }
            }
            return true;
        }


        /// <summary>
        /// Validates the input for the list of dawgtags.
        /// </summary>
        /// <returns><c>true</c>, if the dawgtags were valid, <c>false</c> otherwise.</returns>
        /// <param name="groupForCreationDto">Group for creation dto.</param>
        /// <param name="excepts">Excepts.</param>
        protected bool ValidateInputForUsers(GroupForCreationDto groupForCreationDto, ref List<Exception> excepts)
        {
            using (Validator checker = new Validator())
            {
                //Check if the dawgtags are in the proper format
                foreach (string dawgtag in groupForCreationDto.Dawgtags)
                {
                    checker.ValidateDawgtag(dawgtag, ref excepts);
                }
            }
            return excepts.Count == 0;
        }


        /// <summary>
        /// Initializes the user by checking if they exist and creates a user
        /// if that user does not exist yet.
        /// </summary>
        /// <returns><c>true</c>, if user was initialized, <c>false</c> otherwise.</returns>
        /// <param name="dawgtag">Dawgtag.</param>
        /// <param name="excepts">Excepts.</param>
        protected bool InitializeUser(string dawgtag, ref List<Exception> excepts)
        {
            GuacamoleDatabaseInserter inserter = new GuacamoleDatabaseInserter();
            GuacamoleDatabaseSearcher searcher = new GuacamoleDatabaseSearcher();

            //Check if the user already exists
            if (!searcher.SearchUserName(dawgtag, ref excepts))
            {
                //Add the user if it was not found
                return inserter.InsertUser(dawgtag, ref excepts);
            }
            return true;
        }


        /// <summary>
        /// Adds the user to the user group.
        /// </summary>
        /// <returns><c>true</c>, if user was added to the user group, <c>false</c> otherwise.</returns>
        /// <param name="groupName">Group name.</param>
        /// <param name="dawgtag">Dawgtag.</param>
        /// <param name="excepts">Excepts.</param>
        protected bool AddUserToUserGroup(string groupName, string dawgtag, ref List<Exception> excepts)
        {
            GuacamoleDatabaseInserter inserter = new GuacamoleDatabaseInserter();
            return inserter.InsertUserIntoUserGroup(groupName, dawgtag, ref excepts);
        }



        protected bool CreateConnection(GroupForCreationDto groupForCreationDto, ref List<Exception> excepts)
        {
            Calculator calculator = new Calculator();
            GuacamoleDatabaseInserter inserter = new GuacamoleDatabaseInserter();
            string connectionName;
            using (Formatter styler = new Formatter())
            {
                connectionName = styler.FormatVmName(groupForCreationDto.Name, ref excepts);
                if(connectionName == null)
                {
                    return false;
                }
            }

            if (inserter.InsertConnection(groupForCreationDto.Name, connectionName, calculator.GetNextIp(), 
                groupForCreationDto.Protocol, ref excepts))
            {
                return true;
                //return InitializeConnection(groupForCreationDto, ref excepts);
            }
            return false;
        }


        /// <summary>
        /// Handles getting the error messages formatted.
        /// </summary>
        /// <returns>A string containing the error messages</returns>
        /// <param name="excepts">Exceptions.</param>
        protected String HandleErrors(List<Exception> excepts)
        {
            String exceptionMessage = "";
            foreach (Exception e in excepts)
            {
                Console.Error.Write(e.Message);
                exceptionMessage += e;
            }
            return exceptionMessage;
        }
    }
}
