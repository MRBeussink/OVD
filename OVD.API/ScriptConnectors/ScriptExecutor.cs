using System;
using System.Diagnostics;

namespace OVD.API.ScriptConnectors
{
    public class ScriptExecutor
    {

        private const string START_VMS_SCRIPT_LOCATION = "./Scripts/start_virtual_box_vm.sh";
        private const string STOP_VMS_SCRIPT_LOCATION = "./Scripts/stop_virtual_box_vm.sh";
        private const string CLONE_VMS_SCRIPT_LOCATION = "./Scripts/clone_virtual_box_vm.sh";


        /// <summary>
        /// Executes the clone vms script that makes a copy based off of a given image.
        /// </summary>
        /// <returns>The clone vms.</returns>
        /// <param name="argumentString">Argument string.</param>
        public string executeCloneVms(string argumentString)
        {
            return executeScript(CLONE_VMS_SCRIPT_LOCATION, argumentString);
        }


        /// <summary>
        /// Executes the script that will start a specified virtual machine.
        /// </summary>
        /// <returns>Status information on the started vm.</returns>
        /// <param name="argumentString">A string containing the dash seperated arguments.</param>
        public string executeStartVms(String argumentString)
        {
            return executeScript(START_VMS_SCRIPT_LOCATION, argumentString);
        }


        /// <summary>
        /// Executes the script that will stop a specified virtual machine.
        /// </summary>
        /// <returns>Status information on the stopped vm.</returns>
        /// <param name="argumentString">A string containing the dash seperated arguments.</param>
        public string executeStopVms(String argumentString)
        {
            return executeScript(STOP_VMS_SCRIPT_LOCATION, argumentString);
        }


        /// <summary>
        /// Executes an arbitrary script based off of the script location and any
        /// desired arguments. A new process is created for every new script and is
        /// collected when execution is complete.
        /// </summary>
        /// <returns>Status information from the standard output/error stream.</returns>
        /// <param name="scriptName">Script name or Path.</param>
        /// <param name="argumentString">
        /// A string containing the dash seperated arguments.
        /// Pass the argument string as null if not arguments are required.
        /// </param>
        private string executeScript(string scriptName,string argumentString)
        {
            try
            {
                string standardErrorOutput = "Success";

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = scriptName;
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;

                if (argumentString != null)
                {
                    psi.Arguments = argumentString;
                }

                Process p = Process.Start(psi);
                standardErrorOutput = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                return standardErrorOutput;

            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }
}
