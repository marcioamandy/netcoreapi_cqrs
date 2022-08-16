using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globo.PIC.Domain.Models
{
	/// <summary>
	/// 
	/// </summary>
	public class EmailSettings
	{
		/// <summary>
		/// 
		/// </summary>
		public string Domain { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		public string Port { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string NameFrom { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string From { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		public string SubjectToSupplier { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string SubjectPrefixToNotification { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Cco { get; set; }
	}
}
