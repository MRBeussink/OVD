using System;
namespace test_OVD_clientless.ScriptConnectors
{
    public class ScriptVmCreator
    {
        public string cloneVm(string vmName, string vmChoice)
        {
            ScriptExecutor executor = new ScriptExecutor();
            string arguments = "--start --outputname=\"" + vmName + "\" --inputname=\""
                + vmChoice + "\"";
            return executor.executeCloneVms(arguments);
        }
    }
}
