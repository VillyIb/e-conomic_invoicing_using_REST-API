using Eu.Iamia.Utils.Contract;

namespace Eu.Iamia.Utils;
public sealed class Interval<T> : IInterval<T> where T : struct, IComparable<T>
{
    public T From { get; private set; }

    public T To { get; private set; }

    /// <summary>
    /// Ensures from &lt;= to.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static IInterval<T> Create(T from, T to)
    {
        var compareTo = from.CompareTo(to);
        if (compareTo > 0)
        {
            throw new ArgumentException($"Expected from <= to, actual from: '{from}', to: '{to}'");
        }

        return new Interval<T> { From = from, To = to };
    }
}
