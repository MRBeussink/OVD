using System;
using test_OVD_clientless.GuacamoleDatabaseConnectors;
using test_OVD_clientless.Controllers;

namespace test_OVD_clientless
{
    class Program
    {
        static void Main(string[] args)
        {
            NewGroupController controller = new NewGroupController();
            controller.putExample();
        }
    }
}
