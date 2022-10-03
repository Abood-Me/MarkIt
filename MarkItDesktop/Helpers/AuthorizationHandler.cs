using MarkItDesktop.Data;
using MarkItDesktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarkItDesktop.Helpers
{
    public class AuthorizationHandler : DelegatingHandler
    {
        private readonly IClientDataStore _store;

        public AuthorizationHandler(IClientDataStore store)
        {
            this._store = store;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (await _store.GetLoginTokenAsync() is string token)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
