using System.ComponentModel;

namespace Globo.PIC.Domain.Enums
{
	public enum RoleType
	{
		[Description("Permissões Específicas")]
		Grant,

		[Description("Perfis")]
		Profile
	}
}
