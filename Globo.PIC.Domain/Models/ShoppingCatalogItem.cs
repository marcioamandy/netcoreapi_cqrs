
namespace Globo.PIC.Domain.Models
{

    public class ShoppingCatalogItem
    {

        public decimal combined_price { get; set; }

        public long document_id { get; set; }

        public string document_type { get; set; }

        public CatalogItem item { get; set; }

        public CatalogItemAgreementBU[] agreement_bu_access { get; set; }

        public CatalogItemCategory category { get; set; }

        public CatalogItemAgreementLine agreement_line { get; set; }

    }
}
