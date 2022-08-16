using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Globo.PIC.Domain.ViewModels
{
    public class AssignViewModel
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
        [Description("Role")]
        public string Role { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Range(1, long.MaxValue)]
        [Required]
        [Description("Id da Notificação")]
        public long? NotificationId { get; set; }

        #region Relationship many to one properties

        /// <summary>
        /// 
        /// </summary>
        public NotificationViewModel Notification { get; set; }

        #endregion
    }
}
