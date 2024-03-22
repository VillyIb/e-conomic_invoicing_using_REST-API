using Microsoft.Extensions.DependencyInjection;

namespace Eu.Iamia.ConfigBase;

/// <summary>
/// Defines: {Register(IServiceCollection), UnRegister()}
/// </summary>
public interface IHandlerSetup
{
    // NB! requires NuGet package Microsoft.Extensions.Options -> (IServiceCollection)

    public void Register(IServiceCollection services);

    public void UnRegister();
}