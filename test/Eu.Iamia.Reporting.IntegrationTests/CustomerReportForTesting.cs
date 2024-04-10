﻿using Eu.Iamia.Reporting.Configuration;
using Microsoft.Extensions.Options;

namespace Eu.Iamia.Reporting.IntegrationTests;
public class CustomerReportForTesting : CustomerCustomerReport
{
    public CustomerReportForTesting(SettingsForReporting settings) : base(settings)
    { }

    public bool Exists()
    {
        return ReportFile is not null ? ReportFile.Exists : false;
    }

    public bool Exists(string filename)
    {
        var t1 = new FileInfo(filename);
        t1.Refresh();
        var t2 = t1.Exists;
        
        return t2;
    }

    public string GetContent()
    {
        if (ReportFile is null || !ReportFile.Exists) return string.Empty;
        return new StreamReader(ReportFile.OpenRead()).ReadToEnd();
    }
}
