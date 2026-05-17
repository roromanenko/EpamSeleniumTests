using RestSharp;

namespace Core.Api;

/// <summary>
/// Fluent builder for constructing <see cref="RestRequest"/> instances step by step.
/// </summary>
public class RequestBuilder
{
    private Method _method = Method.Get;
    private string? _resource;
    private readonly List<(string Name, string Value)> _headers = [];
    private readonly List<(string Name, string Value)> _queryParameters = [];
    private Action<RestRequest>? _bodyConfigurator;

    public RequestBuilder WithMethod(Method method)
    {
        _method = method;
        return this;
    }

    public RequestBuilder WithResource(string resource)
    {
        _resource = resource;
        return this;
    }

    public RequestBuilder WithHeader(string name, string value)
    {
        _headers.Add((name, value));
        return this;
    }

    public RequestBuilder WithQueryParameter(string name, string value)
    {
        _queryParameters.Add((name, value));
        return this;
    }

    public RequestBuilder WithJsonBody<T>(T body) where T : class
    {
        _bodyConfigurator = request => request.AddJsonBody(body);
        return this;
    }

    /// <summary>
    /// Builds the configured <see cref="RestRequest"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when <see cref="WithResource"/> was not called.</exception>
    public RestRequest Build()
    {
        if (_resource is null)
            throw new InvalidOperationException("Resource must be set via WithResource() before calling Build().");

        var request = new RestRequest(_resource, _method);

        foreach (var (name, value) in _headers)
            request.AddHeader(name, value);

        foreach (var (name, value) in _queryParameters)
            request.AddQueryParameter(name, value);

        _bodyConfigurator?.Invoke(request);

        return request;
    }
}
