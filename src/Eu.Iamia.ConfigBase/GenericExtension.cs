namespace Eu.Iamia.ConfigBase;

public static class GenericExtension
{
    /// <summary>
    /// Check obj for null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <returns>obj when not null</returns>
    /// <exception cref="NullReferenceException">when obj is null</exception>
    public static T CheckForNull<T>(this T? obj, string name)
    {
        if (obj is null)
        {
            throw new NullReferenceException(name);
        }

        return obj;
    }
}