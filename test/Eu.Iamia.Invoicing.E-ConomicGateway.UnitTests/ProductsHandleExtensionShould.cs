using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.Product;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Product;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;
public class ProductsHandleExtensionShould
{
    //private static ProductsHandle productsHandle = new ProductsHandle
    //{
    //    collection = new List<Collection>
    //    {
    //        new Collection
    //        {
    //            productNumber = "1",
    //            description = "Product 1",
    //            name = "Product 1 name"
    //        }
    //    }
    //};

    //private static string ProductsHandleString => ProductsHandleExtension.ToJson(productsHandle);

    private static string ProductsHandleJsonValid => "{\"collection\":[{\"productNumber\":\"1\",\"description\":\"Product 1\",\"name\":\"Product 1 name\"}]}";

    private static string ProductsHandleJsonInvalidFormat => null;

    private static string ProductsHandleJsonInvalidContent => "{\"collection\":[{\"productNumber\":\"\",\"description\":\"Product 1\",\"name\":\"Product 1 name\"}]}";

    private static string ProductsHandleJsonEmpty => "{ }";

    [Fact]
    public void GivenValidJson_DeserializeOK()
    {
        var actual = ProductsHandleExtension.FromJson(ProductsHandleJsonValid);
        Assert.NotNull(actual);
        Assert.Single(actual.collection);
        Assert.Equal("1", actual.collection[0].productNumber);
    }

    [Fact]
    public void GivenInvalidFormat_DeserializeFail_With_JsonException()
    {
        var ex = Assert.Throws<JsonException>(() => ProductsHandleExtension.FromJson(ProductsHandleJsonInvalidFormat));

        Assert.NotNull(ex.InnerException);
    }

    [Fact]
    public void GivenInvalidContent_DeserializeFail_With_JsonException()
    {
        var value = ProductsHandleExtension.FromJson(ProductsHandleJsonInvalidContent);

        Assert.NotNull(value);
        Assert.Null(value.collection);
    }

    [Fact]
    public void GivenEmptyJson_DeserializeFail_With_NullProperties()
    {
        var value = ProductsHandleExtension.FromJson(ProductsHandleJsonEmpty);
        Assert.NotNull(value);
        Assert.Null(value.collection);
    }

}
