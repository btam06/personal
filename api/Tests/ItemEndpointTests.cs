using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Xunit;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;

namespace Tests;

public class ItemEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ItemEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
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

        var response = await _client.PostAsJsonAsync("/api/items", payload);
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

        var updated_response = await _client.PutAsJsonAsync($"/api/items/{created!.Id}", put);
        var updated          = await updated_response.Content.ReadFromJsonAsync<ItemResponseDto>();

        updated_response.StatusCode.Should().Be(HttpStatusCode.OK);
        updated.Should().NotBeNull();
        updated.Name.Should().Be(put.Name);


        await this.DeleteTestAsync(created);
    }

    private async Task<ItemResponseDto> CreateTestAsync()
    {
        var post = new ItemRequestDto
        {
            Name = "Test"
        };

        var posted_response = await _client.PostAsJsonAsync("/api/items", post);
        var posted          = await posted_response.Content.ReadFromJsonAsync<ItemResponseDto>();

        posted_response.StatusCode.Should().Be(HttpStatusCode.OK);
        posted.Should().NotBeNull();

        return posted;
    }


    private async Task DeleteTestAsync(ItemResponseDto created)
    {
        var delete_response = await _client.DeleteAsync($"/api/items/{created!.Id}");
        delete_response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
