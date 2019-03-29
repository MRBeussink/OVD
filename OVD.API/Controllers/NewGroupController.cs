// using System;
// using System.Collections.Generic;
// using OVD.API.Models;
// using OVD.API.Helpers;
// using OVD.API.GuacamoleDatabaseConnectors;
// using OVD.API.ScriptConnectors;
// using OVD.API.Exceptions;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using System.Threading.Tasks;
// using OVD.API.Dtos;

// namespace OVD.API.Controllers
// {
//     // [Authorize]
//     [Route("api/newgroups")]
//     public class NewGroupsController : ControllerBase
//     {

//         // SECTION  HTTP POST and PUT methods



//         [HttpPost("create")] 
//         public async Task<IActionResult> CreateUserGroup(GroupForCreationDto groupForCreationDto)
//         {
//             return Ok();

//             // check that user is admin
//             // if (userId != int.Parse(Admin.FindFirst(ClaimTypes.NameIdentifier).Value))
//                 // return Unauthorized();

//             // objects to be stored in the EF database
//             GroupConfig group;
//             ICollection<VirtualMachine> virtualMachines = new List<VirtualMachine>();
//             List<Exception> exceptions = new List<Exception>();

//             //Reformat the given input strings to ensure that the consistancy of
//             //the databases is maintained

//             String groupName;
//             String vmChoice;
            
//             using (Formatter styler = new Formatter())
//             {
//                 groupName = styler.formatGroupName(groupForCreationDto.Name);
//                 vmChoice = styler.formatName(groupForCreationDto.VMChoice);
//             }

//             using (Validator checker = new Validator())
//             {
//                 //Check if the group arguments are proper
//                 checker.validateGroupName(groupName, ref exceptions);
//                 checker.validateVmChoice(vmChoice, ref exceptions);
//                 checker.validateMin(groupForCreationDto.MinVms, ref exceptions);
//                 checker.validateMax(groupForCreationDto.MaxVms, ref exceptions);
//                 checker.validateMinMax(groupForCreationDto.MinVms, groupForCreationDto.MaxVms, ref exceptions);
//                 checker.validateHotspares(groupForCreationDto.NumHotspares, ref exceptions);

//                 //Check if the dawgtags are proper
//                 foreach (string dawgtag in groupForCreationDto.Dawgtags)
//                 {
//                     checker.validateDawgtag(dawgtag, ref exceptions);
//                 }

//                 if (exceptions.Count != 0)
//                 {
//                     var message = handleErrors(exceptions);
//                     return BadRequest(message);
//                 }
//             }

//             //Initalize the connection group with Guacamole
//             group = initalizeGroup(groupName, groupForCreationDto.MaxVms,
//                 groupForCreationDto.MinVms, groupForCreationDto.NumHotspares,
//                 ref exceptions);
//             if (exceptions.Count != 0)
//             {
//                 var message = handleErrors(exceptions);
//                 return BadRequest(message);
//             }

//             /*//Initalize the virtual machines by calling the required scripts
//             //Only initialize and start the minimum number desired
//             for(int i = 0; i < minVms; i++)
//             {
//                 VirtualMachine vm = initalizeVm(groupName, vmChoice, ref exceptions);
//                 if(vm != null)
//                 {
//                     virtualMachines.Add(vm);
//                 }
//             }
//             if (exceptions.Count != 0)
//             {
//                 handleErrors(exceptions);
//                 return; //REMOVE
//             }*/

//             //Initalize the users if they do not exist
//             //Add the users to the newly created group
//             foreach (string dawgtag in groupForCreationDto.Dawgtags)
//             {
//                 //Add new users into the database
//                 bool isInitialized = initalizeUser(dawgtag, ref exceptions);

//                 //Add the users to the connection group
//                 if (isInitialized)
//                 {
//                     addUserToUserGroup(dawgtag, groupName, ref exceptions);
//                 }
//             }
//             if (exceptions.Count != 0)
//             {
//                 var message = handleErrors(exceptions);
//                 return BadRequest(message);
//             }

//             return Ok();
//             // NOTE this could return CreatedAtRoute if we need data to be returned
//         }
        
//         [HttpPut("addUsers")]
//         public async Task<IActionResult> AddUsersToGroup(String userName, String groupName, IList<String> usersToAdd)
//         {
//             // ini
//             List<Exception> exceptions = new List<Exception>();
//             List<String> dawgtagsToInitialize = new List<String>();

//             GuacamoleDatabaseSearcher searcher = new GuacamoleDatabaseSearcher();
//             GuacamoleDatabaseInserter inserter = new GuacamoleDatabaseInserter();

//             // check that group exists
//             if (!searcher.searchGroupName(groupName, ref exceptions))
//                 return BadRequest("No group with that group name exists");

//             if (exceptions.Count != 0)
//             {
//                 var message = handleErrors(exceptions);
//                 return BadRequest(message);
//             }

//             foreach (String dawgtag in usersToAdd)
//             {
//                 // initialize any users not already in the database
//                 if (!searcher.searchUserName(dawgtag, ref exceptions))
//                 {
//                     if (exceptions.Count != 0) 
//                     {
//                         var message = handleErrors(exceptions);
//                         return BadRequest(message);
//                     }

//                     initalizeUser(dawgtag, ref exceptions);

//                     if (exceptions.Count != 0) 
//                     {
//                         var message = handleErrors(exceptions);
//                         return BadRequest(message);
//                     }
//                 }

//                 // add initialized users into group
//                 inserter.insertUserIntoUserGroup(dawgtag, groupName, ref exceptions);

//                 if (exceptions.Count != 0) 
//                 {
//                     var message = handleErrors(exceptions);
//                     return BadRequest(message);
//                 }
//             }
//             return Ok();
//         }


//         [HttpPut("removeUsers/{groupName}")]
//         public async Task<IActionResult> RemoveUsersToGroup(String userName, String groupName, IList<String> usersToRemove)
//         {
//             return BadRequest("This had not been implemented yet");
//         }

//         // !SECTION 

//         [HttpDelete("delete/{groupName}")]
//         public async Task<IActionResult> DeleteGroup(String userName, String groupName)
//         {
//             return BadRequest("This had not been implemented yet");
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetGroups(String userId) 
//         {
//             // check that user is admin
//             // if (userId != int.Parse(Admin.FindFirst(ClaimTypes.NameIdentifier).Value))
//                 // return Unauthorized();

//             GuacamoleDatabaseSearcher searcher = new GuacamoleDatabaseSearcher();

//             List<Exception> exceptions = new List<Exception>();

//             Queue<String> groupsQueue = searcher.getAllGroupNames(ref exceptions);

//             if (exceptions.Count != 0) 
//             {
//                 var message = handleErrors(exceptions);
//                 return BadRequest(message);
//             }

//             return Ok(groupsQueue.ToArray());
//         }


//         // ANCHOR Private methods

//         /// <summary>
//         /// Initalizes the a new connection group.
//         /// </summary>
//         /// <returns>The newly created group.</returns>
//         /// <param name="groupName">Group name.</param>
//         /// <param name="maxVms">Max vms.</param>
//         /// <param name="minVms">Minimum vms.</param>
//         /// <param name="hotspares">Hotspares.</param>
//         private GroupConfig initalizeGroup(string groupName, int maxVms, int minVms, int hotspares, ref List<Exception> exceptions)
//         {
//             GroupConfig group = new GroupConfig
//             {
//                 groupName = groupName,
//                 maxNum = maxVms,
//                 minNum = minVms,
//                 hotspareNum = hotspares
//             };

//             GuacamoleDatabaseInserter inserter = new GuacamoleDatabaseInserter();
//             if (!inserter.insertConnectionGroup(group, ref exceptions))
//             {
//                 exceptions.Add(new GroupInitalizationException("The provided connection group (" +
//                     groupName + ") could not be created. Please check the status of both the " +
//                     "entity framework database as well as the guacamole mysql database.\n\n"));
//                 return null;
//             }

//             if (!inserter.insertUserGroup(group, ref exceptions))
//             {
//                 exceptions.Add(new GroupInitalizationException("The provided user group (" +
//                     groupName + ") could not be created. Please check the status of both the " +
//                     "entity framework database as well as the guacamole mysql database.\n\n"));
//                 return null;
//             }

//             if (!inserter.insertConnectionGroupIntoUserGroup(group.groupName, ref exceptions))
//             {
//                 exceptions.Add(new GroupInitalizationException("The provided user group (" +
//                     groupName + ") could not be associated with its connection group. Please " +
//                     	"check the status of both the entity framework database as well as the " +
//                     	"guacamole mysql database.\n\n"));
//                 return null;
//             }
//             return group;
//         }


//         /// <summary>
//         /// Initalizes the vm given the type of the vm as well as the name.
//         /// </summary>
//         /// <returns>The new virtual machine object.</returns>
//         /// <param name="groupName">Group name.</param>
//         /// <param name="vmChoice">Vm choice.</param>
//         /// <param name="exceptions">Exceptions.</param>
//         private VirtualMachine initalizeVm(string groupName, string vmChoice, ref List<Exception> exceptions)
//         {
//             GuacamoleDatabaseInserter inserter = new GuacamoleDatabaseInserter();
//             string vmName = string.Empty;

//             //Get next vm from pattern
//             using (Formatter styler = new Formatter())
//             {
//                 styler.formatVmName(groupName, ref exceptions);
//             }

//             //Validate that the vm name is not taken
//             using(Validator checker = new Validator())
//             {
//                 checker.validateVmName(vmName, ref exceptions);
//             }

//             //Create the vm objected that will be stored in entity framework
//             VirtualMachine vm = new VirtualMachine
//             {
//                 vmName = vmName,
//                 baseBox = vmChoice
//             };

//             //Add the new vm to guacamole
//             if (!inserter.insertVm(vmName, vmChoice, ref exceptions))
//             {
//                 exceptions.Add(new VmInitializationException("The provided vm (" +
//                     vmName + ") of the type (" + vmChoice + ") could not be created." +
//                     " Please check the status of both the entity framework database as" +
//                     " well as the guacamole mysql database.\n\n"));
//                 return null;
//             }

//             return vm;
//         }


//         /// <summary>
//         /// Inserts the user into the guacamole database if it does not exist.
//         /// </summary>
//         /// <returns><c>true</c>, if user was added to guacamole, <c>false</c> otherwise.</returns>
//         /// <param name="dawgtag">Dawgtag.</param>
//         private bool initalizeUser(string dawgtag, ref List<Exception> exceptions)
//         {
//             GuacamoleDatabaseInserter inserter = new GuacamoleDatabaseInserter();
//             GuacamoleDatabaseSearcher searcher = new GuacamoleDatabaseSearcher();

//             //keep the dawgtag format consistant in the database
//             using(Formatter styler = new Formatter())
//             {
//                 dawgtag = styler.formatUserName(dawgtag);
//             }

//             //Check if the user already exists
//             if (!searcher.searchUserName(dawgtag, ref exceptions))
//             {
//                 //Add the user if it was not found
//                 if (!inserter.insertUser(dawgtag, ref exceptions))
//                 {
//                     exceptions.Add(new UserInitializationException("The user with the dawgtag (" +
//                         dawgtag + ") could not be added to the system. Please check the status of " +
//                         "both the entity framework database as well as the guacamole mysql database.\n\n"));
//                     return false;
//                 }
//             }
//             return true;
//         }


//         /// <summary>
//         /// Adds the user to Guacamole connection group.
//         /// </summary>
//         /// <returns><c>true</c>, if user was added to the group<c>false</c> otherwise.</returns>
//         /// <param name="dawgtag">Dawgtag.</param>
//         /// <param name="groupName">Group name.</param>
//         public bool addUserToUserGroup(string dawgtag, string groupName, ref List<Exception> exceptions)
//         {
//             GuacamoleDatabaseInserter inserter = new GuacamoleDatabaseInserter();
//             if (!inserter.insertUserIntoUserGroup(dawgtag, groupName, ref exceptions))
//             {
//                 exceptions.Add(new UserInitializationException("The user with the dawgtag (" +
//                         dawgtag + ") could not be added to the user group (" + groupName + "). " +
//                         	"Please check the status of the guacamole mysql database.\n\n"));
//                 return false;
//             }
//             return true;
//         }


// 	/// <summary>
//         /// Removes the connection and user groups associated with the group name.
//         /// </summary>
//         /// <returns><c>true</c>, if group was removed, <c>false</c> otherwise.</returns>
//         /// <param name="groupName">Group name.</param>
//         /// <param name="exceptions">Exceptions.</param>
//         public bool removeGroup(string groupName, ref List<Exception> exceptions)
//         {
//             GuacamoleDatabaseDeleter deleter = new GuacamoleDatabaseDeleter();
//             //Remove the user group from the database
//             if(!deleter.deleteUserGroup(groupName, ref exceptions))
//             {
//                 exceptions.Add(new GroupDeletionException("The user group with the name (" +
//                     groupName + ") could not be removed from the database.\n\n"));
//                 return false;
//             }

//             //Remove the group of connections from the database
//             if (!deleter.deleteConnectionGroup(groupName, ref exceptions))
//             {
//                 exceptions.Add(new GroupDeletionException("The connection group with the name (" +
//                     groupName + ") could not be removed from the database.\n\n"));
//                 return false;
//             }

//             return true;
//         }


//         /// <summary>
//         /// Sends any recieved errors back to the client.
//         /// </summary>
//         /// <param name="exceptions">Exceptions.</param>
//         public String handleErrors(List<Exception> exceptions)
//         {
//             String exceptionMessage = "";

//             foreach (Exception e in exceptions)
//             {
//                 // Console.Error.Write(e.Message);
//                 exceptionMessage += e;
//             }

//             return exceptionMessage;
//         }
//     }
// }
