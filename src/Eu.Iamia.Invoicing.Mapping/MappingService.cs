using Eu.Iamia.Invoicing.Application.Contract.DTO;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
using Eu.Iamia.Invoicing.Mapping.Caches;
using Eu.Iamia.Reporting.Contract;

namespace Eu.Iamia.Invoicing.Mapping;

public interface IMappingService
{
    Task LoadCustomerCache(IList<int> customerGroupsToAccept);

    Task LoadProductCache();

    Task<IDraftInvoice?> PushInvoice(
        Application.Contract.DTO.InvoiceDto invoiceDto, 
        int layoutNumber, 
        int sourceFileLineNumber, 
        CancellationToken cancellationToken
    );
}

public  class MappingService : IMappingService
{
    private readonly IEconomicGatewayV2 _economicGateway;
    private readonly ICustomerReport _report;

    public MappingService(
        IEconomicGatewayV2 economicGateway,
        ICustomerReport report
    )
    {
        _economicGateway = economicGateway;
        _report = report;
    }

    private readonly CustomerDtoCache _customersCache = new ();

    public async Task LoadCustomerCache(IList<int> customerGroupsToAccept)
    {
        _customersCache.Clear();

        var cts = new CancellationTokenSource();
        bool @continue = true;
        var page = 0;
        while (@continue)
        {
            var customersHandle = await _economicGateway.ReadCustomersPaged(page, 20, cts.Token);
            foreach (var collection in customersHandle.collection)
            {
                if (!customerGroupsToAccept.Any(cg => cg.Equals(collection.customerGroup.customerGroupNumber))) continue;

                var customerDto = collection.ToCustomerDto();
                _customersCache.Add(customerDto);
            }
            @continue = customersHandle.collection.Any() && page < 100;
            page++;
        }
    }

    private readonly ProductDtoCache _productsCache = new();

    public async Task LoadProductCache()
    {
        _productsCache.Clear();

        var cts = new CancellationTokenSource();
        bool @continue = true;
        var page = 0;
        while (@continue)
        {
            var productsHandle = await _economicGateway.ReadProductsPaged(page, 20, cts.Token);
            foreach (var collection in productsHandle.collection)
            {
                var productDto = collection.ToProductDto();
                _productsCache.Add(productDto);
            }
            @continue = productsHandle.collection.Any() && page < 100;
            page++;
        }
    }

    /// <summary>
    /// Outgoing CustomerDto, InvoiceDto, ProductDto to RestApi-Invoice.
    /// </summary>
    /// <param name="customerDto"></param>
    /// <param name="invoiceDto"></param>
    /// <param name="productDtoCache"></param>
    /// <param name="layoutNumber"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public static E_Conomic.Gateway.Contract.DTO.Invoice.Invoice ToRestApiInvoice(
        CustomerDto customerDto,
        InvoiceDto invoiceDto,
        ProductDtoCache productDtoCache,
        int layoutNumber
        )
    {
        var inputInvoiceInvoiceDate = DateTime.MaxValue;

        var invoice = new E_Conomic.Gateway.Contract.DTO.Invoice.Invoice
        {
            Customer = new E_Conomic.Gateway.Contract.DTO.Invoice.Customer(customerDto.CustomerNumber),
            Date = inputInvoiceInvoiceDate.ToString("yyyy-MM-dd"),
            //ExchangeRate = 100,
            //Delivery = new(
            //"delivery-address"
            //, "delivery-zip"
            //, "delivery-city"
            //, "delivery-country"
            //, DateTime.Today
            //),
            Layout = new() { LayoutNumber = layoutNumber },
            Notes = new()
            {
                Heading = $"#{customerDto.CustomerNumber} {customerDto.Name}",
                TextLine1 = invoiceDto.Text1
                //TextLine2 = "Text2.1\nText2.2\nText2.3"
            },
            Recipient = new()
            {
                Address = $"{customerDto.Address}",
                City = $"{customerDto.City}",
                Zip = $"{customerDto.Zip}",
                Name = $"{customerDto.Name}",
                VatZone = new()
                {
                    EnabledForCustomer = true,
                    EnabledForSupplier = true,
                    Name = "Domestic",
                    VatZoneNumber = 1 // Hardcoded value
                }
            },
            References = new()
            {
                //Other = "references-other"
            },
            PaymentTerms = new()
            {
                //DaysOfCredit = 14
                //,
                //PaymentTermsNumber = customerDto.PaymentTerms,
                PaymentTermsNumber = customerDto.PaymentTerms,
                //,
                //Name = "Lb. md. 14 dage"
                //,
                //PaymentTermsType = PaymentTermsType.invoiceMonth
            }
        };

        foreach (var invoiceLineDto in invoiceDto.InvoiceLines)
        {
            const int lineNumber = 1;

            var productDto = productDtoCache.GetProduct(invoiceLineDto.ProductNumber);

            if (productDto is null)
            {
                throw new ApplicationException($"Product: '{invoiceLineDto.ProductNumber}' not found in e-conomic, Source file line: {invoiceLineDto.SourceFileLineNumber}");
            }

            var unit = productDto.Unit is null
                ? null
                : new E_Conomic.Gateway.Contract.DTO.Invoice.Unit(
                    name: productDto.Unit.Name,
                    unitNumber: productDto.Unit.UnitNumber
                );

            var line = new E_Conomic.Gateway.Contract.DTO.Invoice.Line()
            {
                Description = invoiceLineDto.Description,
                LineNumber = lineNumber,
                Product = new E_Conomic.Gateway.Contract.DTO.Invoice.Product()
                {
                    ProductNumber = invoiceLineDto.ProductNumber
                },
                Quantity = invoiceLineDto.Quantity!.Value,
                SortKey = lineNumber,
                Unit = unit,
                UnitNetPrice = invoiceLineDto.UnitNetPrice!.Value,
            };

            invoice.Lines.Add(line);
        }

        return invoice;
    }


    public async Task<IDraftInvoice?> PushInvoice(Application.Contract.DTO.InvoiceDto invoiceDto, int layoutNumber, int sourceFileLineNumber, CancellationToken cancellationToken)
    {
        const string reference = nameof(PushInvoice);

        var customerDto = _customersCache.GetCustomer(invoiceDto.CustomerNumber);

        _report.SetCustomer(new CachedCustomer
        {
            Name = customerDto is null ? "---- ----" : customerDto.Name,
            CustomerNumber = invoiceDto.CustomerNumber
        });

        if (customerDto is null)
        {
            throw new ApplicationException($"Customer does not exist: '{invoiceDto.CustomerNumber}', Source file line: {sourceFileLineNumber}");
        }

        var restApiInvoice =     ToRestApiInvoice(

             customerDto,
             invoiceDto,
             _productsCache,
             layoutNumber
        );

        var draftInvoice = await _economicGateway.PushInvoice(restApiInvoice, sourceFileLineNumber, cancellationToken);

        return draftInvoice;
    }
}
