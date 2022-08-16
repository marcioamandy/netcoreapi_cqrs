using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Globo.PIC.Domain.Types.Queries
{
    public class GetByProjetosFilter :
        IRequest<List<ProjetoModel>>,
        IRequest<List<Tarefa>>
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("Id Projeto Oracle")]
        public List<long> ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Nome do Projeto")]
        public string ProjectName { get; set; }
    }
}
