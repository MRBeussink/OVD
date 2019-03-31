using System;
using System.Collections.Generic;
using OVD.API.Dtos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
// using OVD.API.GuacamoleDatabaseConnectors;
// using GuacamoleDatabaseConnectionFacade.GuacamoleDatabaseConnectors;

using OVD.API.Helpers;

namespace OVD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewGroupController : GroupController
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateGroup(GroupForCreationDto groupForCreationDto)
        {
            //Method Level Variable Declarations
            List<Exception> excepts = new List<Exception>();

            //Format the given input
            if (!FormatInput(groupForCreationDto, ref excepts))
            {
                var message = HandleErrors(excepts);
                return BadRequest(message);
            }

            //Validate group input parameters
            if (!ValidateInputForNewGroup(groupForCreationDto, ref excepts))
            {
                var message = HandleErrors(excepts);
                return BadRequest(message);
            }

            //Validate user input parameters
            if (!ValidateInputForUsers(groupForCreationDto, ref excepts))
            {
                var message = HandleErrors(excepts);
                return BadRequest(message);
            }

            //Create user group
            if (!CreateUserGroup(groupForCreationDto, ref excepts))
            {
                var message = HandleErrors(excepts);
                return BadRequest(message);
            }

            //Create users if they do not exist in the system and add them to the
            //created user group
            foreach(string dawgtag in groupForCreationDto.Dawgtags)
            {
                //Verify a user exists and create them if they do not
                if(InitializeUser(dawgtag, ref excepts))
                {
                    AddUserToUserGroup(groupForCreationDto.Name, dawgtag, ref excepts);
                }
            }

            //Create Connection Group
            if (!CreateConnectionGroup(groupForCreationDto, ref excepts))
            {
                var message = HandleErrors(excepts);
                return BadRequest(message);
            }

            //Connect the user group to the connection group
            if(!AddConnectionGroupToUserGroup(groupForCreationDto.Name, ref excepts))
            {
                var message = HandleErrors(excepts);
                return BadRequest(message);
            }

            //Create the minimum desired Connections
            //for(int i = 0; i < groupForCreationDto.MaxVms; i++)
            //{
                CreateConnection(groupForCreationDto, ref excepts);
            //}
            if (excepts.Count != 0)
            {
                var message = HandleErrors(excepts);
                return BadRequest(message);
            }
            return Ok();
        }


        /// <summary>
        /// Validates the user input for group parameters.
        /// </summary>
        /// <returns><c>true</c>, if input parameters for the group is valid, <c>false</c> otherwise.</returns>
        /// <param name="groupForCreationDto">Group for creation dto.</param>
        /// <param name="excepts">Excepts.</param>
        private bool ValidateInputForNewGroup(GroupForCreationDto groupForCreationDto, ref List<Exception> excepts)
        {
            using (Validator checker = new Validator())
            {
                //Check if the group inupt parameters are valid
                checker.ValidateNewGroupName(groupForCreationDto.Name, ref excepts);
                checker.ValidateConnectionType(groupForCreationDto.VMChoice, ref excepts);
                checker.ValidateMin(groupForCreationDto.MinVms, ref excepts);
                checker.ValidateMax(groupForCreationDto.MaxVms, ref excepts);
                checker.ValidateCpu(groupForCreationDto.Cpu, ref excepts);
                checker.ValidateMemory(groupForCreationDto.Ram, ref excepts);
                checker.ValidateMinMax(groupForCreationDto.MinVms, groupForCreationDto.MaxVms, ref excepts);
                checker.ValidateHotspares(groupForCreationDto.NumHotspares, ref excepts);
            }
            return excepts.Count == 0;
        }


        /// <summary>
        /// Creates the new user group.
        /// </summary>
        /// <returns><c>true</c>, if user group was created, <c>false</c> otherwise.</returns>
        /// <param name="groupForCreationDto">Group for creation dto.</param>
        /// <param name="excepts">Excepts.</param>
        private bool CreateUserGroup(GroupForCreationDto groupForCreationDto, ref List<Exception> excepts)
        {
            GuacamoleDatabaseConnectionFacade.GuacamoleDatabaseConnectors.GuacamoleDatabaseInserter inserter = new GuacamoleDatabaseConnectionFacade.GuacamoleDatabaseConnectors.GuacamoleDatabaseInserter();
            return inserter.InsertUserGroup(groupForCreationDto.Name, ref excepts);
        }


        /// <summary>
        /// Creates the new connection group.
        /// </summary>
        /// <returns><c>true</c>, if the connection group was created, <c>false</c> otherwise.</returns>
        /// <param name="groupForCreationDto">Group for creation dto.</param>
        /// <param name="excepts">Excepts.</param>
        private bool CreateConnectionGroup(GroupForCreationDto groupForCreationDto, ref List<Exception> excepts)
        {
            GuacamoleDatabaseConnectionFacade.GuacamoleDatabaseConnectors.GuacamoleDatabaseInserter inserter = new GuacamoleDatabaseConnectionFacade.GuacamoleDatabaseConnectors.GuacamoleDatabaseInserter();
            return inserter.InsertConnectionGroup(groupForCreationDto.Name, groupForCreationDto.MaxVms, ref excepts);
        }


        /// <summary>
        /// Adds the connection group to user group.
        /// </summary>
        /// <returns><c>true</c>, if connection group to user group was added, <c>false</c> otherwise.</returns>
        /// <param name="groupName">Group name.</param>
        /// <param name="excepts">Excepts.</param>
        private bool AddConnectionGroupToUserGroup(string groupName, ref List<Exception> excepts)
        {
            GuacamoleDatabaseConnectionFacade.GuacamoleDatabaseConnectors.GuacamoleDatabaseInserter inserter = new GuacamoleDatabaseConnectionFacade.GuacamoleDatabaseConnectors.GuacamoleDatabaseInserter();
            return inserter.InsertConnectionGroupIntoUserGroup(groupName, ref excepts);
        }
    }
}