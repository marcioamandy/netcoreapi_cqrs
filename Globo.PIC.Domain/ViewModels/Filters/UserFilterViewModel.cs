using System.Collections.Generic;
using System.ComponentModel;

namespace Globo.PIC.Domain.ViewModels
{
    public class UserFilterViewModel : BaseFilterViewModel
    {
		/// <summary>
		/// 
		/// </summary>
		[Description("Campo de busca")]
		public string Search { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Departamento Id")]
        public long? DepartmentId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Roles")]
		public IEnumerable<string> Roles { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("É Gestor?")]
		public bool? IsAdm { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Está Ativo?")]
		public bool? IsActive { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public UserFilterViewModel()
        {
			Roles = new List<string>();
		}
    }
}
