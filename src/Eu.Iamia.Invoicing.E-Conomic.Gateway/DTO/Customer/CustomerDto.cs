﻿using System.Text.Json.Serialization;
using System.Text.Json;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
#pragma warning disable CS8618

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;

internal class CustomerDto
{
}

public class Collection
{
    public int customerNumber { get; set; }
    public string currency { get; set; }
    public PaymentTerms paymentTerms { get; set; }
    public CustomerGroup customerGroup { get; set; }
    public string address { get; set; }
    public double balance { get; set; }
    public double dueAmount { get; set; }
    public string corporateIdentificationNumber { get; set; }
    public string city { get; set; }
    public string country { get; set; }
    public string email { get; set; }
    public string name { get; set; }
    public string zip { get; set; }
    public string telephoneAndFaxNumber { get; set; }
    public VatZone vatZone { get; set; }
    public DateTime lastUpdated { get; set; }
    public string contacts { get; set; }
    public Templates templates { get; set; }
    public Totals totals { get; set; }
    public string deliveryLocations { get; set; }
    public Invoices invoices { get; set; }
    public bool eInvoicingDisabledByDefault { get; set; }
    public string self { get; set; }
}

public class Create
{
    public string description { get; set; }
    public string href { get; set; }
    public string httpMethod { get; set; }
}

public class CustomerGroup
{
    public int customerGroupNumber { get; set; }
    public string self { get; set; }
}

public class Invoices
{
    public string drafts { get; set; }
    public string booked { get; set; }
    public string self { get; set; }
}

public class MetaData
{
    public Create create { get; set; }
}

public class Pagination
{
    public int skipPages { get; set; }
    public int pageSize { get; set; }
    public int maxPageSizeAllowed { get; set; }
    public int results { get; set; }
    public int resultsWithoutFilter { get; set; }
    public string firstPage { get; set; }
    public string previousPage { get; set; }
    public string lastPage { get; set; }
}

public class PaymentTerms
{
    public int paymentTermsNumber { get; set; }
    public string self { get; set; }
}

public class CustomersHandle
{
    public List<Collection> collection { get; set; }
    public Pagination pagination { get; set; }
    public MetaData metaData { get; set; }
    public string self { get; set; }
}

public class Templates
{
    public string invoice { get; set; }
    public string invoiceLine { get; set; }
    public string self { get; set; }
}

public class Totals
{
    public string drafts { get; set; }
    public string booked { get; set; }
    public string self { get; set; }
}

public class VatZone
{
    public int vatZoneNumber { get; set; }
    public string self { get; set; }
}

public static class CustomersHandleExtension
{
    public static string ToJson(this CustomersHandle customersHandle)
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Converters = { new JsonStringEnumConverter() }
        };

        var json = JsonSerializer.Serialize(customersHandle, options);
        return json;
    }

    public static CustomersHandle? FromJson(string json)
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Converters = { new JsonStringEnumConverter() }
        };

        var invoice = JsonSerializer.Deserialize<CustomersHandle>(json, options);
        return invoice;
    }

}
