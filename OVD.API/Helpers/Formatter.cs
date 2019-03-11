using System;
using test_OVD_clientless.GuacamoleDatabaseConnectors;

namespace test_OVD_clientless.Helpers
{
    public class Formatter
    {

        /// <summary>
        /// Formats the name of the vm ensuring it is not taken.
        /// </summary>
        /// <returns>The vm name.</returns>
        /// <param name="vmName">The desired vm name based off of the group name.</param>
        public string formatVmName(String vmName)
        {
            GuacamoleDatabaseSearcher searcher = new GuacamoleDatabaseSearcher();
            int vmId = searcher.getVmId() + 1;
            return formatName(vmName + vmId);
        }


        /// <summary>
        /// Formats the name of the group.
        /// </summary>
        /// <returns>The group name.</returns>
        /// <param name="groupName">The newly formatted group name.</param>
        public string formatGroupName(string groupName)
        {
            return formatName(groupName);
        }


        /// <summary>
        /// General formatting pattern for renaming text.
        /// </summary>
        /// <returns>The newly reformatted name.</returns>
        /// <param name="name">The input name.</param>
        public string formatName(string name)
        {
            //Format the name to be similar to the following...
            //EX. cs306_linux_unix_virtual_machine
            name = name.ToLower();
            name = name.Replace(' ', '_');
            name = name.Replace('-', '_');
            return name;
        }


        /// <summary>
        /// Formats the given dawgtag.
        /// </summary>
        /// <returns>The newly reformatted dawgtag.</returns>
        /// <param name="dawgtag">Dawgtag.</param>
        public string formatUserName(string dawgtag)
        {
            return dawgtag.ToLower();
        }
    }
}
