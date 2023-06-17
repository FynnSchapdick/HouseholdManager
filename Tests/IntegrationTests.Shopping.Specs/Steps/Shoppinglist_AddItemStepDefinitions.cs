using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using HouseholdManager.Api.Endpoints.Shopping.AddShoppingListItem;
using HouseholdManager.Domain.Product;
using HouseholdManager.Domain.Shopping;
using IntegrationTests.Shopping.Specs.Data;
using Testing.Shared.Setup;

namespace IntegrationTests.Shopping.Specs.Steps;

[Binding]
public class Shoppinglist_AddItemStepDefinitions
{
    private readonly HouseholdManagerWebApplicationFactory _factory;
    private readonly ScenarioContext _scenarioContext;

    public HttpClient Client { get; private set; } = null!;

    public Shoppinglist_AddItemStepDefinitions(HouseholdManagerWebApplicationFactory factory, ScenarioContext scenarioContext)
    {
        _factory = factory;
        _scenarioContext = scenarioContext;
    }

    [Given(@"a shopping list")]
    public async Task GivenAShoppinglist()
    {
        _scenarioContext.Set(await Setup.SingleShoppingList(_factory));
    }

    [Given(@"a product")]
    public async Task GivenAProduct()
    {
        _scenarioContext.Set(await Setup.SingleProduct(_factory));
    }

    [Given(@"a client to make http calls with")]
    public void GivenAClientToMakeHttpCallsWith()
    {
        _scenarioContext.Set(_factory.CreateDefaultClient());
    }

    [When(@"an item is added to the shopping list with the product's id using the client")]
    public async Task WhenAnItemIsAddedToTheShoppinglistWithTheProductsIdUsingTheClient()
    {
        var client = _scenarioContext.Get<HttpClient>();
        var product = _scenarioContext.Get<ProductAggregate>();
        var list = _scenarioContext.Get<ShoppingListAggregate>();

        HttpResponseMessage result = await client.PostAsJsonAsync($"/api/v1/shoppinglists/{list.Id}/items", new AddShoppingListItemRequest(product.Id));

        _scenarioContext.Set(result);
    }

    [Then(@"the resulting statuscode should be ""(.*)""")]
    public void ThenTheResultingStatuscodeShouldBe(HttpStatusCode statusCode)
    {
        var response = _scenarioContext.Get<HttpResponseMessage>();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Then(@"the location header should be set")]
    public void ThenTheLocationHeaderShouldBeSet()
    {
        var response = _scenarioContext.Get<HttpResponseMessage>();
        var list = _scenarioContext.Get<ShoppingListAggregate>();
        var client = _scenarioContext.Get<HttpClient>();
        response.Headers.Location.Should().NotBeNull()
            .And.BeEquivalentTo(new Uri(client.BaseAddress!, $"api/v1/shoppinglists/{list.Id}"));
    }
}
