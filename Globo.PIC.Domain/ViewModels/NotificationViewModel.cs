using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Globo.PIC.Domain.ViewModels
{
    public class NotificationViewModel
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

        /// <summary>
        /// 
        /// </summary>
        [Description("Lido?")]
        public bool IsRead { get; set; }
    }
}
