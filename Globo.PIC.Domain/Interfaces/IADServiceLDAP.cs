using Globo.PIC.Domain.Models;
namespace Globo.PIC.Domain.Interfaces
{

    /// <summary>
    /// Serviço utilitário do Active Directory
    /// </summary>
    public interface IADServiceLDAP
    {

        /// <summary>
        /// Recupera usuário do AD
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public IdentityUser GetUser(string login);

    }
}


