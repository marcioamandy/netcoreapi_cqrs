namespace Globo.PIC.Domain.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class ProjetoModel
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
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string Status { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string SourceCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BusinessUnit BusinessUnit { get; set; }
    }
}
