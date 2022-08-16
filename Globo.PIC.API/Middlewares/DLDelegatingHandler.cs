
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.API.Middlewares
{
    public class DLDelegatingHandler: DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DLDelegatingHandler> _logger;
        private readonly ITokenDLProxy _tokenDLProxy;
        private readonly TokenDL _tokenDL ;
        public DLDelegatingHandler(IHttpContextAccessor httpContextAccessor, ILogger<DLDelegatingHandler> logger,
            ITokenDLProxy tokenDLProxy, TokenDL tokenDL)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _tokenDLProxy = tokenDLProxy;
            _tokenDL = tokenDL;
        }


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {   
            var newTokenDl = await _tokenDLProxy.GetDLToken(cancellationToken);
            _tokenDL.Update(newTokenDl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenDL.Token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
