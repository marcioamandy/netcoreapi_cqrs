using System;
using System.Collections.Generic;
using System.Text;

namespace Globo.PIC.Domain.Exceptions
{
	public class BadRequestException : Exception
	{
		public BadRequestException(string message) : base(message)
		{
		}
	}
}
