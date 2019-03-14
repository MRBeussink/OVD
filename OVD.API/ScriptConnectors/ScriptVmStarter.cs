using System;
namespace test_OVD_clientless.ScriptConnectors
{
    public class ScriptVmStarter
    {
        public string startVm(string vmName)
        {
            ScriptExecutor executor = new ScriptExecutor();
            string arguments = "--inputname=\"" + vmName + "\"";
            return executor.executeStartVms(arguments);
        }
    }
}
