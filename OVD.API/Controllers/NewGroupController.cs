using System;
using System.Collections.Generic;
using test_OVD_clientless.Models;
using test_OVD_clientless.Helpers;
using test_OVD_clientless.GuacamoleDatabaseConnectors;

namespace test_OVD_clientless.Controllers
{
    public class NewGroupController
    { 

        public bool putExample(string groupName, int maxVms, int minVms, int hotspares)
        {
            Validator checker = new Validator();
            Formatter styler = new Formatter();
            GroupConfig group;
            ICollection<VirtualMachine> virtualMachines = new List<VirtualMachine>();

            //Fomat the group name given
            groupName = styler.formatGroupName(groupName);

            //Validate the group name
            if (!checker.validateGroupName(groupName))
            {
                Console.Error.Write("Error: This group name is already taken.\n");
                return false;
            }

            //Validate the maximum number of virtual machines
            if (!checker.validateInputNumber(maxVms))
            {
                Console.Error.Write("Error: The given vm max total is invalid.\n");
                return false;
            }

            //Validate the minimum number of virtual machines
            if (!checker.validateInputNumber(maxVms))
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

            //Initalize the connection group with Guacamole
            group = initalizeGroup(groupName, maxVms, minVms, hotspares);
            if (group == null)
            {
                Console.Error.Write("Error: The group could not be created.\n");
                return false;
            }

            /*//Initalize the virtual machines by calling the required scripts
            for(int i = 0; i < minVms; i++)
            {
                VirtualMachine vm = initalizeVm(groupName, vmChoice);
                if (vm == null)
                {
                    Console.Error.Write("Error: Could not initalize the virtual machine.\n");
                    return false;
                }
                else
                {
                    virtualMachines.Add(vm);
                }
            }*/
            return true;
        }


        public GroupConfig initalizeGroup(string groupName, int maxVms, int minVms, int hotspares)
        {
            GroupConfig group = new GroupConfig
            {
                groupName = groupName,
                maxNum = maxVms,
                minNum = minVms,
                hotspareNum = hotspares
            };

            GuacamoleDatabaseInserter inserter = new GuacamoleDatabaseInserter();
            if (!inserter.insertGroup(group)) 
            {
                Console.Error.Write("Error: Could not insert the new group into " +
                	"the Guacamole database.\n");
                return null;
            }
            return group;
        }


        public VirtualMachine initalizeVm(string groupName, string vmChoice)
        {
            return null;
        }
    }
}