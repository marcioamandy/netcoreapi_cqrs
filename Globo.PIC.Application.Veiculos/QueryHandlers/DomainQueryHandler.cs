using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Extensions;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Globo.PIC.Domain.Enums;
using MediatR;
using System.Threading;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Types.Queries.Filters;

namespace Globo.PIC.Application.QueryHandlers
{
    public class DomainQueryHandler :
		IRequestHandler<GetPreSignedUrl, string>
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly IS3Client s3Client;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_localRepository"></param>
		public DomainQueryHandler(
			IS3Client _s3Client
			)
		{
			
			s3Client = _s3Client;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<string> IRequestHandler<GetPreSignedUrl, string>.Handle(GetPreSignedUrl request, CancellationToken cancellationToken)
		{
			return Task.FromResult(s3Client.GetPreSignedUrl(request.Key, request.FileName, request.ContentType, request.Verb, request.Expires)); 
		}


	}
}
