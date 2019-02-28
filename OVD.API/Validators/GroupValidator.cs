using System;
using test_OVD_clientless.Script_Executors;
using test_OVD_clientless.Guacamole_Connector;

namespace test_OVD_clientless.Validators
{
    public class Group_Validator
    {

        /// <summary> 
        /// Validates an if the provided integer is greater than or equal to zero.
        /// This is used for validating user integer input. 
        /// </summary>
        /// <returns><c>true</c>, if input number was validated, <c>false</c> otherwise.</returns>
        /// <param name="number">The integer to validate.</param>
        public bool validateInputNumber(int number) 
        {
	        return number >= 0;
	    }


        /// <summary> 
        /// Validates the name of the group ensuring that the provided group
        /// name is not taken and is in the proper format. 
        /// </summary>
        /// <returns><c>true</c>, if group name was validated, <c>false</c> otherwise.</returns>
        /// <param name="groupName">The name of the group to validate.</param>
        public bool validateGroupName(string groupName) 
        {
            GuacamoleDatabaseConnector dbc = new GuacamoleDatabaseConnector();
            return !dbc.searchGroupName(groupName);
        }

        /// <summary>
        /// Validates the name of the given vm.
        /// </summary>
        /// <returns><c>true</c>, if vm name was not taken<c>false</c> otherwise.</returns>
        /// <param name="vmName">Vm name.</param>
        public bool validateVmName(string vmName)
        {
            ScriptExecutor executor = new ScriptExecutor();
            String statusResult = executor.executeVmNameExists(vmName);
            return statusResult == "status";
        }
    }
}
