using Eu.Iamia.Utils.Contract;

namespace Eu.Iamia.Utils;
public sealed class Interval<T> : IInterval<T> where T : struct, IComparable<T>
{
    public T From { get; private set; }

    public T To { get; private set; }

    public string ToAsEconomicDate => ((DateTime)(ValueType)To).ToString("yyyy-MM-dd");

    public string FromAsEconomicDate => ((DateTime)(ValueType)From).ToString("yyyy-MM-dd");

    private static bool HasRightOrder(T from, T to) => from.CompareTo(to) <= 0;

    /// <summary>
    /// Ensures from &lt;= to.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static IInterval<T> Create(T from, T to)
    {
        return HasRightOrder(from, to)
            ? (IInterval<T>)new Interval<T> { From = from, To = to }
            :throw new ArgumentException($"Expected from <= to, actual from: '{from}', to: '{to}'")            
        ;
    }
}
