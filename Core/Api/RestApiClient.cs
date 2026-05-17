using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Core.Configuration;
using NLog;
using RestSharp;
using RestSharp.Serializers.Json;

namespace Core.Api;

/// <summary>
/// Thread-safe RestSharp wrapper with NLog instrumentation.
/// </summary>
public class RestApiClient
{
    private static readonly Logger _log = LogManager.GetCurrentClassLogger();
    private const int BodyExcerptLength = 500;

    private readonly RestClient _client;

    public RestApiClient(ApiConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);

        // PropertyNameCaseInsensitive avoids per-property [JsonPropertyName] attributes on DTOs.
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        _client = new RestClient(
            new RestClientOptions(config.BaseUrl) { Timeout = TimeSpan.FromSeconds(config.Timeout) },
            configureSerialization: s => s.UseSystemTextJson(serializerOptions));
    }

    public async Task<RestResponse> ExecuteAsync(RestRequest request)
    {
        var (method, url) = GetRequestInfo(request);
        LogRequest(method, url);

        var stopwatch = Stopwatch.StartNew();
        var response = await _client.ExecuteAsync(request);
        stopwatch.Stop();

        if (response.ResponseStatus != ResponseStatus.Completed)
        {
            LogTransportFailure(method, url, response.ErrorException, response.ErrorMessage);
            return response;
        }

        LogResponse(method, url, response.StatusCode, stopwatch.ElapsedMilliseconds, response.IsSuccessful, response.Content);
        return response;
    }

    public async Task<RestResponse<T>> ExecuteAsync<T>(RestRequest request)
    {
        var (method, url) = GetRequestInfo(request);
        LogRequest(method, url);

        var stopwatch = Stopwatch.StartNew();
        var response = await _client.ExecuteAsync<T>(request);
        stopwatch.Stop();

        if (response.ResponseStatus != ResponseStatus.Completed)
        {
            LogTransportFailure(method, url, response.ErrorException, response.ErrorMessage);
            return response;
        }

        LogResponse(method, url, response.StatusCode, stopwatch.ElapsedMilliseconds, response.IsSuccessful, response.Content);
        return response;
    }

    private (string Method, string Url) GetRequestInfo(RestRequest request)
    {
        var method = request.Method.ToString().ToUpperInvariant();
        var url = _client.BuildUri(request).ToString();
        return (method, url);
    }

    private static void LogRequest(string method, string url)
        => _log.Info("HTTP {Method} {Url}", method, url);

    private static void LogTransportFailure(string method, string url, Exception? ex, string? errorMessage)
        => _log.Error(ex, "HTTP {Method} {Url} failed: {Message}", method, url,
               errorMessage ?? ex?.Message ?? "Unknown transport error");

    private static void LogResponse(string method, string url, HttpStatusCode status, long elapsedMs, bool isSuccessful, string? content)
    {
        if (isSuccessful)
        {
            _log.Info("HTTP {Method} {Url} -> {Status} in {Elapsed}ms", method, url, (int)status, elapsedMs);
            return;
        }

        _log.Warn("HTTP {Method} {Url} -> {Status} in {Elapsed}ms", method, url, (int)status, elapsedMs);

        var excerpt = content is not null && content.Length > BodyExcerptLength
            ? content[..BodyExcerptLength]
            : content;
        _log.Debug("Response body excerpt: {Body}", excerpt);
    }
}
