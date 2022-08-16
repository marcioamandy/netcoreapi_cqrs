using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Domain.Interfaces
{
	public interface INodeServiceProxy : IDisposable
	{
		Task<Stream> PutPdfGenAsync(string body, CancellationToken cancellationToken);
	}
}