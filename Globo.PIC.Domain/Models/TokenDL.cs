using System;
using System.Collections.Generic;
using System.Text;

namespace Globo.PIC.Domain.Models
{
	public class TokenDL
	{  
		public DateTime ExpiresOn { get; set; }  
		public string Token { get; set; }
		public void Update(TokenDL tokenDL)
        {
			ExpiresOn = tokenDL.ExpiresOn;
			Token = tokenDL.Token;
        }
		public bool IsValid()
        {
			return ( ExpiresOn <= DateTime.Now && !string.IsNullOrWhiteSpace(Token));
        }
	}
}
