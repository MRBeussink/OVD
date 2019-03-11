using System;
using System.Text.RegularExpressions;

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
