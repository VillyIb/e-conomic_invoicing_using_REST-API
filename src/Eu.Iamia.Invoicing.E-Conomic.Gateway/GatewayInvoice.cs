using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Eu.Iamia.Invoicing.Loader.Contract;
using System.Text;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.DraftInvoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Mapping;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;

public partial class GatewayBase
{
    // see:http://restdocs.e-conomic.com/#post-invoices-drafts

    public async Task<string> ReadInvoice()
    {
        try
        {
            SetAuthenticationHeaders();

            var response = await _httpClient.GetAsync("https://restapi.e-conomic.com/invoices/drafts/340");
            response.EnsureSuccessStatusCode();
            var htmlBody = await GetHtmlBody(response);
            return htmlBody;
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }

    // TODO return a more explicit status code !
    internal async Task<DraftInvoice?> PushInvoice(Invoice invoice, int sourceFileLineNumber)
    {
        SetAuthenticationHeaders();

        var json = invoice.ToJson();
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://restapi.e-conomic.com/invoices/drafts", content);


        if (!response.IsSuccessStatusCode)
        {
           var htmlBodyFail = await GetHtmlBody(response);

            var prettyJson = htmlBodyFail.JsonPrettify();
            Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}");
            Console.WriteLine($"Failing on input line # {sourceFileLineNumber}" );
            Console.WriteLine(prettyJson);

            // TODO return informative information.
            // A: return Either<DraftInvoice;ErrorMessage>
            // B: throw exception with ErrorMessage
            // C: Log error message and throw HttpRequestException
            // D: Log error and return null
            // E: Log to console and return null

            var sourceFileLineNumberToErrorMessag3 = sourceFileLineNumber;
            // error
            var x = invoice.Customer;
            response.EnsureSuccessStatusCode();
        }

        var htmlBody = await GetHtmlBody(response);

        var draftInvoice = DraftInvoiceExtensions.FromJson(htmlBody);

        return draftInvoice;
    }

    private Mapper? _mapper;

    private Mapper Mapper => _mapper ??= new Mapper(_settings, CustomerCache, ProductCache);

    public async Task<IDraftInvoice?> PushInvoice(IInputInvoice inputInvoice, int sourceFileLineNumber)
    {

        var economicInvoice = Mapper.From(inputInvoice);

        if (economicInvoice == null)
        {
            throw new ArgumentException($"Unable to map inputInvoice {inputInvoice.CustomerNumber}{Environment.NewLine}, Source file line: {inputInvoice.SourceFileLineNumber}");
        }

        var status = await PushInvoice(economicInvoice, sourceFileLineNumber);
        return status;
    }


}
