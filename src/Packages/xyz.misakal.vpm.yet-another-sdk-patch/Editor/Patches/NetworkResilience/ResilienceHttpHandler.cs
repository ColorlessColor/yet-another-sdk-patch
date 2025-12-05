using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using UnityEngine;

namespace YetAnotherPatchForVRChatSdk.Patches.NetworkResilience;

internal sealed class ResilienceHttpHandler : DelegatingHandler
{
    private readonly ResiliencePipeline _pipeline;

    public ResilienceHttpHandler(HttpMessageHandler innerHandler) : base(innerHandler)
    {
        var options = new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<HttpRequestException>(),
            MaxRetryAttempts = 5,
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            Delay = TimeSpan.FromSeconds(3),
            OnRetry = arguments =>
            {
                Debug.LogWarning(
                    $"Retrying HTTP request. Attempt {arguments.AttemptNumber}. Delay {arguments.RetryDelay.ToString()}.");

                return default;
            }
        };

        var builder = new ResiliencePipelineBuilder()
            .AddRetry(options);

        _pipeline = builder.Build();
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        return _pipeline.ExecuteAsync(
                async token => await base.SendAsync(request, token),
                cancellationToken)
            .AsTask();
    }
}