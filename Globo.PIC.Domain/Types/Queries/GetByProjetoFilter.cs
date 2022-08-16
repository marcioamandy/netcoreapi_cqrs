using Globo.PIC.Domain.Models;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel;

namespace Globo.PIC.Domain.Types.Queries
{
    public class GetByProjetoFilter :
        IRequest<ProjetoModel>,
        IRequest<List<Tarefa>>
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("Id Projeto Oracle")]
        public long ProjectId { get; set; }
    }
}
