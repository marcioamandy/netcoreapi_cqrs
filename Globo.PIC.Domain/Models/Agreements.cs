using System;
using System.Collections.Generic;
using System.Text;

namespace Globo.PIC.Domain.Models
{
    public class Agreements
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string CreationDate { get; set; }
        public string StartDate { get; set; }
        public string EffectiveDate { get; set; }
        public string Status { get; set; }
        public string AgreementType { get; set; }
        public string RevisionNumber { get; set; }
        public decimal Balance { get; set; }
        public string Comments { get; set; }
        public string ContractNumber { get; set; }
        public string SupplierCode { get; set; }
        public List<AgreementItems> AgreementItems { get; set; }
        public List<RequisitionBusinessUnits> RequisitionBusinessUnits { get; set; }
    }
}
