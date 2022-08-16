using System;
using System.Collections.Generic;
using System.Text;

namespace Globo.PIC.Domain.Models
{
    public class AgreementItems
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public string Description { get; set; }
        public int LineNumber { get; set; }
        public string ExpirationDate { get; set; }
        public decimal Value { get; set; }
    }
}
