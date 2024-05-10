﻿using Eu.Iamia.Invoicing.Loader.Contract;
// ReSharper disable StringLiteralTypo

namespace Eu.Iamia.Invoicing.CSVLoader;

/// <summary>
/// Loads invoices from .csv file.
/// One line is One invoice with multiple lines.
/// </summary>
public partial class Loader : ILoader
{
    private Metadata? Metadata { get; set; }

    public string? Text1 => Metadata?.Text1;

    public DateTime? InvoiceDate => Metadata?.InvoiceDate;

    public int? PaymentTerm => Metadata?.PaymentTerm;

    public IList<int>? CustomerGroupToAccept => _csvParser?.Metadata?.CustomerGroupToAccept;

    public IList<IInputInvoice>? Invoices => _csvParser?.Invoices;

    private readonly CsvParser? _csvParser;

    public Loader()
    {
        _csvParser = new CsvParser(new Level2Parsers());
    }

    public int ParseCSV(FileInfo file)
    {
        using var fs = file.OpenRead();
        return _csvParser!.ParseCSV(fs);
    }

    public int ParseCSV(Stream fs)
    {
        return _csvParser!.ParseCSV(fs);
    }
}
