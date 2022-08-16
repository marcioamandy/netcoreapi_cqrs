using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Globo.PIC.Domain.Extensions
{
	public static class JwtExtensions
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string JWTDecode(this string value, string type)
		{
			string cognitoUsername = string.Empty;

			try
			{
				var jsonToken = new JwtSecurityTokenHandler().ReadToken(value) as JwtSecurityToken;

				cognitoUsername = jsonToken?.Claims?.FirstOrDefault(c => c.Type == type)?.Value;	
			}
			catch
			{
			}

			return cognitoUsername;
		}
	}
}
