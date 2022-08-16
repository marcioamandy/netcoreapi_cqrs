using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Globo.PIC.Domain.Entities
{
    public class Viewer
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
        [Description("Login")]
        public string Login { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Range(1, long.MaxValue)]
        [Required]
        [Description("Id da Notificação")]
        public long? NotificationId { get; set; }

        #region Relationship many to one properties

        public virtual Notificacao Notification { get; set; }

        #endregion
    }
}
