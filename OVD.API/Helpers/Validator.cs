using System;
using System.Text.RegularExpressions;
using test_OVD_clientless.DatabaseConnectors;

namespace test_OVD_clientless.Helpers
{
    public class Validator
    {

        /// <summary> 
        /// Validates an if the provided integer is greater than or equal to zero.
        /// This is used for validating user integer input. 
        /// </summary>
        /// <returns><c>true</c>, if input number was validated, <c>false</c> otherwise.</returns>
        /// <param name="number">The integer to validate.</param>
        public bool validateInputNumber(int number) 
        {
            return (number >= 0);
        }


        /// <summary> 
        /// Validates the name of the group ensuring that the provided group
        /// name is not taken and is in the proper format. 
        /// </summary>
        /// <returns><c>true</c>, if group name was found, <c>false</c> otherwise.</returns>
        /// <param name="groupName">The name of the group to validate.</param>
        public bool validateGroupName(string groupName) 
        {
            GuacamoleDatabaseConnector dbc = new GuacamoleDatabaseConnector();
            return dbc.searchGroupName(groupName);
        }


        /// <summary>
        /// Validates and sees if the given user is stored within the Guacamole database.
        /// </summary>
        /// <returns><c>true</c>, if user was a valid existing user, <c>false</c> otherwise.</returns>
        /// <param name="dawgtag">Dawgtag.</param>
        /// <param name="hash">Password hash.</param>
        /// <param name="salt">Password salt.</param>
        public bool validateUser(String dawgtag, String hash, String salt)
        {
            GuacamoleDatabaseConnector dbc = new GuacamoleDatabaseConnector();
            return dbc.searchUser(dawgtag);
        }


        /// <summary>
        /// Ensures that the dawgtag given is in the proper format.
        /// </summary>
        /// <returns><c>true</c>, if dawgtag was valid, <c>false</c> otherwise.</returns>
        /// <param name="dawgtag">Dawgtag.</param>
        public bool validateDawgtag(string dawgtag)
        {
            //Ensure dawg tag is in the proper format
            dawgtag = dawgtag.ToLower();
            Regex regex = new Regex(@"siu85\d{7}\z");
            Match match = regex.Match(dawgtag);

            return match.Success;
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
