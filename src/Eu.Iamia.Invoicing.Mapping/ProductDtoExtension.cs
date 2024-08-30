// ReSharper disable RedundantNameQualifier

namespace Eu.Iamia.Invoicing.Mapping;

/// <summary>
/// Mapping between Application data definition and RestApiData definition.
/// </summary>
public static class ProductDtoExtension
{
    /// <summary>
    /// Incoming RestApi-Product to ProductDto.
    /// </summary>
    /// <param name="restApiProduct"></param>
    /// <returns></returns>
    public static Application.Contract.DTO.ProductDto ToProductDto(
        this E_Conomic.Gateway.V2.Contract.DTO.Product.Product restApiProduct)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        var unitDto = restApiProduct.unit is null
                ? null
                : new Application.Contract.DTO.ProductDto.UnitDto
                {
                    Name = restApiProduct.unit.name,
                    UnitNumber = restApiProduct.unit.unitNumber
                }
            ;

        return new Application.Contract.DTO.ProductDto
        {
            Description = restApiProduct.description,
            ProductNumber = restApiProduct.productNumber,
            Name = restApiProduct.name,
            Unit = unitDto
        };
    }
}