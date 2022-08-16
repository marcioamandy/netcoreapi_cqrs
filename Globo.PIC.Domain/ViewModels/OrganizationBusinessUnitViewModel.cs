using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class OrganizationBusinessUnitViewModel
    {
        [Description("Id")]
        public long Id { get; set; }
        [Description("Code")]
        public string Description { get; set; }

    }

}
