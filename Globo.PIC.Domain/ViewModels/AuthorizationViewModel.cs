using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Globo.PIC.Domain.ViewModels
{
	public class AuthorizationViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		[Description("Roles")]
		public IEnumerable<string> Roles { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public AuthorizationViewModel()
		{
		}
	}
}
