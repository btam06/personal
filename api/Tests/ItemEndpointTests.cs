using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Xunit;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests;

public class ItemEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    private readonly String _clientSecret;

    public ItemEndpointTests(WebApplicationFactory<Program> factory)
    {
        var config = factory.Services.GetRequiredService<IConfiguration>();

        _client       = factory.CreateClient();
        _clientSecret = config["Strapi:ClientSecret"]!;
    }

    [Fact]
    public async Task GetItems_WhenOk_ReturnAllItems()
    {
        var created  = await this.CreateTestAsync();
        var response = await _client.GetAsync("/api/items");
        var list     = await response.Content.ReadFromJsonAsync<List<ItemResponseDto>>();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        list.Should().Contain(i => i.Id == created.Id);

        await this.DeleteTestAsync(created);
    }

    [Fact]
    public async Task GetItem_WhenOk_ReturnItem()
    {
        var created  = await this.CreateTestAsync();
        var response = await _client.GetAsync($"/api/items/{created!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await this.DeleteTestAsync(created);
    }

    [Fact]
    public async Task PostItem_WhenOk_ReturnItem()
    {
        var payload = new ItemRequestDto
        {
            Name = "Test"
        };

        var response = await this.SendWithAuthAsync(HttpMethod.Post, "/api/items", payload, "items.write");
        var created  = await response.Content.ReadFromJsonAsync<ItemResponseDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        created.Should().NotBeNull();
        created.Name.Should().Be(payload.Name);

        await this.DeleteTestAsync(created);
    }

    [Fact]
    public async Task DeleteItem_WhenOk_ItemDeleted()
    {
        var created  = await this.CreateTestAsync();
        var response = await _client.DeleteAsync($"/api/items/{created!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PutItem_WhenOk_UpdateItem()
    {
        var created = await this.CreateTestAsync();

        var put = new ItemRequestDto
        {
            Name = "Test Update"
        };

        var updated_response = await this.SendWithAuthAsync(HttpMethod.Put, $"/api/items/{created!.Id}", put, "items.write");
        var updated          = await updated_response.Content.ReadFromJsonAsync<ItemResponseDto>();

        updated_response.StatusCode.Should().Be(HttpStatusCode.OK);
        updated.Should().NotBeNull();
        updated.Name.Should().Be(put.Name);


        await this.DeleteTestAsync(created);
    }

    [Fact]
    public async Task PostItem_WhenNoToken_ReturnsUnauthorized()
    {
        var payload = new ItemRequestDto { Name = "Test" };
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/items")
        {
            Content = JsonContent.Create(payload)
        };

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }


    [Fact]
    public async Task GetToken_WhenScopeNotGrantedToClient_TokenRequestFails()
    {
        // proves: OpenIddict itself won't even issue a token for scopes the client wasn't granted
        var tokenResponse = await _client.PostAsync("/connect/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = "strapi-cms",
            ["client_secret"] = _clientSecret,
            ["scope"] = "wrong.scope"
        }));

        tokenResponse.StatusCode.Should().NotBe(HttpStatusCode.OK);
    }

    private async Task<ItemResponseDto> CreateTestAsync()
    {
        var post = new ItemRequestDto
        {
            Name = "Test"
        };

        var posted_response = await this.SendWithAuthAsync(HttpMethod.Post, "/api/items", post, "items.write");
        var posted          = await posted_response.Content.ReadFromJsonAsync<ItemResponseDto>();

        posted_response.StatusCode.Should().Be(HttpStatusCode.OK);
        posted.Should().NotBeNull();

        return posted;
    }


    private async Task DeleteTestAsync(ItemResponseDto created)
    {
        var delete_response = await this.SendWithAuthAsync(HttpMethod.Delete, $"/api/items/{created!.Id}", null, "items.write");
        delete_response.StatusCode.Should().Be(HttpStatusCode.OK);
    }


    private async Task<string> GetAccessTokenAsync(string scope)
    {
        var tokenResponse = await _client.PostAsync("/connect/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = "strapi-cms",
            ["client_secret"] = _clientSecret,
            ["scope"] = scope
        }));

        tokenResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var token = await tokenResponse.Content.ReadFromJsonAsync<TokenResponse>();
        return token!.AccessToken;
    }

    private async Task<HttpResponseMessage> SendWithAuthAsync(HttpMethod method, string url, object? payload, string scope)
    {
        var token = await GetAccessTokenAsync(scope);
        var request = new HttpRequestMessage(method, url);

        if (payload is not null) {
            request.Content = JsonContent.Create(payload);
        }

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await _client.SendAsync(request);
    }

    private record TokenResponse
    {
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; init; }
    }
}
