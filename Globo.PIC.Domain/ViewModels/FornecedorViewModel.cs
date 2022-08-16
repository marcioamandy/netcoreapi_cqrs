using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class FornecedorViewModel
    {
        [Description("Id")]
        public long Id { get; set; }
        [Description("Name")]
        public string Name { get; set; }
        [Description("CNPJ ou Code")]
        public string Code { get; set; }
    }
}
