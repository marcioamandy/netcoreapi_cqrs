using Globo.PIC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Domain.Interfaces
{
    public interface IConteudoServiceProxy : IDisposable
    {
        Task<string> GetToken(string user, string password); 
        Task<List<Conteudo>> GetConteudos();
    }
}
