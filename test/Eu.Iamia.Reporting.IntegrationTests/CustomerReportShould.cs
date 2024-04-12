using Eu.Iamia.Reporting.Configuration;
using Eu.Iamia.Reporting.Contract;

namespace Eu.Iamia.Reporting.IntegrationTests;

public class CustomerReportShould
{
    private readonly CustomerReportForTesting _sut;

    private const string Alfa = "{\"message\":\"Validation failed. 2 errors found.\",\"errorCode\":\"E04300\", \"developerHint\":\"Inspect validation errors and correct your request.\", \"logId\":\"86d2a1f150c392bb-CPH\", \"httpStatusCode\":400,\"errors\":{ \"paymentTerms\":{\"errors\":[{\"propertyName\":\"paymentTerms\",\"errorMessage\":\"PaymentTerms '4711' not found.\",\"errorCode\":\"E07080\",\"inputValue\":4711,\"developerHint\":\"Find a list of paymentTermss at https://restapi.e-conomic.com/payment-terms .\"}]}, \"paymentTermsType\":{\"errors\":[{\"propertyName\":\"paymentTermsType\",\"errorMessage\":\"Payment terms type does not match the type on the payment terms specified.\", \"errorCode\":\"E07180\",\"inputValue\":\"invoiceMonth\",\"developerHint\":\"Either specify the matching payment terms type for the payment terms in question, or omit the property.\"}]}},\"logTime\":\"2024-03-31T21:09:13\",\"errorCount\":2}";

    private const string Bravo = "{'message':'Validation failed. 2 errors found.','errorCode':'E04300', 'developerHint':'Inspect validation errors and correct your request.', 'logId':'86d2a1f150c392bb-CPH', 'httpStatusCode':400,'errors':{ 'paymentTerms':{'errors':[{'propertyName':'paymentTerms','errorMessage':'PaymentTerms '4711' not found.','errorCode':'E07080','inputValue':4711,'developerHint':'Find a list of paymentTermss at https://restapi.e-conomic.com/payment-terms .'}]}, 'paymentTermsType':{'errors':[{'propertyName':'paymentTermsType','errorMessage':'Payment terms type does not match the type on the payment terms specified.', 'errorCode':'E07180','inputValue':'invoiceMonth','developerHint':'Either specify the matching payment terms type for the payment terms in question, or omit the property.'}]}},'logTime':'2024-03-31T21:09:13','errorCount':2}";


    /*
     * Expected behaviour.
     *
     * Filename should be informative about content.
     *
     * Filename should reflect reference and time of execution optionally state (error/info)
     * "nnnn_ffff-llll_MM-dd_HH-mm-ss_[I/E]_name.txt"
     * The Customer can be updated multiple times, eg. name can be added.
     * The file is not created until the firste write, then the filename is locked. Futher calls to 
     * The Info/Error suffix is updated after the file is closed.
     *
     */

    private ICustomer GetCustomer(int id = 99)
    {
        return new Customer
        {
            Name = "firstname lastname",
            CustomerNumber = id,
        };
    }

    private ICustomer GetCustomerWithoutName(int id = 9999)
    {
        return new Customer
        {
            Name = null,
            CustomerNumber = id,
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
        _settings.DiscardNonErrorLogfiles = false;

        _sut = new CustomerReportForTesting(_settings);
    }

    [Fact]
    public void Given_CustomerWithName_When_Error_CreateErrorFilename()
    {
        const int customerId = 13;

        _sut.SetCustomer(GetCustomerWithoutName(customerId));

        _sut.Error("Alfa", Alfa);
        _sut.Close();

        var customerPart = $"__{customerId}_ffff-llll";
        var timePart = _sut.GetTimeStamp()!.Value.ToString("yyyy-MM-dd_HH-mm-ss");
        var expectedErrorFilename = Path.Combine(_settings.OutputDirectory, $"{customerPart}_{timePart}_E_CustomerReportShould.txt");

        Assert.True(_sut.Exists(expectedErrorFilename));
        _sut.DeleteFile(expectedErrorFilename);
    }

    [Fact]
    public void Given_CustomerWithName_When_Info_LeaveInfoFilename()
    {
        const int customerId = 17;

        _sut.SetCustomer(GetCustomerWithoutName(customerId));

        _sut.Info("Alfa", Alfa);
        _sut.Close();

        var customerPart = $"__{customerId}_ffff-llll";
        var timePart = _sut.GetTimeStamp()!.Value.ToString("yyyy-MM-dd_HH-mm-ss");
        var expectedInfoFilename = Path.Combine(_settings.OutputDirectory, $"{customerPart}_{timePart}_I_CustomerReportShould.txt");

        Assert.True(_sut.Exists(expectedInfoFilename));
        _sut.DeleteFile(expectedInfoFilename);

    }

    [Fact]
    public void Given_CustomerWithOutName_When_Error_LeaveDefaultErrorFilename()
    {
        const int customerId = 31;

        _sut.SetCustomer(GetCustomerWithoutName(customerId));
        _sut.Error("Alfa", Alfa);
        _sut.Close();

        var customerPart = $"__{customerId}_ffff-llll";
        var timePart = _sut.GetTimeStamp()!.Value.ToString("yyyy-MM-dd_HH-mm-ss");
        var expectedDefaultErrorFilename = Path.Combine(_settings.OutputDirectory, $"{customerPart}_{timePart}_E_CustomerReportShould.txt");

        Assert.True(_sut.Exists(expectedDefaultErrorFilename));
        _sut.DeleteFile(expectedDefaultErrorFilename);

    }

    /// <summary>
    /// Updating the customer after the file is created is ignored.
    /// </summary>
    [Fact]
    public void Given_FileIsCreated_When_SetCustomer_CustomerIsUnchanged()
    {
        const int customerId = 71;

        var customerWithoutName = GetCustomerWithoutName(customerId);
        _sut.SetCustomer(customerWithoutName);
        _sut.Error("Alfa", Alfa);
        _sut.SetCustomer(GetCustomer(customerId));

        Assert.Equal(customerWithoutName.Name, _sut.GetCustomerName());
        Assert.Equal(customerWithoutName.CustomerNumber, _sut.GetCustomerNumber());

        _sut.Close();

        var customerPart = $"__{customerId}_ffff-llll";
        var timePart = _sut.GetTimeStamp()!.Value.ToString("yyyy-MM-dd_HH-mm-ss");
        var expectedTemporaryFilename = Path.Combine(_settings.OutputDirectory, $"{customerPart}_{timePart}_E_CustomerReportShould.txt");

        Assert.True(_sut.Exists(expectedTemporaryFilename));
        _sut.DeleteFile(expectedTemporaryFilename);
    }

    /// <summary>
    /// Updating the customer to a another customer has new customer values.
    /// </summary>
    [Fact]
    public void Given_FileIsNotCreated_When_SetCustomer_CustomerIsChanged()
    {
        const int customerId = 73;

        var customerWithoutName = GetCustomerWithoutName(customerId);
        _sut.SetCustomer(customerWithoutName);

        Assert.False(_sut.IsOpenForWriting);

        var customerWithName = GetCustomer(customerId);
        _sut.SetCustomer(customerWithName);

        Assert.Equal(customerWithName.Name, _sut.GetCustomerName());
        Assert.Equal(customerWithName.CustomerNumber, _sut.GetCustomerNumber());
    }

    [Fact]
    public void Given_file_is_created_written_closed_opened_written_AllTextIsRetained()
    {
        const int customerId = 79;

        var customerWithoutName = GetCustomerWithoutName(customerId);
        _sut.SetCustomer(customerWithoutName);
        _sut.Error("Alfa", Alfa);

        _sut.Close();

        _sut.Info("Bravo", Bravo);
        _sut.Close();

        var content = _sut.GetContent();
        var lines = content.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        {
            var asserts = new List<Action<string>>
            {
                state => Assert.Equal("Error: Alfa", state),
                state => Assert.Equal("{", state),
                state => Assert.Equal("\"message\": \"Validation failed. 2 errors found.\",", state),
                state => Assert.Equal("\"errorCode\": \"E04300\",", state),
                state => Assert.Equal("\"developerHint\": \"Inspect validation errors and correct your request.\",", state),
            };

            Assert.Collection(lines.Take(5), asserts.ToArray());
        }

        {
            var asserts = new List<Action<string>>
            {
                state => Assert.Equal("Info: Bravo", state),
            };

            Assert.Collection(lines.Skip(34).Take(1), asserts.ToArray());
        }

        var customerPart = $"__{customerId}_ffff-llll";
        var timePart = _sut.GetTimeStamp()!.Value.ToString("yyyy-MM-dd_HH-mm-ss");
        var expectedTemporaryFilename = Path.Combine(_settings.OutputDirectory, $"{customerPart}_{timePart}_E_CustomerReportShould.txt");

        Assert.True(_sut.Exists(expectedTemporaryFilename));
        _sut.DeleteFile(expectedTemporaryFilename);
    }

    [Fact]
    public void Given_DiscardNonErrors_is_True_When_Info_LeavesNoFile()
    {
        _settings.DiscardNonErrorLogfiles = true;
        const int customerId = 37;

        _sut.SetCustomer(GetCustomer(customerId));

        _sut.Info("Alfa", Alfa);
        _sut.Close();

        var customerPart = $"__{customerId}_firs-last";
        var timePart = _sut.GetTimeStamp()!.Value.ToString("yyyy-MM-dd_HH-mm-ss");
        var expectedTemporaryFilename = Path.Combine(_settings.OutputDirectory, $"{customerPart}_{timePart}_I_CustomerReportShould.txt");

        Assert.False(_sut.Exists(expectedTemporaryFilename));
        _sut.DeleteFile(expectedTemporaryFilename);
    }

    [Fact]
    public void Given_Alfa_as_Key_and_Body_When_Error_File_Contains_ExpectedText()
    {
        const int customerId = 41;

        _sut.SetCustomer(GetCustomer(customerId));

        _sut.Error("Alfa", Alfa);
        _sut.Close();

        var customerPart = $"__{customerId}_firs-last";
        var timePart = _sut.GetTimeStamp()!.Value.ToString("yyyy-MM-dd_HH-mm-ss");
        var expectedErrorFilename = Path.Combine(_settings.OutputDirectory, $"{customerPart}_{timePart}_E_CustomerReportShould.txt");

        Assert.True(_sut.Exists(expectedErrorFilename));

        var content = _sut.GetContent();
        var lines = content
            .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Take(5)
        ;

        var asserts = new List<Action<string>>
        {
            state => Assert.Equal("Error: Alfa", state),
            state => Assert.Equal("{", state),
            state => Assert.Equal("\"message\": \"Validation failed. 2 errors found.\",", state),
            state => Assert.Equal("\"errorCode\": \"E04300\",", state),
            state => Assert.Equal("\"developerHint\": \"Inspect validation errors and correct your request.\",", state)
        };

        Assert.Collection(lines, asserts.ToArray());

        _sut.DeleteFile(expectedErrorFilename);
    }
}

public class Customer : ICustomer
{
    public int CustomerNumber { get; init; }

    public string? Name { get; init; }
}