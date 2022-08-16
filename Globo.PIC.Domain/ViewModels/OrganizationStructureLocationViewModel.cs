using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class OrganizationStructureLocationViewModel
    {
        [Description("Id")]
        public long Id { get; set; }
        [Description("Code")]
        public string Code { get; set; } 

    }

}
