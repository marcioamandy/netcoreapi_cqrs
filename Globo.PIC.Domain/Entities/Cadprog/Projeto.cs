namespace Globo.PIC.Domain.Entities.Cadprog
{
    /// <summary>
    /// 
    /// </summary>
    public class Projeto
    {

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
        public string CodigoPrograma { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        public void Update(Projeto project)
        {
            this.CodigoPrograma = project.CodigoPrograma;
            this.ProjectName = project.ProjectName;
        }
    }
}
