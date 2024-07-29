namespace Eu.Iamia.Invoicing.Application.Contract;

public class ExecutionStatus
{
    public int Status { get; set; } = 0;

    public string Report { get; set; } = string.Empty;

    public int CountFails { get; set; }
}