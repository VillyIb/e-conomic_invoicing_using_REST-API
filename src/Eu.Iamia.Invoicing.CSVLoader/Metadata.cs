// ReSharper disable StringLiteralTypo

namespace Eu.Iamia.Invoicing.CSVLoader;

    public class Metadata
    {
        public int CustomerNumberColumn { get; set; }

        private IList<ProductMetadata>? _productMetadata;

        public IList<ProductMetadata> ProductMetadata => _productMetadata ??= new List<ProductMetadata>();

        private List<int>? _customerGroupToAccept;

        public IList<int> CustomerGroupToAccept => _customerGroupToAccept ??= [];

        public string Text1 { get; set; } = string.Empty;

        public DateTime? InvoiceDate { get; set; }

        public int? PaymentTerm { get; set; }
    }
