using Globo.PIC.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Domain.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	public interface ITokenDLProxy : IDisposable
	{

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //Task<TokenDL> GetDLToken(CancellationToken cancellationToken);
        public Task<TokenDL> GetDLToken(CancellationToken cancellationToken);


    }
}
