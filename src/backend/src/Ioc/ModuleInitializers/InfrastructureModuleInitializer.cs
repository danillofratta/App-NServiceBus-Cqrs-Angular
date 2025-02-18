using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        //builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());
        //builder.Services.AddScoped<IUserRepository, UserRepository>();
    }
}