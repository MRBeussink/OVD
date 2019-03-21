using System;
using System.Collections.Generic;
using test_OVD_clientless.Models;
using test_OVD_clientless.Helpers;
using test_OVD_clientless.GuacamoleDatabaseConnectors;
using test_OVD_clientless.ScriptConnectors;
using test_OVD_clientless.Exceptions;

namespace test_OVD_clientless.Controllers
{
    public class NewGroupController
    {

        public void putExample()
        {

            /*******************REMOVE FROM IMPLEMENTATION***********************/
            string groupName = "Test Group 2";
            string vmChoice = "test_ubuntu";
            int maxVms = 10;
            int minVms = 5;
            int hotspares = 2;

            ICollection<string> dawgtags = new List<string>();
            dawgtags.Add("siu853401103");
            /********************************************************************/

            //Objects to be stored into the entity framework database
            GroupConfig group;
            ICollection<VirtualMachine> virtualMachines = new List<VirtualMachine>();
            List<Exception> exceptions = new List<Exception>(); ;

            //Reformat the given input strings to ensure that the consistancy of
            //the databases is maintained
            using (Formatter styler = new Formatter())
            {
                groupName = styler.formatGroupName(groupName);
                vmChoice = styler.formatName(vmChoice);
            }

            //Validate the user input provided
            using (Validator checker = new Validator())
            {
                //Check if the group arguments are proper
                checker.validateGroupName(groupName, ref exceptions);
                checker.validateVmChoice(vmChoice, ref exceptions);
                checker.validateMin(minVms, ref exceptions);
                checker.validateMax(maxVms, ref exceptions);
                checker.validateMinMax(minVms, maxVms, ref exceptions);
                checker.validateHotspares(hotspares, ref exceptions);

                //Check if the dawgtags are proper
                foreach (string dawgtag in dawgtags)
                {
                    checker.validateDawgtag(dawgtag, ref exceptions);
                }

                if (exceptions.Count != 0)
                {
                    handleErrors(exceptions);
                    return; //REMOVE
                }
            }

            //Initalize the connection group with Guacamole
            group = initalizeGroup(groupName, maxVms, minVms, hotspares, ref exceptions);
            if (exceptions.Count != 0)
            {
                handleErrors(exceptions);
                return; //REMOVE
            }

            /*//Initalize the virtual machines by calling the required scripts
            //Only initialize and start the minimum number desired
            for(int i = 0; i < minVms; i++)
            {
                VirtualMachine vm = initalizeVm(groupName, vmChoice, ref exceptions);
                if(vm != null)
                {
                    virtualMachines.Add(vm);
                }
            }
            if (exceptions.Count != 0)
            {
                handleErrors(exceptions);
                return; //REMOVE
            }*/

            //Initalize the users if they do not exist
            //Add the users to the newly created group
            foreach (string dawgtag in dawgtags)
            {
                //Add new users into the database
                bool isInitialized = initalizeUser(dawgtag, ref exceptions);

                //Add the users to the connection group
                if (isInitialized)
                {
                    addUserToUserGroup(dawgtag, groupName, ref exceptions);
                }
            }
            if (exceptions.Count != 0)
            {
                handleErrors(exceptions);
                return; //REMOVE
            }
        }


        /// <summary>
        /// Initalizes the a new connection group.
        /// </summary>
        /// <returns>The newly created group.</returns>
        /// <param name="groupName">Group name.</param>
        /// <param name="maxVms">Max vms.</param>
        /// <param name="minVms">Minimum vms.</param>
        /// <param name="hotspares">Hotspares.</param>
        public GroupConfig initalizeGroup(string groupName, int maxVms, int minVms, int hotspares, ref List<Exception> exceptions)
        {
            GroupConfig group = new GroupConfig
            {
                groupName = groupName,
                maxNum = maxVms,
                minNum = minVms,
                hotspareNum = hotspares
            };

            GuacamoleDatabaseInserter inserter = new GuacamoleDatabaseInserter();
            if (!inserter.insertConnectionGroup(group, ref exceptions))
            {
                exceptions.Add(new GroupInitalizationException("The provided connection group (" +
                    groupName + ") could not be created. Please check the status of both the " +
                    "entity framework database as well as the guacamole mysql database.\n\n"));
                return null;
            }

            if (!inserter.insertUserGroup(group, ref exceptions))
            {
                exceptions.Add(new GroupInitalizationException("The provided user group (" +
                    groupName + ") could not be created. Please check the status of both the " +
                    "entity framework database as well as the guacamole mysql database.\n\n"));
                return null;
            }

            if (!inserter.insertConnectionGroupIntoUserGroup(group.groupName, ref exceptions))
            {
                exceptions.Add(new GroupInitalizationException("The provided user group (" +
                    groupName + ") could not be associated with its connection group. Please " +
                    	"check the status of both the entity framework database as well as the " +
                    	"guacamole mysql database.\n\n"));
                return null;
            }
            return group;
        }


        /// <summary>
        /// Initalizes the vm given the type of the vm as well as the name.
        /// </summary>
        /// <returns>The new virtual machine object.</returns>
        /// <param name="groupName">Group name.</param>
        /// <param name="vmChoice">Vm choice.</param>
        /// <param name="exceptions">Exceptions.</param>
        public VirtualMachine initalizeVm(string groupName, string vmChoice, ref List<Exception> exceptions)
        {
            ScriptVmCreator creator = new ScriptVmCreator();
            ScriptVmStarter starter = new ScriptVmStarter();
            GuacamoleDatabaseInserter inserter = new GuacamoleDatabaseInserter();
            string vmName = string.Empty;

            //Get next vm from pattern
            using (Formatter styler = new Formatter())
            {
                styler.formatVmName(groupName, ref exceptions);
            }

            //Validate that the vm name is not taken
            using(Validator checker = new Validator())
            {
                checker.validateVmName(vmName, ref exceptions);
            }

            //Create the vm objected that will be stored in entity framework
            VirtualMachine vm = new VirtualMachine
            {
                vmName = vmName,
                baseBox = vmChoice
            };

            //Create the new virtual machine and start it
            creator.cloneVm(vmName, vmChoice);
            starter.startVm(vmName);

            //Add the new vm to guacamole
            if (!inserter.insertVm(vmName, vmChoice, ref exceptions))
            {
                exceptions.Add(new VmInitializationException("The provided vm (" +
                    vmName + ") of the type (" + vmChoice + ") could not be created." +
                    " Please check the status of both the entity framework database as" +
                    " well as the guacamole mysql database.\n\n"));
                return null;
            }

            return vm;
        }


        /// <summary>
        /// Inserts the user into the guacamole database if it does not exist.
        /// </summary>
        /// <returns><c>true</c>, if user was added to guacamole, <c>false</c> otherwise.</returns>
        /// <param name="dawgtag">Dawgtag.</param>
        public bool initalizeUser(string dawgtag, ref List<Exception> exceptions)
        {
            GuacamoleDatabaseInserter inserter = new GuacamoleDatabaseInserter();
            GuacamoleDatabaseSearcher searcher = new GuacamoleDatabaseSearcher();

            //keep the dawgtag format consistant in the database
            using(Formatter styler = new Formatter())
            {
                dawgtag = styler.formatUserName(dawgtag);
            }

            //Check if the user already exists
            if (!searcher.searchUserName(dawgtag, ref exceptions))
            {
                //Add the user if it was not found
                if (!inserter.insertUser(dawgtag, ref exceptions))
                {
                    exceptions.Add(new UserInitializationException("The user with the dawgtag (" +
                        dawgtag + ") could not be added to the system. Please check the status of " +
                        "both the entity framework database as well as the guacamole mysql database.\n\n"));
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Adds the user to Guacamole connection group.
        /// </summary>
        /// <returns><c>true</c>, if user was added to the group<c>false</c> otherwise.</returns>
        /// <param name="dawgtag">Dawgtag.</param>
        /// <param name="groupName">Group name.</param>
        public bool addUserToUserGroup(string dawgtag, string groupName, ref List<Exception> exceptions)
        {
            GuacamoleDatabaseInserter inserter = new GuacamoleDatabaseInserter();
            if (!inserter.insertUserIntoUserGroup(dawgtag, groupName, ref exceptions))
            {
                exceptions.Add(new UserInitializationException("The user with the dawgtag (" +
                        dawgtag + ") could not be added to the user group (" + groupName + "). " +
                        	"Please check the status of the guacamole mysql database.\n\n"));
                return false;
            }
            return true;
        }


        /// <summary>
        /// Sends any recieved errors back to the client.
        /// </summary>
        /// <param name="exceptions">Exceptions.</param>
        public void handleErrors(List<Exception> exceptions)
        {
            foreach (Exception e in exceptions)
            {
                Console.Error.Write(e.Message);
            }
            //SEND BACK ERROR MESSAGES
        }
    }
}