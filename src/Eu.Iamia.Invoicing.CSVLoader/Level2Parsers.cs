using System.Globalization;

namespace Eu.Iamia.Invoicing.CSVLoader
{
    public class Level2Parsers : ILevel2Parsers
    {
        public Metadata? Metadata { get; set; }

        public int _sourceFileLineNumber { get; set; }

        private List<Application.Contract.DTO.InvoiceDto>? _invoices;

        public IList<Application.Contract.DTO.InvoiceDto> Invoices => _invoices ??= [];

        public void ParseCustomerGroupRow(IList<string> columns)
        {
            var customerGroups = columns[1];
            var cc = customerGroups.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var customerGroup in cc)
            {
                Metadata.CustomerGroupToAccept.Add(int.Parse(customerGroup));
            }
        }

        public void ParseInvoiceDateRow(IList<string> columns)
        {
            var value = columns[1];

            if (!DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime date))
            {
                throw new ArgumentException($"Unable to parse #Bilagsdato/#InvoiceDate '{value}' {Environment.NewLine}, Source file line: {_sourceFileLineNumber}");
            }
            Metadata.InvoiceDate = date;
        }

        public void ParseInvoiceRow(IList<string> columns)
        {
            if (!int.TryParse(columns[Metadata.CustomerNumberColumn], out var customerNumber) || customerNumber <= 0) return;

            var invoice = new Application.Contract.DTO.InvoiceDto
            {
                CustomerNumber = customerNumber,
                SourceFileLineNumber = _sourceFileLineNumber
            };

            foreach (var productMetadata in Metadata.ProductMetadata)
            {
                if (!double.TryParse(columns[productMetadata.Column], out var quantity) || quantity <= 0.001)
                {
                    continue;
                }

                var inputLine = new Application.Contract.DTO.InvoiceLineDto
                {
                    UnitNetPrice = productMetadata.UnitPrice,
                    Description = productMetadata.InvoiceLineText,
                    ProductNumber = productMetadata.ProductId,
                    UnitText = productMetadata.UnitText,
                    UnitNumber = productMetadata.UnitNumber,
                    Quantity = quantity,
                    SourceFileLineNumber = _sourceFileLineNumber
                };

                invoice.InvoiceLines.Add(inputLine);
            }

            Invoices.Add(invoice);
        }

        public void ParseInvoiceTextRow(IList<string> columns)
        {
            foreach (var productMetadata in Metadata.ProductMetadata)
            {
                productMetadata.InvoiceLineText = columns[productMetadata.Column];
            }
        }

        public void ParsePaymentTerm(IList<string> columns)
        {
            var value = columns[1];

            if (!int.TryParse(value, out int paymentTerm))
            {
                throw new ArgumentException($"Unable to parse #BetalingsBetingelse/ #PaymentTerm '{value}' {Environment.NewLine}Source file line: {_sourceFileLineNumber}");
            }
            Metadata.PaymentTerm = paymentTerm;
        }

        public void ParseProductsRow(IList<string> columns)
        {
            foreach (var productMetadata in Metadata.ProductMetadata)
            {
                productMetadata.ProductId = columns[productMetadata.Column];
            }
        }

        public void ParseTagsRow(IList<string> columns)
        {
            Metadata = new Metadata();

            var columnIndex = 1;
            foreach (var column in columns.Skip(1))
            {
                switch (column.ToLowerInvariant())
                {
                    case "#debitornummer":
                    case "#customernumber":
                        Metadata.CustomerNumberColumn = columnIndex;
                        break;
                    case "#produkt":
                    case "#product":
                        {
                            Metadata.ProductMetadata.Add(new ProductMetadata
                            {
                                Column = columnIndex
                            });
                            break;
                        }
                }
                columnIndex++;
            }
        }

        public void ParseText1Row(IList<string> columns)
        {
            var value = columns[1];
            if (string.IsNullOrWhiteSpace(value)) return;

            var text1 = Metadata.Text1;
            Metadata.Text1 = text1.Length > 0 ? $"{text1}\n{value.Trim()}" : value.Trim();
        }

        public void ParseUnitTextRow(IList<string> columns)
        {
            foreach (var productMetadata in Metadata.ProductMetadata)
            {
                productMetadata.UnitText = columns[productMetadata.Column];
            }
        }

        public void ParseUnitPriceRow(IList<string> columns)
        {
            foreach (var productMetadata in Metadata.ProductMetadata)
            {
                productMetadata.UnitPrice = double.Parse(columns[productMetadata.Column]);
            }
        }

        public void ParseUnitNumberRow(string[] columns)
        {
            foreach (var productMetadata in Metadata.ProductMetadata)
            {
                productMetadata.UnitNumber = (int)double.Parse(columns[productMetadata.Column]);
            }
        }


    }
}
