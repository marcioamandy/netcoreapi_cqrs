using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Globo.PIC.Domain.Models
{

	/// <summary>
	/// 
	/// </summary>
	public class Authorization
	{

		/// <summary>
		/// 
		/// </summary>
		[Description("Roles")]
		public IEnumerable<string> Roles { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Authorization()
		{
		}

	}
}
