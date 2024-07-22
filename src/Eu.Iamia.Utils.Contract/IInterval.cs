namespace Eu.Iamia.Utils.Contract;

public interface IInterval<T> where T : struct, IComparable<T>
{
    T                 From { get; }
    T To   { get; }
}