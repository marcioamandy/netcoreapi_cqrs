using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Models;
namespace Globo.PIC.Domain.ViewModels
{

    /// <summary>
    /// 
    /// </summary>
    public class ProjetoViewModel
    {

        /// <summary>
        /// 
        /// </summary> 
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string Number { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UnidadeNegocio UnidadeNegocio { get; set; }
    }
}
