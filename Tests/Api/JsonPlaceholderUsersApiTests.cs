using System.Net;
using Core.Api;
using FluentAssertions;
using NLog;
using PageObjects.Models;
using RestSharp;

namespace Tests.Api;

[TestFixture]
[Category("API")]
[Parallelizable(ParallelScope.All)]
public class JsonPlaceholderUsersApiTests : ApiBaseTest
{
    private static readonly Logger _log = LogManager.GetCurrentClassLogger();

    private const string UsersEndpoint = "/users";
    private const string InvalidEndpoint = "/invalidendpoint";
    private const string ExpectedContentType = "application/json; charset=utf-8";
    private const int ExpectedUserCount = 10;

    [Test]
    public async Task GetUsers_ReturnsOk_AndEachUserHasRequiredFields()
    {
        _log.Info("Validating /users returns 200 and every user has required fields");

        var request = new RequestBuilder()
            .WithMethod(Method.Get)
            .WithResource(UsersEndpoint)
            .Build();

        var response = await ApiClient.ExecuteAsync<List<User>>(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNullOrEmpty();

        foreach (var user in response.Data!)
        {
            user.Id.Should().BePositive();
            user.Name.Should().NotBeNullOrEmpty();
            user.Username.Should().NotBeNullOrEmpty();
            user.Email.Should().NotBeNullOrEmpty();
            user.Phone.Should().NotBeNullOrEmpty();
            user.Website.Should().NotBeNullOrEmpty();
            user.Address.Should().NotBeNull();
            user.Company.Should().NotBeNull();
        }
    }

    [Test]
    public async Task GetUsers_ContentTypeHeader_IsApplicationJsonUtf8()
    {
        _log.Info("Validating /users Content-Type header equals application/json; charset=utf-8");

        var request = new RequestBuilder()
            .WithMethod(Method.Get)
            .WithResource(UsersEndpoint)
            .Build();

        var response = await ApiClient.ExecuteAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var contentType = response.ContentHeaders?.FirstOrDefault(h => h.Name == "Content-Type")?.Value?.ToString();
        contentType.Should().Be(ExpectedContentType);
    }

    [Test]
    public async Task GetUsers_ReturnsArrayOfTenUsers_WithDistinctIdsAndNonEmptyKeyFields()
    {
        _log.Info("Validating /users returns exactly 10 users with distinct IDs and non-empty key fields");

        var request = new RequestBuilder()
            .WithMethod(Method.Get)
            .WithResource(UsersEndpoint)
            .Build();

        var response = await ApiClient.ExecuteAsync<List<User>>(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().HaveCount(ExpectedUserCount);
        response.Data!.Select(u => u.Id).Should().OnlyHaveUniqueItems();

        foreach (var user in response.Data)
        {
            user.Name.Should().NotBeNullOrEmpty();
            user.Username.Should().NotBeNullOrEmpty();
            user.Company.Name.Should().NotBeNullOrEmpty();
        }
    }

    [Test]
    public async Task PostUser_WithNameAndUsername_Returns201_WithIdInBody()
    {
        _log.Info("Validating POST /users with Name+Username returns 201 and a non-zero Id");

        // JSONPlaceholder POST is a simulation — it returns a fake created resource (id = 11 or higher)
        // but does not persist state; no cleanup is needed.
        var request = new RequestBuilder()
            .WithMethod(Method.Post)
            .WithResource(UsersEndpoint)
            .WithJsonBody(new CreateUserRequest("John Doe", "johndoe"))
            .Build();

        var response = await ApiClient.ExecuteAsync<User>(request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Data.Should().NotBeNull();
        response.Data!.Id.Should().BePositive();
    }

    [Test]
    public async Task GetInvalidEndpoint_Returns404()
    {
        _log.Info("Validating GET /invalidendpoint returns 404 and IsSuccessful is false");

        // RestSharp does not throw on 4xx/5xx; StatusCode and IsSuccessful are safe to read directly.
        var request = new RequestBuilder()
            .WithMethod(Method.Get)
            .WithResource(InvalidEndpoint)
            .Build();

        var response = await ApiClient.ExecuteAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.IsSuccessful.Should().BeFalse();
    }
}
