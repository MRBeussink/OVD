using System;
using System.Collections.Generic;
using test_OVD_clientless.Models;
using test_OVD_clientless.Helpers;

namespace test_OVD_clientless.Controllers
{
    public class NewGroupController
    { 

        public bool putExample()
        {
            //Form Variables REMOVE ONCE CONTROLLER IS USED
            string groupName = "test_group";
            string boxType = "test_box";
            string[] dawgtags = { "siu853401101", "siu82341254" };
            int maxNumVms = 15;
            int minNumVms = 5;
            int hotspares = 5; 

            //Validate the group name
            Validator checker = new Validator();
            if (!checker.validateGroupName(groupName))
            {
                Console.Error.Write("Error: This group name is already taken.\n");
                return false;
            }

            //Validate the maximum number of virtual machines
            if (!checker.validateInputNumber(maxNumVms))
            {
                Console.Error.Write("Error: The given vm max total is invalid.\n");
                return false;
            }

            //Validate the minimum number of virtual machines
            if (!checker.validateInputNumber(maxNumVms))
            {
                Console.Error.Write("Error: The given vm min total is invalid.\n");
                return false;
            }

            //Validate the total number of hotspares
            if (!checker.validateInputNumber(hotspares))
            {
                Console.Error.Write("Error: The given hotspare number is invalid.\n");
                return false;
            }

            Group newGroup = null;
            Config newConfig = null;
            ICollection<User> users = null;
            ICollection<VirtualMachine> vms = null;

            //Initalize the group object
            newGroup = new Group
            {
                name = groupName,
                config = newConfig,
                members = users
            };

            //Initalize the config object
            Config groupConfig = new Config
            {
                group = newGroup,
                maxNum = maxNumVms,
                minNum = minNumVms,
                hotspareNum = hotspares,
                virtualMachines = vms
            };

            //Insert group into Apache Guacamole
            if (!insertGroup(newGroup))
            {
                Console.Error.Write("Error: Could not insert the created group" +
                    "into Apache Guacamole.");
                //Delete from Entity framework
                return false;
            }

            //Initalize the user collection
           //users = initalizeUsers(dawgtags);

            //Initalize the virtual machines
            //vms = initalizeVms(groupName, boxType, minNumVms);




            //Store the corresponding user and group information
            /*for (int i = 0; i < dawgtags.Length - 1; i++)
            {
                User newUser = new User();
                newUser.dawgtag = dawgtags[i];
                newUser.groups.Add(newGroup);
                userCollection.Add(newUser);
            }*/
            return true;
        }


        public bool insertGroup(Group newGroup)
        {
            //Update the Guacamole Database
            GuacamoleDatabaseConnector connector = new GuacamoleDatabaseConnector();
            return connector.insertGroup(newGroup);
        }


        public bool initalizeVm(ICollection<string> vmParameters)
        {
            return false;
        }
    }
}