using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace YetAnotherPatchForVRChatSdk.Patches.NetworkResilience;

internal sealed class HttpLoggingHandler : DelegatingHandler
{
    public HttpLoggingHandler(HttpMessageHandler innerHandler) : base(innerHandler)
    {
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        Debug.Log($"{request.Method} {request.RequestUri.GetLeftPart(UriPartial.Path)}");
        return base.SendAsync(request, cancellationToken);
    }
}