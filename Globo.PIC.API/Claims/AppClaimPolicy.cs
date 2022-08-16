namespace Globo.PIC.API.Claims
{

	/// <summary>
	/// Políticas de segurança
	/// </summary>
	public class AppClaimPolicies
	{

		/// <summary>
		/// Política de segurança que verifica se o perfil enviado pelo front é o mesmo configurado no banco
		/// </summary>
		public const string MatchProfile = "MatchProfile";

	}
}
