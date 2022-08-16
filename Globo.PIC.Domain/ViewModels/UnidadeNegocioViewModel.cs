using System.ComponentModel;

namespace Globo.PIC.Domain.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class UnidadeNegocioViewModel 
    {

        /// <summary>
        /// 
        /// </summary>
        [Description("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Codigo")]
        public string Codigo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Name")]
        public string Nome { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("CodigoOrganizacaoInventario")]
        public string CodigoOrganizacaoInventario { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [Description("UF")]
        public string Uf { get; set; }
    }
}
