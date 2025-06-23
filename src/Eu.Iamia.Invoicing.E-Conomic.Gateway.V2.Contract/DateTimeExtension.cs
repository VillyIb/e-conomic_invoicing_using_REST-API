namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract;

internal static class DateTimeExtension
{
    /// <summary>
    /// Returns Date part as 'yyyy-MM-dd'.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToEconomicDate(this DateTime dateTime) => dateTime.ToString("yyyy-MM-dd");
}
