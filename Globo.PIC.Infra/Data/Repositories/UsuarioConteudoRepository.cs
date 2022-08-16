using Globo.PIC.Domain.Entities;
using Globo.PIC.Infra.Data.Context;

namespace Globo.PIC.Infra.Data.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class UsuarioConteudoRepository : Repository<UsuarioConteudo>
    {        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public UsuarioConteudoRepository(PicDbContext context) : base(context)
        {
        }
    }
}