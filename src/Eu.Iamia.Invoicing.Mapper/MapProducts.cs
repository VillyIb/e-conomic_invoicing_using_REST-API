using Eu.Iamia.Invoicing.E_Conomic.Gateway;

namespace Eu.Iamia.Invoicing.Mapper;

public  class MapProducts : IMapProducts
{
    // TODO load MapList from configuration

    private static readonly Dictionary<Loader.Contract.Products, string> MapList = new()
    {
        {Loader.Contract.Products.MedlemsKontingent, "2"},
        {Loader.Contract.Products.Grundejerforening, "3"},
        {Loader.Contract.Products.Jordvarme, "24"},
        {Loader.Contract.Products.Vandafledning, "4"},
        {Loader.Contract.Products.VideresalgVand, "5"},
        {Loader.Contract.Products.VideresalgElektricitet, "6"},
        {Loader.Contract.Products.LejeErhvervsareal, "7"},
        {Loader.Contract.Products.LejeNyttehave, "25"},
        {Loader.Contract.Products.LejeMarkskur, "21"},
        {Loader.Contract.Products.LejeJordkælder, "26"}
    };

    public  Product? FromInput( Loader.Contract.Products inputProducts)
    {
        var productId = MapList.FirstOrDefault(ps => ps.Key == inputProducts).Value;
        return new Product(productId);
    }
}
