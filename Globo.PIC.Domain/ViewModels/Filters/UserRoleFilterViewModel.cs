using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class UserRoleFilterViewModel : BaseFilterViewModel
    {
        /// <summary>
		/// 
		/// </summary>
		[Description("Login")]
        public string Login{ get; set; }

		/// <summary>
		/// 
		/// </summary>
		public UserRoleFilterViewModel()
		{

		}
	}
}
