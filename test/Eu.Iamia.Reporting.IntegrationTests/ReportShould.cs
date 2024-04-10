using Eu.Iamia.Reporting.Configuration;
using Eu.Iamia.Reporting.Contract;

namespace Eu.Iamia.Reporting.IntegrationTests;

public class ReportShould
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
     * A: processing invoices
     * Filename should reflect reference and time of execution optionally state (error/info)
     * "nnnn_ffff-llll_MM-dd_HH-mm-ss_[I/E].txt"
     * The Info/Error suffix is updated after the file is closed.
     *
     * B: processing aggregate report
     * "{report type}_MM-dd_HH-mm-ss.txt"
     */

    public ReportShould()
    {
        using var setup = new Setup();

        var settings = setup.GetSetting<SettingsForReporting>();

        _sut = new CustomerReportForTesting(settings);
    }

    [Fact]
    public void Test1()
    {
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