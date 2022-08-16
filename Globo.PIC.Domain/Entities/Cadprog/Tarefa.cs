namespace Globo.PIC.Domain.Entities.Cadprog
{

    /// <summary>
    /// 
    /// </summary>
    public class Tarefa
    {

        /// <summary>
        /// 
        /// </summary>
        public long TaskId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TaskNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TaskLevel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? ParentTaskId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DataInicioTarefa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DataFimTarefa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CentroCusto { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CentroResultado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Finalidade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Projeto { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Segmento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Chargeable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectTask"></param>
        public void Update(Tarefa projectTask)
        {
            TaskNumber = projectTask.TaskNumber;
            CentroCusto = projectTask.CentroCusto;
            TaskName = projectTask.TaskName;
            TaskLevel = projectTask.TaskLevel;
            ParentTaskId = projectTask.ParentTaskId;
            ProjectId = projectTask.ProjectId;
            DataInicioTarefa = projectTask.DataInicioTarefa;
            DataFimTarefa = projectTask.DataFimTarefa;
            CentroCusto = projectTask.CentroCusto;
            CentroResultado = projectTask.CentroResultado;
            Finalidade = projectTask.Finalidade;
            Projeto = projectTask.Projeto;
            Chargeable = projectTask.Chargeable;
            Segmento = projectTask.Segmento;
        }
    }
}
