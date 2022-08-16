using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Globo.PIC.Domain.ViewModels.Emails
{
    public class DefaultNotificationViewModel
    {

        /// <summary>
        /// 
        /// </summary>
        [Description("Subject")]
        public string Subject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Body")]
        public string Body { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> ToEmails { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DefaultNotificationViewModel()
        {
            ToEmails = new List<string>();
        }
    }
}
