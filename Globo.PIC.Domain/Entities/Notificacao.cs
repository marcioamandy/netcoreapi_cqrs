using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class Notificacao
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Range(1, long.MaxValue)]
        [Description("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Notificação")]
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Criado Em")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Link")]
        public string Link { get; set; }

        #region Relationship one to many properties

        public virtual IEnumerable<Reader> Readers { get; set; }

        public virtual IEnumerable<Viewer> Viewers { get; set; }

        public virtual IEnumerable<Assign> Assigns { get; set; }

        #endregion
    }
}
