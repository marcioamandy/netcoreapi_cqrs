
namespace Globo.PIC.Domain.Models
{
    public class CatalogItemAgreementLine
    {

        public string agreement { get; set; }

        public string description { get; set; }

        public string line_status { get; set; }

        public string long_description { get; set; }

        public string currency_code { get; set; }

        public long agreement_line_id { get; set; }

        public long agreement_id { get; set; }

        public string document_status { get; set; }

        public CatalogItemAgreementHeader agreement_header { get; set; }

    }
}
