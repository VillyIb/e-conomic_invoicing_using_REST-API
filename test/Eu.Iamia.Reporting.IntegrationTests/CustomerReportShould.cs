using Eu.Iamia.Reporting.Configuration;
using Eu.Iamia.Reporting.Contract;

namespace Eu.Iamia.Reporting.IntegrationTests;

public class CustomerReportShould
{
    private readonly CustomerReportForTesting _sut;

    private const string Alfa = "{\"message\":\"Validation failed. 2 errors found.\",\"errorCode\":\"E04300\", \"developerHint\":\"Inspect validation errors and correct your request.\", \"logId\":\"86d2a1f150c392bb-CPH\", \"httpStatusCode\":400,\"errors\":{ \"paymentTerms\":{\"errors\":[{\"propertyName\":\"paymentTerms\",\"errorMessage\":\"PaymentTerms '4711' not found.\",\"errorCode\":\"E07080\",\"inputValue\":4711,\"developerHint\":\"Find a list of paymentTermss at https://restapi.e-conomic.com/payment-terms .\"}]}, \"paymentTermsType\":{\"errors\":[{\"propertyName\":\"paymentTermsType\",\"errorMessage\":\"Payment terms type does not match the type on the payment terms specified.\", \"errorCode\":\"E07180\",\"inputValue\":\"invoiceMonth\",\"developerHint\":\"Either specify the matching payment terms type for the payment terms in question, or omit the property.\"}]}},\"logTime\":\"2024-03-31T21:09:13\",\"errorCount\":2}";

    private const string Bravo = "{'message':'Validation failed. 2 errors found.','errorCode':'E04300', 'developerHint':'Inspect validation errors and correct your request.', 'logId':'86d2a1f150c392bb-CPH', 'httpStatusCode':400,'errors':{ 'paymentTerms':{'errors':[{'propertyName':'paymentTerms','errorMessage':'PaymentTerms '4711' not found.','errorCode':'E07080','inputValue':4711,'developerHint':'Find a list of paymentTermss at https://restapi.e-conomic.com/payment-terms .'}]}, 'paymentTermsType':{'errors':[{'propertyName':'paymentTermsType','errorMessage':'Payment terms type does not match the type on the payment terms specified.', 'errorCode':'E07180','inputValue':'invoiceMonth','developerHint':'Either specify the matching payment terms type for the payment terms in question, or omit the property.'}]}},'logTime':'2024-03-31T21:09:13','errorCount':2}";

    private ICustomer GetCustomer(int id = 99)
    {
        return new MockedCustomer
        {
            Name = "firstname lastname",
            CustomerNumber = id,
        };
    }

    private ICustomer GetCustomerWithoutName(int id = 9999)
    {
        return new MockedCustomer
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
        _settings.PruneLogfiles = false;

        _sut = new CustomerReportForTesting(_settings);
    }

    /// <summary>
    /// A logfile name is upon close changed to contain an '_E' suffix if there has been any errors written to it.
    /// </summary>
    [Fact]
    public void Given_CustomerWithName_When_Error_CreateErrorFilename()
    {
        const int customerId = 13;

        _sut.SetCustomer(GetCustomerWithoutName(customerId));

        _sut.Error("Alfa", Alfa);
        _sut.Message("Alfa", Alfa);
        _sut.Close();

        var customerPart = $"__{customerId}_ffff-llll";
        var timePart = _sut.GetTimeStamp()!.Value.ToString("yyyy-MM-dd_HH-mm-ss");
        var expectedErrorFilename = Path.Combine(_settings.OutputDirectory, $"{customerPart}_{timePart}_E_CustomerReportShould.txt");

        Assert.True(_sut.Exists(expectedErrorFilename));
        _sut.DeleteFile(expectedErrorFilename);
    }

    /// <summary>
    /// A logfile name is upon close changed to contain an '_E' suffix if there has been any errors written to it.
    /// </summary>
    [Fact]
    public void Given_CustomerWithName_When_Message_CreateMessageFilename()
    {
        const int customerId = 15;

        _sut.SetCustomer(GetCustomerWithoutName(customerId));

        _sut.Message("Alfa", Alfa);
        _sut.Info("Alfa", Alfa);
        _sut.Close();

        var customerPart = $"__{customerId}_ffff-llll";
        var timePart = _sut.GetTimeStamp()!.Value.ToString("yyyy-MM-dd_HH-mm-ss");
        var expectedMessageFilename = Path.Combine(_settings.OutputDirectory, $"{customerPart}_{timePart}_M_CustomerReportShould.txt");

        Assert.True(_sut.Exists(expectedMessageFilename));
        _sut.DeleteFile(expectedMessageFilename);
    }

    /// <summary>
    /// A logfile without any errors written to it should have an '_I' suffix.
    /// </summary>
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

    /// <summary>
    /// A logfile where the customer is unknown will have a default '_ffff-llll' name section.
    /// </summary>
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
    /// Changing the Customer section of a filename is ignored if the file is created.
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
    /// Changing the Customer section is accepted if the file isn't created.
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

    /// <summary>
    /// A logfile can be reopened for writing if then name hasn't changed.
    /// </summary>
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
        var lines = content.Split(new[] { "\r\n" }, StringSplitOptions.None).ToList();

        var elements = new List<string>
        {
            lines[1]
            , lines[2]
            , lines[3]
            , lines[36]
            , lines[37]
        };

        var asserts = new List<Action<string>>
            {
                state => Assert.Equal("Error: Alfa", state),
                state => Assert.Equal("{", state),
                state => Assert.Equal("  \"message\": \"Validation failed. 2 errors found.\",", state),
                state => Assert.Equal("Info: Bravo", state),
                state => Assert.StartsWith("{'message':'Validation failed. 2 errors", state)
            }.ToArray();


        Assert.Collection(elements, asserts);

        var customerPart = $"__{customerId}_ffff-llll";
        var timePart = _sut.GetTimeStamp()!.Value.ToString("yyyy-MM-dd_HH-mm-ss");
        var expectedTemporaryFilename = Path.Combine(_settings.OutputDirectory, $"{customerPart}_{timePart}_E_CustomerReportShould.txt");

        Assert.True(_sut.Exists(expectedTemporaryFilename));
        _sut.DeleteFile(expectedTemporaryFilename);
    }

    /// <summary>
    /// Files without any error written to it will optionally be deleted after close.
    /// </summary>
    [Fact]
    public void Given_DiscardNonErrors_is_True_When_Info_LeavesNoFile()
    {
        _settings.PruneLogfiles = true;
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

    /// <summary>
    /// Logfile must contain indication of logLevel, key and a structured message (JsonPretty).
    /// </summary>
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
            .Split(new[] { "\r\n" }, StringSplitOptions.None)
            .ToList()
        ;

        var elements = new List<string>
        {
             lines[1]
            , lines[2]
            , lines[3]
            , lines[4]
            , lines[5]
            , lines[9]
            , lines[12]
        };

        var asserts = new List<Action<string>>
        {
            state => Assert.Equal("Error: Alfa", state),
            state => Assert.Equal("{", state),
            state => Assert.Equal("  \"message\": \"Validation failed. 2 errors found.\",", state),
            state => Assert.Equal("  \"errorCode\": \"E04300\",", state),
            state => Assert.Equal("  \"developerHint\": \"Inspect validation errors and correct your request.\",", state),
            state => Assert.Equal("    \"paymentTerms\": {", state),
            state => Assert.Equal("          \"propertyName\": \"paymentTerms\",", state)
        }.ToArray();

        Assert.Collection(elements, asserts);

        _sut.DeleteFile(expectedErrorFilename);
    }
}

public class MockedCustomer : ICustomer
{
    public int CustomerNumber { get; init; }

    public string? Name { get; init; }
}