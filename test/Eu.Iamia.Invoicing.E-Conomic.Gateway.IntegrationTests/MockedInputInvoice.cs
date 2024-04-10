using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eu.Iamia.Invoicing.CSVLoader;
using Eu.Iamia.Invoicing.Loader.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.IntegrationTests;
public class MockedInputInvoice : InputInvoice
{
}

public static class MockedInputInvoiceExtension
{
    public static MockedInputInvoice Valid =>
        new MockedInputInvoice
        {
            CustomerNumber = 1,
            SourceFileLineNumber = 1,
            InvoiceDate = DateTime.Today,
            PaymentTerm = 1,
            Text1 = "Text 1",
            InvoiceLines = new List<IInputLine>{
                new InputLine
                {
                    UnitNetPrice = 1,
                    SourceFileLineNumber = 1,
                    UnitNumber = 1,
                    Description = "Line description",
                    ProductNumber = "1",
                    Quantity = 10.0,
                    SourceFileLine = 2,
                    UnitText = "Unit text"
                }
            }
        };
}
