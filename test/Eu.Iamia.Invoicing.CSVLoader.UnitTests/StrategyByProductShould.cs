using Eu.Iamia.Invoicing.Loader.Contract;

namespace Eu.Iamia.Invoicing.CSVLoader.UnitTests;

public class StrategyByProductShould
{
    [Fact]
    public void ParsesAllProducts()
    {
        var allProducts = (Products[])Enum.GetValues(typeof(Products));
        var now = DateTime.Now;
        var _ = allProducts.Select(product => new StrategyByProduct(product, now)).ToList();
    }
}
