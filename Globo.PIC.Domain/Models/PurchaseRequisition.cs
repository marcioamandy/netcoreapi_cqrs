
using System.Collections.Generic;

namespace Globo.PIC.Domain.Models
{

    /// <summary>
    /// Contrato de integração com DL para Criação de RC no ERP 
    /// </summary>
    public class PurchaseRequisition
    {

        /// <summary>
        /// Número de referência para um documento no sistema Legado
        /// </summary>
        public string documentSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string interfaceSourceCode { get; set; }

        /// <summary>
        /// idLegado: ID único (header) do sistema legado
        /// </summary>
        public string sourceApplicationCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool activeRequisitionFlag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string additionalInformation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string auxiliaryAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string functionalCurrencyCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public string ContractNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool externallyManagedFlag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string preparerEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string requisitionBusinessUnit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Line> lines { get; set; }
	}
}
