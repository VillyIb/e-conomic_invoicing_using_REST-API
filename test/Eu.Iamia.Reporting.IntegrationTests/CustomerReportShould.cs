using Eu.Iamia.Reporting.Configuration;
using Eu.Iamia.Reporting.Contract;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Reporting.IntegrationTests;

public class CustomerReportShould
{
    private readonly CustomerReportForTesting _sut;

    private const string Alfa = "{\"message\":\"Validation failed. 2 errors found.\",\"errorCode\":\"E04300\", \"developerHint\":\"Inspect validation errors and correct your request.\", \"logId\":\"86d2a1f150c392bb-CPH\", \"httpStatusCode\":400,\"errors\":{ \"paymentTerms\":{\"errors\":[{\"propertyName\":\"paymentTerms\",\"errorMessage\":\"PaymentTerms '4711' not found.\",\"errorCode\":\"E07080\",\"inputValue\":4711,\"developerHint\":\"Find a list of paymentTermss at https://restapi.e-conomic.com/payment-terms .\"}]}, \"paymentTermsType\":{\"errors\":[{\"propertyName\":\"paymentTermsType\",\"errorMessage\":\"Payment terms type does not match the type on the payment terms specified.\", \"errorCode\":\"E07180\",\"inputValue\":\"invoiceMonth\",\"developerHint\":\"Either specify the matching payment terms type for the payment terms in question, or omit the property.\"}]}},\"logTime\":\"2024-03-31T21:09:13\",\"errorCount\":2}";

    private const string Bravo = "{'message':'Validation failed. 2 errors found.','errorCode':'E04300', 'developerHint':'Inspect validation errors and correct your request.', 'logId':'86d2a1f150c392bb-CPH', 'httpStatusCode':400,'errors':{ 'paymentTerms':{'errors':[{'propertyName':'paymentTerms','errorMessage':'PaymentTerms '4711' not found.','errorCode':'E07080','inputValue':4711,'developerHint':'Find a list of paymentTermss at https://restapi.e-conomic.com/payment-terms .'}]}, 'paymentTermsType':{'errors':[{'propertyName':'paymentTermsType','errorMessage':'Payment terms type does not match the type on the payment terms specified.', 'errorCode':'E07180','inputValue':'invoiceMonth','developerHint':'Either specify the matching payment terms type for the payment terms in question, or omit the property.'}]}},'logTime':'2024-03-31T21:09:13','errorCount':2}";

    private const string Charlie = "ccccccccc";

    /*
     * Expected behaviour.
     *
     * Filename should be informative about content.
     *
     * Filename should reflect reference and time of execution optionally state (error/info)
     * "nnnn_ffff-llll_MM-dd_HH-mm-ss_[I/E].txt"
     * The Info/Error suffix is updated after the file is closed.
     *
     */

    private ICustomer GetCustomer()
    {
        return new Customer
        {
            Name = "firstname lastname",
            CustomerNumber = 99,
        };
    }

    private ICustomer GetCustomerWithoutName()
    {
        return new Customer
        {
            Name = null,
            CustomerNumber = 99999,
        };
    }


    private readonly SettingsForReporting _settings;

        public CustomerReportShould()
    {
        using var setup = new Setup();

        _settings = setup.GetSetting<SettingsForReporting>();

        _settings.FilNameFormat = "yyyy-MM-dd_hh-mm-ss";

        _sut = new CustomerReportForTesting(_settings);
    }

    [Fact]
    public void Given_CustomerWithName_When_Error_CreateRightFilename()
    {
        /*
         * Filename should reflect reference and time of execution optionally state (error/info)
         * "nnnn_ffff-llll_MM-dd_HH-mm-ss_[I/E].txt"*
         */
        var timestamp = DateTime.Now;
        var customer = GetCustomer();

        _sut.Setup(customer);
        _sut.Create(timestamp);


        var nameParts = customer.Name.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var customerPart = $"{customer.CustomerNumber.ToString().TrimNumberToLength(4)}_{nameParts.First().TrimToLength(4,'f')}-{nameParts.Last().TrimToLength(4,'l')}";
        var timePart = timestamp.ToString("yyyy-MM-dd_hh-mm-ss");

        var expectedFilename = $"{customerPart}_{timePart}_E.txt";

        Assert.False(_sut.Exists(expectedFilename));
        _sut.Error("Alfa", Alfa);
        _sut.Close();
        Assert.True(_sut.Exists(expectedFilename));
    }

    [Fact]
    public void Given_CustomerWithOutName_When_Error_CreateRightFilename()
    {

    }



    [Fact]
    public void Test1()
    {
        _sut.Setup(GetCustomer());
        _sut.Create(DateTime.Now);

        _sut.Error("Alfa", Alfa);
        _sut.Info("Charlie", Charlie);
        _sut.Error("Bravo", Bravo);

        _sut.Close();

        Assert.True(_sut.Exists());
        var content = _sut.GetContent();
        var parts = content.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        Assert.True(parts[0].Contains("Alfa"));
    }

    // info should not leave file.
}

public class Customer : ICustomer
{
    public int CustomerNumber { get; init; }

    public string? Name { get; init; }
}