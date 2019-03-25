using System;
using System.IO;
using System.Text.RegularExpressions;
using OVD.API.GuacamoleDatabaseConnectors;
using OVD.API.Exceptions;
using System.Collections.Generic;

namespace OVD.API.Helpers
{
    public class Validator : IDisposable
    {

        private const string VM_STORAGE_DIRECTORY = "/home/amcowden97/VirtualBox VMs/";
        private bool isDisposed = false;


        /// <summary>
        /// Releases all resource used by the <see cref="T:test_OVD_clientless.Helpers.Validator"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the
        /// <see cref="T:test_OVD_clientless.Helpers.Validator"/>. The <see cref="Dispose"/> method leaves the
        /// <see cref="T:test_OVD_clientless.Helpers.Validator"/> in an unusable state. After calling
        /// <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:test_OVD_clientless.Helpers.Validator"/> so the garbage collector can reclaim the memory that
        /// the <see cref="T:test_OVD_clientless.Helpers.Validator"/> was occupying.</remarks>
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
        /// <see cref="T:test_OVD_clientless.Helpers.Validator"/> is reclaimed by garbage collection.
        /// </summary>
        ~Validator()
        {
            ReleaseResources(false);
        }


        /*******************************************************************************
         *------------------------Primary Validator Methods----------------------------*
         ******************************************************************************/
        /// <summary> 
        /// Checks if the provided integer is greater than or equal to zero.
        /// This is used for validating user integer input. 
        /// </summary>
        /// <returns><c>true</c>, if input number was validated, <c>false</c> otherwise.</returns>
        /// <param name="number">The integer to validate.</param>
        public bool checkPositiveInputNumber(int number) 
        {
            return (number >= 0);
        }


        /// <summary>
        /// Validates the minimum vm number provided.
        /// </summary>
        /// <param name="min">Minimum.</param>
        public bool validateMin(int min, ref List<Exception> exceptions)
        {
            if (!checkPositiveInputNumber(min))
            {
                exceptions.Add(new InvalidUserArgumentException("The minimum vm number has" +
                    " an invalid input of (" + min + "). Please enter an integer that" +
                	" is greater than or equal to zero.\n\n"));
                return false;
            }
            return true;
        }


        /// <summary>
        /// Validates the maximum vm number provided.
        /// </summary>
        /// <param name="max">Maximum.</param>
        public bool validateMax(int max, ref List<Exception> exceptions)
        {
            if (!checkPositiveInputNumber(max))
            {
                exceptions.Add(new InvalidUserArgumentException("The maximum vm number has" +
                    " an invlaid input of (" + max + "). Please enter an integer that" +
                    " is greater than or equal to zero.\n\n"));
                return false;
            }
            return true;
        }


        /// <summary>
        /// Validates that the minimum vm value is less than the maximum.
        /// </summary>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Max.</param>
        public bool validateMinMax(int min, int max, ref List<Exception> exceptions)
        {
            if (min > max)
            {
                exceptions.Add(new InvalidUserArgumentException("The minimum vm number given (" +
                    min + ") is invalid as it is larger than the maximum vm number (" +
                    max + "). Please enter a minimum vm number smaller or equal to the maximum number.\n\n"));
                return false;
            }
            return true;
        }


        /// <summary>
        /// Validates the hotspare number provided.
        /// </summary>
        /// <param name="hotspareNumber">Hotspare number.</param>
        public bool validateHotspares(int hotspareNumber, ref List<Exception> exceptions)
        {
            if (!checkPositiveInputNumber(hotspareNumber))
            {
                exceptions.Add(new InvalidUserArgumentException("The number of hotspares" +
                    " that was provided (" + hotspareNumber + ") must be greater than" +
                    " or equal to zero.\n\n"));
                return false;
            }
            return true;
        }


        /// <summary>
        /// Validates the name of the group by checking if the given name exists.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        public bool validateGroupName(string groupName, ref List<Exception> exceptions)
        {
            GuacamoleDatabaseSearcher searcher = new GuacamoleDatabaseSearcher();
            if (searcher.searchGroupName(groupName, ref exceptions))
            {
                exceptions.Add(new InvalidUserArgumentException("The provided group name (" +
                    groupName + ") is already being used. Please select another group name" + 
                    " or remove the existing group.\n\n" ));
                return false;
            }
            return true;
        }


        /// <summary>
        /// Validates the name of the vm by checking if the given name exists.
        /// </summary>
        /// <returns><c>true</c>, if vm name was validated, <c>false</c> otherwise.</returns>
        /// <param name="vmName">Vm name.</param>
        /// <param name="exceptions">Exceptions.</param>
        public bool validateVmName(string vmName, ref List<Exception> exceptions)
        {
            GuacamoleDatabaseSearcher searcher = new GuacamoleDatabaseSearcher();
            if (searcher.searchVmName(vmName, ref exceptions))
            {
                exceptions.Add(new VmInitializationException("The provided vm name (" +
                    vmName + ") is already being used. Please select another vm name.\n\n"));
                return false;
            }
            return true;
        }


        /// <summary>
        /// Validates if vm choice exists.
        /// </summary>
        /// <param name="vmChoice">Vm choice.</param>
        public bool validateVmChoice(string vmChoice, ref List<Exception> exceptions)
        {
            if (!Directory.Exists(VM_STORAGE_DIRECTORY + vmChoice))
            {
                exceptions.Add(new InvalidUserArgumentException("The provided virtual machine" +
                    " choice (" + vmChoice + ") is not a valid option. Please choose another" +
                    " virtual machine choice or manually add the desired virtual machine to the" +
                    " boxes table in the Entity Framework database.\n\n"));
                return false;
            }
            return true;
        }


        /// <summary>
        /// Ensures that the dawgtag given is in the proper format.
        /// </summary>
        /// <param name="dawgtag">Dawgtag.</param>
        public bool validateDawgtag(string dawgtag, ref List<Exception> exceptions)
        {
            //Ensure dawg tag is in the proper format
            Regex regex = new Regex(@"siu85\d{7}\z", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!regex.Match(dawgtag).Success)
            {
                exceptions.Add(new InvalidUserArgumentException("The provided dawgtag " + dawgtag +
                    " is not a valid dawgtag entry. Please provide a username with the following" +
                    " format: siu85XXXXXXX Ignored Case.\n\n"));
                return false;
            }
            return true;
        }
    }
}
