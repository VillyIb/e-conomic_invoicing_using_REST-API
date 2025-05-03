using Eu.Iamia.Reporting.Contract;

namespace Eu.Iamia.Reporting;
public  class ReportStateStrategy
{
    public ReportStateStrategy(ReportState reportState)
    {
        ReportState = reportState;

        switch (reportState)
        {
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

            case ReportState.Info:
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

    public ReportState ReportState { get; init; }
}
