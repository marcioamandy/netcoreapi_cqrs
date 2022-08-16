using System.Collections.Generic;

namespace Globo.PIC.Domain.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class Tarefa
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Chargeable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ParentNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long ProjectNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CentroCusto CentroCusto { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Finalidade Finalidade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Tarefa> Niveis { get; set; } = new List<Tarefa>();

    }
}
