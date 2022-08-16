using System.ComponentModel;

namespace Globo.PIC.Domain.Enums
{
	public enum Role
	{

		#region "Demandante"
		[Description("Demandante")]
		PERFIL_DEMANDANTE,

		[Description("Gestor de Demandantes")]
		GRANT_GESTOR_DEMANDANTE,
		#endregion

		#region "Suprimentos"
		[Description("Suprimentos")]
		PERFIL_BASE_SUPRIMENTOS,

		[Description("Gestor de Base de Suprimentos")]
		GRANT_GESTOR_BASE_SUPRIMENTOS,
		#endregion

		#region "Comprador"
		[Description("Comprador")]
		PERFIL_COMPRADOR_ESTRUTURADA,

		[Description("Comprador Externo")]
		PERFIL_COMPRADOR_EXTERNA,
		#endregion

		#region "demais grants"
		[Description("Adminstração de Usuários")]
		GRANT_ADM_USUARIOS,

		[Description("Acesso completo para devs")]
		GRANT_DEVELOPER,
		#endregion

		#region "Comprador Veículos"
		[Description("Comprador de Veículos")]
		PERFIL_COMPRADOR_VEICULOS
		#endregion
	}
}
