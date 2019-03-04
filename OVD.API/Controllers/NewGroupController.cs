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

            try
            {
                Group newGroup = new Group();
                Config newConfig = new Config();
                ICollection<User> userCollection = new List<User>();
                ICollection<VirtualMachine> vmCollection = new List<VirtualMachine>();

                //Initalize the Group Object
                newGroup.name = groupName;
                newGroup.config = newConfig;
                newGroup.members = userCollection;

                //Initalize the Configuration Object
                newConfig.group = newGroup;
                newConfig.hotspareNum = hotspares;
                newConfig.maxNum = maxNumVms;
                newConfig.minNum = minNumVms;

                //Store the corresponding user and group information
                /*for (int i = 0; i < dawgtags.Length - 1; i++)
                {
                    User newUser = new User();
                    newUser.dawgtag = dawgtags[i];
                    newUser.groups.Add(newGroup);
                    userCollection.Add(newUser);
                }*/

                //Initalize the Guacamole connection group
                if (!initalizeConnectionGroup(newGroup))
                {
                    Console.Error.Write("Error: Could not configure the Guacamole " +
                        "connection group.\n");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.Error.Write(e);
                return false;
            }
            //Initalize the VirtualBox connection virtual machines
            //initalizeVm()
            return true;
        }


        public bool initalizeConnectionGroup(Group newGroup)
        {
            GuacamoleDatabaseConnector connector = new GuacamoleDatabaseConnector();
            return connector.insertGroup(newGroup);
        }


        public bool initalizeVm(ICollection<string> vmParameters)
        {
            return false;
        }
    }
}