using Microsoft.AspNetCore.Builder;

namespace IoC;

public interface IModuleInitializer
{
    void Initialize(WebApplicationBuilder builder);
}
