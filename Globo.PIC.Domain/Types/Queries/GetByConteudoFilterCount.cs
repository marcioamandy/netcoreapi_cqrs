using MediatR; 
using System.ComponentModel;
using Globo.PIC.Domain.ViewModels;
namespace Globo.PIC.Domain.Types.Queries
{
    public class GetByConteudoFilterCount :
        IRequest<int>
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("Filter")]
        public ConteudoFilterViewModel Filter { get; set; }
    }
}
