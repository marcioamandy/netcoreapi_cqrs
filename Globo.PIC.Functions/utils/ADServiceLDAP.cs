using System;
using System.Net;
using System.DirectoryServices;

namespace Globo.PIC.Functions.utils
{

    /// <summary>
    /// Serviço utilitário do Active Directory
    /// </summary>
    public class ADServiceLDAP
    {

        private string domainName { get; } = Environment.GetEnvironmentVariable("AD_DOMAIN");

        private string username { get; } = Environment.GetEnvironmentVariable("AD_USERNAME");

        private string password { get; } = Environment.GetEnvironmentVariable("AD_PASSWORD");

        private readonly string baseQuery = "dc=corp,dc=tvglobo,dc=com,dc=br";

        private readonly string queryFormat = "(&(objectClass=user)(sAMAccountName=*{0}*))";

        /// <summary>
        /// 
        /// </summary>
        public ADServiceLDAP() { }

        public dynamic GetUser(string login)
        {

            try
            {
                using (DirectoryEntry entry = new DirectoryEntry("LDAP://" + domainName + "/"+ baseQuery, username, password, AuthenticationTypes.Secure))
                {
                    DirectorySearcher directorySearcher = new DirectorySearcher(entry);
                    directorySearcher.SearchScope = SearchScope.Subtree;
                    directorySearcher.Filter = string.Format(queryFormat, login);
                    var todos = directorySearcher.FindAll();

                    SearchResult result = directorySearcher.FindOne();
                }
            }
            catch (Exception error) {
            throw new Exception(error.Message);
            }

            return null;
        }

        // public dynamic GetUser(string login)
        // {
        //     //string filter = "(&(&(objectclass=user)(objectcategory=person))sAMAccountName=username)";
        //     string dirs = "dc=corp,dc=tvglobo,dc=com,dc=br";

        //     NetworkCredential credentials = new NetworkCredential(username, password);

        //     LdapDirectoryIdentifier directoryIdentifier = new LdapDirectoryIdentifier(string.Format("LDAP://{0}/{1}", domainName, dirs), 389, false, false);

        //     using (LdapConnection connection = new LdapConnection(directoryIdentifier, credentials, AuthType.Basic))
        //     {

        //         connection.Timeout = new TimeSpan(0, 0, 30);
        //         connection.SessionOptions.ProtocolVersion = 3;
        //         SearchRequest search = new SearchRequest(queryFormat, queryFormat, SearchScope.Base, "mail");
        //         SearchResponse response = connection.SendRequest(search) as SearchResponse;
        //         foreach (SearchResultEntry entry in response.Entries)
        //         {
        //             Console.WriteLine(entry.Attributes["mail"][0]);
        //         }

        //     }


        //     return null; 
        // }
    }
}
