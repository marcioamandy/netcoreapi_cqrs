using Globo.PIC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Globo.PIC.Domain.Models
{
    public class PedidoItemCompraDocumentosAnexosModel : PedidoItemArteCompraDocumentoAnexo
    {

        /// <summary>
        /// 
        /// </summary>
        public string CaminhoArquivo { get; set; }
    }
}
