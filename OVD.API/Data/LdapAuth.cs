using System;
using System.Collections.Generic;

using Novell.Directory.Ldap;
using OVD.API.Dtos;

namespace OVD.API.Data
{
    public class LdapAuth
    {
        public bool validateUser(UserForLoginDto userForLoginDto) 
        {
            using (LdapConnection connection = new LdapConnection())
            {
                connection.SecureSocketLayer = true;
                int version = LdapConnection.LdapV3;

                try 
                {
                    connection.Connect("AD.SIU.EDU", 636);
                    var sdn = connection.GetSchemaDn();

                    var username = "AD\\" + userForLoginDto.Username;

                    // Authentification bind
                    connection.Bind(version, username, userForLoginDto.Password);

                    return true;
                }

                catch (LdapException)
                {
                    return false;
                }
            }
        }
    }
}