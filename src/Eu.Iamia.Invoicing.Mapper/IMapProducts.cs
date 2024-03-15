using Eu.Iamia.Invoicing.E_Conomic.Gateway;

namespace Eu.Iamia.Invoicing.Mapper;

public interface IMapProducts
{
    Product? FromInput( Loader.Contract.Products inputProducts);
}