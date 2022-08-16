using System;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Novell.Directory.Ldap;

namespace Globo.PIC.Infra.ActiveDirectory
{

    /// <summary>
    /// Serviço utilitário do Active Directory
    /// </summary>
    public class ADServiceLDAP : IADServiceLDAP
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

        /// <summary>
        /// Recupera usuário do AD
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public IdentityUser GetUser(string login)
        {

            IdentityUser identityUser = null;

            try
            {
                using (var cn = new LdapConnection())
                {
                    cn.Connect(domainName, 389);

                    cn.Bind(username, password);

                    var query = string.Format(queryFormat, login);

                    ILdapSearchResults lsc = cn.Search(baseQuery, LdapConnection.ScopeSub, query, null, false);

                    while (lsc.HasMore())
                    {
                        LdapEntry nextEntry = null;

                        try
                        {
                            nextEntry = lsc.Next();
                        }
                        catch { continue; }

                        LdapAttributeSet attributeSet = nextEntry.GetAttributeSet();

                        System.Collections.IEnumerator ienum = attributeSet.GetEnumerator();

                        var _user = new Usuario();

                        while (ienum.MoveNext())
                        {
                            LdapAttribute attribute = (LdapAttribute)ienum.Current;

                            if (attribute.Name == "givenName")
                                _user.Name = attribute.StringValue;

                            if (attribute.Name == "sn")
                                _user.LastName = attribute.StringValue;

                            if (attribute.Name == "mail")
                                _user.Email = attribute.StringValue;

                            if (attribute.Name == "displayName")
                                _user.Apelido = attribute.StringValue;

                            if (!(string.IsNullOrWhiteSpace(_user.Name) ||
                                string.IsNullOrWhiteSpace(_user.LastName) ||
                                string.IsNullOrWhiteSpace(_user.Email) ||
                                string.IsNullOrWhiteSpace(_user.Apelido)))
                                break;
                        }

                        identityUser = new IdentityUser(_user, new Authorization());
                    }

                    cn.Disconnect();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            

            return identityUser;

        }
    }
}
