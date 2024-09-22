﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using Eu.Iamia.Invoicing.Application.Contract.DTO;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.draftInvoiceNumber.lines.post;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.post;
using Eu.Iamia.Invoicing.Mapping.Caches;
using Eu.Iamia.Reporting.Contract;

namespace Eu.Iamia.Invoicing.Mapping;

public interface IMappingService
{
    /// <summary>
    /// Returns result by CustomerDtoCache.
    /// </summary>
    /// <param name="customerGroupsToAccept"></param>
    /// <returns>CustomerDtoCache</returns>
    Task<int> LoadCustomerCache(IList<int> customerGroupsToAccept);

    Task<int> LoadProductCache();

    ReadOnlyCollection<ProductDto> ProductDtoCache { get; }

    ReadOnlyCollection<CustomerDto> CustomerDtoCache { get; }

    Task<int> LoadPaymentTermCache();

    Task<IDraftInvoice?> PushInvoice(
        Application.Contract.DTO.InvoiceDto invoiceDto,
        int layoutNumber,
        int sourceFileLineNumber,
        CancellationToken cancellationToken
    );
}

public class MappingService : IMappingService
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

    private readonly CustomerDtoCache _customersCache = new();

    public async Task<int> LoadCustomerCache(IList<int> customerGroupsToAccept)
    {
        _customersCache.Clear();

        var cts = new CancellationTokenSource();
        bool @continue = true;
        var page = 0;
        while (@continue)
        {
            var customersHandle = await _economicGateway.ReadCustomers(page, 20, cts.Token);
            foreach (var collection in customersHandle.Customers)
            {
                if(collection.customerNumber == 3) Debugger.Break();

                if (!customerGroupsToAccept.Any(cg => cg.Equals(collection.customerGroup.customerGroupNumber))) 
                    continue;

                var customerDto = collection.ToCustomerDto();
                _customersCache.Add(customerDto);
            }
            @continue = customersHandle.Customers.Any() && page < 100;
            page++;
        }

        return _customersCache.Count;
    }

    public ReadOnlyCollection<CustomerDto> CustomerDtoCache => new ReadOnlyCollection<CustomerDto>(_customersCache);

    private readonly ProductDtoCache _productsCache = [];

    public async Task<int> LoadProductCache()
    {
        _productsCache.Clear();

        var cts = new CancellationTokenSource();
        bool @continue = true;
        var page = 0;
        while (@continue)
        {
            var productsHandle = await _economicGateway.ReadProducts(page, 20, cts.Token);
            foreach (var collection in productsHandle.Products)
            {
                var productDto = collection.ToProductDto();
                _productsCache.Add(productDto);
            }
            @continue = productsHandle.Products.Any() && page < 100;
            page++;
        }

        return _productsCache.Count;
    }

    public ReadOnlyCollection<ProductDto> ProductDtoCache => new ReadOnlyCollection<ProductDto>(_productsCache);


    public async Task<int> LoadPaymentTermCache()
    {
        return await _economicGateway.LoadPaymentTermsCache();
    }

    //private readonly PaymentTermDtoCache _paymentTermCache = [];

    //public async Task<int> LoadPaymentTermCache()
    //{
    //    _paymentTermCache.Clear();

    //    var cts = new CancellationTokenSource();
    //    bool @continue = true;
    //    var page = 0;
    //    while (@continue)
    //    {
    //        var paymentTermHandle = await _economicGateway.ReadPaymentTerms(page, 20, cts.Token);
    //        foreach (var c in paymentTermHandle.Products)
    //        {
    //            var paymentDto = c.ToPaymentTermDto();
    //            _paymentTermCache.Add(paymentDto);
    //        }
    //        @continue = paymentTermHandle.Products.Any() && page < 100;
    //        page++;
    //    }
    //    return _paymentTermCache.Count;
    //}

    /// <summary>
    /// Outgoing CustomerDto, InvoiceDto, ProductDto to RestApi-Invoice.
    /// </summary>
    /// <param name="customerDto"></param>
    /// <param name="invoiceDto"></param>
    /// <param name="productDtoCache"></param>
    /// <param name="layoutNumber"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    private Invoice ToRestApiInvoice(
        CustomerDto customerDto,
        InvoiceDto invoiceDto,
        ProductDtoCache productDtoCache,
        int layoutNumber
        )
    {
        var paymentTerm1 = _economicGateway.GetPaymentTerm(invoiceDto.PaymentTerm ?? customerDto.PaymentTerms);

        if (paymentTerm1 is null)
        {
            throw new ApplicationException($"PaymentTerm: '{customerDto.PaymentTerms}' not found in e-conomic");
        }

        var paymentTerms = paymentTerm1.ToInvoice();

        var layout = new Layout() { LayoutNumber = layoutNumber };

        var notes = new Notes()
        {
            Heading = $"#{customerDto.CustomerNumber} {customerDto.Name}",
            TextLine1 = invoiceDto.Text1
            //TextLine2 = "Text2.1\nText2.2\nText2.3"
        };

        var recipient = new Recipient()
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
        };

        var references = new References()
        {
            //Other = "references-other"
        };

        var customer = new Customer(customerDto.CustomerNumber);

        var invoice = new Invoice
        {
            Customer = customer,
            Date = invoiceDto.InvoiceDate.ToString("yyyy-MM-dd"),
            //ExchangeRate = 100,
            //Delivery = new(
            //"delivery-address"
            //, "delivery-zip"
            //, "delivery-city"
            //, "delivery-country"
            //, DateTime.Today
            //),
            Layout = layout,
            Notes = notes,
            Recipient = recipient,
            References = references,
            PaymentTerms = paymentTerms
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
                : new Unit(
                    name: productDto.Unit.Name,
                    unitNumber: productDto.Unit.UnitNumber
                );

            var product = new Product()
            {
                ProductNumber = invoiceLineDto.ProductNumber
            };
            var line = new Line()
            {
                Description = invoiceLineDto.Description,
                LineNumber = lineNumber,
                Product = product,
                Quantity = invoiceLineDto.Quantity!.Value,
                SortKey = lineNumber,
                Unit = unit,
                UnitNetPrice = invoiceLineDto.UnitNetPrice!.Value,
                // extra
                DiscountPercentage = 0.0,
                MarginInBaseCurrency = 100.0,
                MarginPercentage = 100.0,
                TotalNetAmount = 100.0,
                UnitCostPrice = 0.0
            };

            invoice.Lines.Add(line);
        }

        return invoice;
    }

    public async Task<IDraftInvoice?> PushInvoice(Application.Contract.DTO.InvoiceDto invoiceDto, int layoutNumber, int sourceFileLineNumber, CancellationToken cancellationToken)
    {
        const string reference = nameof(PushInvoice);

        var customerDto = _customersCache.GetCustomer(invoiceDto.CustomerNumber);

        _report.SetCustomer(new CustomerDto
        {
            Name = customerDto is null ? "---- ----" : customerDto.Name,
            CustomerNumber = invoiceDto.CustomerNumber
        });

        if (customerDto is null)
        {
            throw new ApplicationException($"Customer does not exist: '{invoiceDto.CustomerNumber}', Source file line: {sourceFileLineNumber}");
        }

        var restApiInvoice = ToRestApiInvoice(

             customerDto,
             invoiceDto,
             _productsCache,
             layoutNumber
        );

        var draftInvoice = await _economicGateway.PostInvoice(restApiInvoice, sourceFileLineNumber, cancellationToken);

        return draftInvoice;
    }
}
