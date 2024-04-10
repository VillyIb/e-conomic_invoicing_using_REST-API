using Eu.Iamia.Reporting.Configuration;
using Eu.Iamia.Reporting.Contract;

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

        _settings.TimePartFormat = "yyyy-MM-dd_HH-mm-ss";
        _settings.CustomerNameLength = 4;
        _settings.CustomerNumberLength = 4;
        _settings.CustomerSurnameLength = 4;
        _settings.Filename = "CustomerReportShould.txt";
        //_settings.DiscardNonErrors = true;

        _sut = new CustomerReportForTesting(_settings);
    }

    [Fact]
    public void Given_CustomerWithName_When_Error_CreateRightFilename()
    {
        var timestamp = DateTime.Now;
        var customer = GetCustomer();

        _sut.Setup(customer);
        _sut.Create(timestamp);

        var customerPart = $"__99_firs-last";
        var timePart = timestamp.ToString("yyyy-MM-dd_HH-mm-ss");

        var expectedFilename = Path.Combine(_settings.OutputDirectory, $"{customerPart}_{timePart}_E_CustomerReportShould.txt");

        Assert.False(_sut.Exists(expectedFilename));
        _sut.Error("Alfa", Alfa);
        _sut.Close();
        Assert.True(_sut.Exists(expectedFilename));
    }

    [Fact]
    public void Given_CustomerWithName_When_Info_CreateRightFilename()
    {
        var timestamp = DateTime.Now;
        var customer = GetCustomer();

        _sut.Setup(customer);
        _sut.Create(timestamp);

        var customerPart = $"__99_firs-last";
        var timePart = timestamp.ToString("yyyy-MM-dd_HH-mm-ss");

        var expectedFilename = Path.Combine(_settings.OutputDirectory, $"{customerPart}_{timePart}_I_CustomerReportShould.txt");

        Assert.True(_sut.Exists(expectedFilename));
        _sut.Info("Alfa", Alfa);
        _sut.Close();
        Assert.True(_sut.Exists(expectedFilename));
    }

    [Fact]
    public void Given_CustomerWithOutName_When_Error_CreateDefaultFilename()
    {
        var timestamp = DateTime.Now;
        var customer = GetCustomerWithoutName();

        _sut.Setup(customer);
        _sut.Create(timestamp);

        var customerPart = $"9999_ffff-llll";
        var timePart = timestamp.ToString("yyyy-MM-dd_HH-mm-ss");

        var expectedFilename = Path.Combine(_settings.OutputDirectory, $"{customerPart}_{timePart}_E_CustomerReportShould.txt");

        Assert.False(_sut.Exists(expectedFilename));
        _sut.Error("Alfa", Alfa);
        _sut.Close();
        Assert.True(_sut.Exists(expectedFilename));
    }

    [Fact]
    public void Given_ClosedReport_When_Info_ThrowsException()
    {
        var timestamp = DateTime.Now;
        var customer = GetCustomer();

        _sut.Setup(customer);
        _sut.Create(timestamp);
        _sut.Close();

        _ = Assert.Throws<ApplicationException>(() => _sut.Info("Alfa", Alfa));
    }

    [Fact]
    public void Given_DiscardNonErrors_is_True_When_Info_LeavesNoFile()
    {
        _settings.DiscardNonErrors = true;

        var timestamp = DateTime.Now;
        var customer = GetCustomer();

        _sut.Setup(customer);
        _sut.Create(timestamp);

        var customerPart = $"__99_firs-last";
        var timePart = timestamp.ToString("yyyy-MM-dd_HH-mm-ss");

        var expectedFilename = Path.Combine(_settings.OutputDirectory, $"{customerPart}_{timePart}_I_CustomerReportShould.txt");

        Assert.True(_sut.Exists(expectedFilename));
        _sut.Info("Alfa", Alfa);
        _sut.Close();
        Assert.False(_sut.Exists(expectedFilename));
    }
}

public class Customer : ICustomer
{
    public int CustomerNumber { get; init; }

    public string? Name { get; init; }
}