using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class OrganizacaoViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("Id")]
        public long Id { get; set; } 
        public string Code { get; set; }
        public string Name { get; set; }
        public OrganizationStructureLocationViewModel Location { get; set; }
        public string Status { get; set; }

        public OrganizationBusinessUnitViewModel OrganizationBusinessUnit { get; set; }
        public LegalEntityViewModel LegalEntity { get; set; }

        public ManagementBusinessUnitViewModel ManagementBusinessUnit { get; set; }





        //public long Id { get; set; }
        //public string Code { get; set; }
        //public string Name { get; set; }
        //public OrganizationStructureLocation Location { get; set; }
        //public string Status { get; set; }

        //public OrganizationBusinessUnit OrganizationBusinessUnit { get; set; }
        //public LegalEntity LegalEntity { get; set; }

        //public ManagementBusinessUnit ManagementBusinessUnit { get; set; }
    }

}
