
namespace Globo.PIC.Domain.Models
{
    public class CatalogItem
    {

        public long item_id { get; set; }

        public string image_url { get; set; }

        public string item_number { get; set; }

        public string description { get; set; }

        public string long_description { get; set; }

        public CatalogItemUOM item_uom_info { get; set; }
    }
}
