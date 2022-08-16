using MediatR;
using System.ComponentModel;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Models;

namespace Globo.PIC.Domain.Types.Queries
{
    public class GetByIdAcionamentoItem :
        IRequest<AcionamentoItem>
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("id do filtro para pesquisa de acionamento")]
        public long Id { get; set; }
    }
}
