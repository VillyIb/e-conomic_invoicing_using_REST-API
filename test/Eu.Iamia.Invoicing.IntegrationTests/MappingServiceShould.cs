namespace Eu.Iamia.Invoicing.Mapping.IntegrationTests;

public class MappingServiceShould
{
    private IMappingService _sut;

    private CancellationTokenSource _cts;

    public MappingServiceShould()
    {
        _cts = new CancellationTokenSource();
        using var setup = new Setup();
        _sut = setup.GetService<IMappingService>();
    }

    [Fact]
    public async Task LoadCustomerCache_OK()
    {
        var customerGroupsToAccept = new List<int> { 1, 2, 3 };
        var customerCount = await _sut.LoadCustomerCache(customerGroupsToAccept);
        Assert.True(customerCount > 1);
    }

    [Fact]
    public async Task LoadProductCache_OK()
    {
        var customerCount = await _sut.LoadProductCache();
        Assert.True(customerCount > 1);
    }
}