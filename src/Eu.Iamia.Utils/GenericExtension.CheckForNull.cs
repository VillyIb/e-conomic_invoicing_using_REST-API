namespace Eu.Iamia.Utils;

public static partial class GenericExtension
{
    /// <summary>
    /// Converts nullable type to non nullable type by explicit checking for null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <returns>obj when not null</returns>
    /// <exception cref="NullReferenceException">when obj is null</exception>
    public static T CheckForNull<T>(this T? obj, string? name)
    {
        if (obj is null)
        {
            throw new NullReferenceException(name);
        }

        return obj;
    }
}