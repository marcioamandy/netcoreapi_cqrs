using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class AgreementsViewModel
    {
        [Description("Id")]
        public long Id { get; set; }
        [Description("Numero")]
        public string Number { get; set; }
        [Description("Data de Criação")]
        public string CreationDate { get; set; }
        [Description("Data de Inicio")]
        public string StartDate { get; set; }
        [Description("Datat de Efetivação")]
        public string EffectiveDate { get; set; }
        [Description("Status")]
        public string Status { get; set; }
        [Description("Tipo de Acordo")]
        public string AgreementType { get; set; }
        [Description("Numero de Revisão")]
        public string RevisionNumber { get; set; }
        [Description("Balanço")]
        public decimal Balance { get; set; }
        [Description("Comentários")]
        public string Comments { get; set; }
        [Description("Número do Contrato")]
        public string ContractNumber { get; set; }
        [Description("Código ou CNPJ do Fornecedor")]
        public string SupplierCode { get; set; }
        public List<AgreementItemsViewModel> AgreementItems { get; set; }
        public List<UnidadeNegocioViewModel> RequisitionBusinessUnits { get; set; }
    }
}
