using System;
using System.Collections.Generic;
using System.Text;

namespace Globo.PIC.Domain.Models
{
    public class Arquivo
    { 
        public long? Id { get; set; }
         
        public string OriginalName { get; set; }
         
        public string Name { get; set; }
         
        public string Url { get; set; }
         
        public string Type { get; set; }
         
        public DateTime SentAt { get; set; }
    }
}
