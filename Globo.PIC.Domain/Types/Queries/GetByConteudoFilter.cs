using MediatR;
using System.Collections.Generic;
using System.ComponentModel; 
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.ViewModels;
namespace Globo.PIC.Domain.Types.Queries
{
    public class GetByConteudoFilter :
        IRequest<List<Conteudo>>
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("Filter")]
        public ConteudoFilterViewModel Filter { get; set; }
    }
}
