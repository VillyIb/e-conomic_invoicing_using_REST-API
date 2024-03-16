using Eu.Iamia.Invoicing.Loader.Contract;

namespace Eu.Iamia.Invoicing.CSVLoader;
public  class StrategyByProduct
{
    public string ProductName { get;  }

    public Products Product { get; }

    public Units Units { get; }

    public double UnitNetPrice { get; }


    public StrategyByProduct(Products product, DateTime invoiceYear)
    {
        var referenceYear = invoiceYear.AddYears(-1);

        switch (product)
        {
            // TODO Load from configuration

            // ReSharper disable StringLiteralTypo

            case Products.MedlemsKontingent:
                ProductName = $"Medlemskontingent ({invoiceYear:yyyy})";
                Product = Products.MedlemsKontingent;
                Units = Units.år;
                UnitNetPrice = 3000.00;
                break;
            case Products.Grundejerforening:
                ProductName = $"Grundejerforening ({invoiceYear:yyyy})";
                Product = Products.Grundejerforening;
                Units = Units.m2;
                UnitNetPrice = 6.00;
                break;
            case Products.Jordvarme:
                ProductName = $"Jordvarme driftsbidrag ({invoiceYear:yyyy})";
                Product = Products.Jordvarme;
                Units = Units.kW;
                UnitNetPrice = 240.00;
                break;
            case Products.Vandafledning:
                ProductName = $"Vandafledning ({referenceYear:yyyy})";
                Product = Products.Vandafledning;
                Units = Units.m3;
                UnitNetPrice = 56.13;
                break;
            case Products.VideresalgVand:
                ProductName = $"Videresalg af vand ({referenceYear:yyyy})";
                Product = Products.VideresalgVand;
                Units = Units.m3;
                UnitNetPrice = 20.00;
                break;
            case Products.VideresalgElektricitet:
                ProductName = $"Videresalg af elektricitet ({referenceYear:yyyy})";
                Product = Products.VideresalgElektricitet;
                Units = Units.kWh;
                UnitNetPrice = 3.05;
                break;
            case Products.LejeErhvervsareal:
                ProductName = $"Leje erhvervsareal ({invoiceYear:yyyy})";
                Product = Products.LejeErhvervsareal;
                Units = Units.m2;
                UnitNetPrice = 21.31;
                break;
            case Products.LejeNyttehave:
                ProductName = $"Leje nyttehave ({invoiceYear:yyyy})";
                Product = Products.LejeNyttehave;
                Units = Units.m2;
                UnitNetPrice = 1.00;
                break;
            case Products.LejeMarkskur:
                ProductName = $"Leje markskur ({invoiceYear:yyyy})";
                Product = Products.LejeMarkskur;
                Units = Units.mdr;
                UnitNetPrice = 1000.00;
                break;
            case Products.LejeJordkælder:
                ProductName = $"Leje jordkælder ({invoiceYear:yyyy})";
                Product = Products.LejeJordkælder;
                Units = Units.m2;
                UnitNetPrice = 10.00;
                break;
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(product),
                    $"Unexpected: '{product:g}: {product:D}'/{product:X}x"
                );

            // ReSharper restore StringLiteralTypo
        }
    }
}
