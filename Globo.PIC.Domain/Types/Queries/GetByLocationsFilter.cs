using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace Globo.PIC.Domain.Types.Queries
{
    public class GetByLocationsFilter :
        IRequest<Location>
    {

        /// <summary>
        /// 
        /// </summary>
        [Description("Nome da Organização")]
        public string OrganizationName { get; set; }
    }
}
