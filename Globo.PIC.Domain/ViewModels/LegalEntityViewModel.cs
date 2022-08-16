using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class LegalEntityViewModel
    {
        [Description("Id")]
        public long Id { get; set; }
        [Description("Name")]
        public string Name { get; set; }

    }

}
