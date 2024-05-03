using Eu.Iamia.Reporting.Contract;

namespace Eu.Iamia.Reporting;
public  class ReportStateStrategy
{
    private readonly ReportState _reportState;

    public ReportStateStrategy(ReportState reportState)
    {
        _reportState = reportState;

        switch (reportState)
        {
            case ReportState.Info:
                StatusPart = "I";
                PruneFileAfterClose = true;
                Locked = false;
                break;
            case ReportState.Message:
                StatusPart = "M";
                PruneFileAfterClose = false;
                Locked = false;
                break;
            case ReportState.Error:
                StatusPart = "E";
                PruneFileAfterClose = false;
                Locked = true;
                break;
            default:
                StatusPart = "I";
                PruneFileAfterClose = true;
                Locked = false;
                break;
        }
    }

    public bool Locked { get; init; }

    public string StatusPart { get; init; }

    public bool PruneFileAfterClose { get; init; }
}
