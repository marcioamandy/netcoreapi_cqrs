using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Domain.ViewModels
{
    public class UserRoleViewModel
    {
        /// <summary>
        ///
        /// </summary>
        [Required]
        [Description("Role")]
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Description("Login do Usuário")]
        public string Login { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Descrição da Role")]
        public string Description
        {
            get
            {
                string val = string.Empty;

                try
                {
                    val = Enum.Parse<Role>(Name).GetEnumDescription();
                }
                catch { }

                return val;
            }
        }
    }
}
