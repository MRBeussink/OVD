﻿using System;
using System.Collections.Generic;
using test_OVD_clientless.GuacamoleDatabaseConnectors;

namespace test_OVD_clientless.Helpers
{
    public class Formatter : IDisposable
    {
        private bool isDisposed = false;

        /// <summary>
        /// Releases all resource used by the <see cref="T:test_OVD_clientless.Helpers.Formatter"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the
        /// <see cref="T:test_OVD_clientless.Helpers.Formatter"/>. The <see cref="Dispose"/> method leaves the
        /// <see cref="T:test_OVD_clientless.Helpers.Formatter"/> in an unusable state. After calling
        /// <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:test_OVD_clientless.Helpers.Formatter"/> so the garbage collector can reclaim the memory that
        /// the <see cref="T:test_OVD_clientless.Helpers.Formatter"/> was occupying.</remarks>
        public void Dispose()
        {
            ReleaseResources(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Releases the managed and unmanaged resources.
        /// </summary>
        /// <param name="isFromDispose">If set to <c>true</c> is from dispose.</param>
        protected void ReleaseResources(bool isFromDispose)
        {
            if (!isDisposed)
            {
                if (isFromDispose)
                {
                    // TODO: Release managed resources here
                }
                //TODO: Release unmanaged resources here
            }
            isDisposed = true;
        }


        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="T:test_OVD_clientless.Helpers.Formatter"/> is reclaimed by garbage collection.
        /// </summary>
        ~Formatter()
        {
            ReleaseResources(false);
        }


        /*******************************************************************************
         *------------------------Primary Formatter Methods----------------------------*
         ******************************************************************************/
        /// <summary>
        /// Formats the name of the vm ensuring it is not taken.
        /// </summary>
        /// <returns>The vm name.</returns>
        /// <param name="vmName">The desired vm name based off of the group name.</param>
        public string formatVmName(String vmName, ref List<Exception> exceptions)
        {
            GuacamoleDatabaseSearcher searcher = new GuacamoleDatabaseSearcher();
            int vmId = searcher.getMaxVmId(ref exceptions) + 1;
            return formatName(vmName + "_" + vmId);
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
