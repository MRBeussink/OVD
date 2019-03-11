using System;
using System.Text.RegularExpressions;
using test_OVD_clientless.GuacamoleDatabaseConnectors;

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
        /// Validates the name of the group by checking if the given name exists.
        /// </summary>
        /// <returns><c>true</c>, if group name was validated, <c>false</c> otherwise.</returns>
        /// <param name="groupName">Group name.</param>
        public bool validateGroupName(string groupName)
        {
            GuacamoleDatabaseSearcher searcher = new GuacamoleDatabaseSearcher();
            return !searcher.searchGroupName(groupName);
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
    }
}
