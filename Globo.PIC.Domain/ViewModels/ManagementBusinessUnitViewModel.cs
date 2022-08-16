using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class ManagementBusinessUnitViewModel
    {
        [Description("Id")]
        public long Id { get; set; }
        [Description("Description")]
        public string Description { get; set; } 

    }

}
