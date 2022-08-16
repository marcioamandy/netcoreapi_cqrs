using System;
using Newtonsoft.Json;
using Novell.Directory.Ldap;

namespace Globo.PIC.Functions.utils
{

    /// <summary>
    /// Serviço utilitário do Active Directory
    /// </summary>
    public class ADServiceNovell
    {

        private string domainName { get; } = Environment.GetEnvironmentVariable("AD_DOMAIN");

        private string username { get; } = Environment.GetEnvironmentVariable("AD_USERNAME");

        private string password { get; } = Environment.GetEnvironmentVariable("AD_PASSWORD");

        private readonly string baseQuery = "dc=corp,dc=tvglobo,dc=com,dc=br";

        private readonly string queryFormat = "(&(objectClass=user)(sAMAccountName=*{0}*))";

        /// <summary>
        /// 
        /// </summary>
        public ADServiceNovell() { }

        /// <summary>
        /// Recupera usuário do AD
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public dynamic GetUser(string login)
        {

            dynamic user = null;

            try
            {
                using (var cn = new LdapConnection())
                {

                    Console.WriteLine($"Conectando ao AD [{domainName}] ...");

                    cn.Connect(domainName, 389);

                    Console.WriteLine($"Binded!");

                    cn.Bind(username, password);

                    Console.WriteLine($"Conectado com sucesso!");

                    var query = string.Format(queryFormat, login);

                    Console.WriteLine($"Executando Query [{query}]");

                    ILdapSearchResults lsc = cn.Search(baseQuery, LdapConnection.ScopeSub, query, null, false);

                    Console.WriteLine($"Query executada com sucesso!");

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

                        string
                            Login = login,
                            Name = string.Empty,
                            LastName = string.Empty,
                            Email = string.Empty,
                            Apelido = string.Empty;

                        while (ienum.MoveNext())
                        {
                            LdapAttribute attribute = (LdapAttribute)ienum.Current;                            

                            if (attribute.Name == "givenName")
                                Name = attribute.StringValue;

                            if (attribute.Name == "sn")
                                LastName = attribute.StringValue;

                            if (attribute.Name == "mail")
                                Email = attribute.StringValue;

                            if (attribute.Name == "displayName")
                                Apelido = attribute.StringValue;

                            if (!(string.IsNullOrWhiteSpace(Name) ||
                                string.IsNullOrWhiteSpace(LastName) ||
                                string.IsNullOrWhiteSpace(Apelido) ||
                                string.IsNullOrWhiteSpace(Email))){
                                    user = new { Login, Name, LastName, Email, Apelido };

                                    Console.WriteLine($"Usuário {Name} encontrado!");
                                    break;                                    
                                }
                        }

                        if(user != null)
                            break;
                    }

                    cn.Disconnect();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Falha na comunicação com AD: ", JsonConvert.SerializeObject(ex));
            }

            return user;
        }
    }
}
