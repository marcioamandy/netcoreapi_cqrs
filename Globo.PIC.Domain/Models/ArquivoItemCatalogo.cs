using System; 

namespace Globo.PIC.Domain.Models
{
    public class ArquivoItemCatalogo
    { 
        public long? Id { get; set; }
         
        public string OriginalName { get; set; }
         
        public string Name { get; set; }
           
        public string Type { get; set; }
         
        public DateTime SentAt { get; set; }
    }
}
