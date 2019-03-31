using System;
using System.Collections.Generic;
using OVD.API.Dtos;
using OVD.API.Controllers;
using OVD.API.Helpers;

namespace test_OVD_clientless
{
    class Program
    {
        static void Main(string[] args)
        {
            NewGroupController gc = new NewGroupController();
            GroupForEditDto dto = new GroupForEditDto();
            Calculator calc = new Calculator();
            List<Exception> excepts = new List<Exception>();
            IList<string> dawgtags = new List<string>();
            IList<string> removeDawgtags = new List<string>();

            dawgtags.Add("siu853401101");
            dawgtags.Add("siu853401102");

            removeDawgtags.Add("siu853401102");

            dto.Name = "Test_Group_1";
            dto.VMChoice = "peru/ubuntu-18.04-desktop-amd64";
            dto.MinVms = 5;
            dto.MaxVms = 10;
            dto.NumHotspares = 1;
            dto.Protocol = "rdp";
            dto.Dawgtags = dawgtags;
            dto.RemoveDawgtags = removeDawgtags;
            
            //Console.Write(calc.GetNextIp());

            gc.CreateGroup("test", dto);

            //gc.DeleteGroup("test", "test_group_1");

            //gc.EditGroup("test", dto);
        }
    }
}
