using Alanyang.DotNetEmail.ApplicationCore.Interfaces;
using Alanyang.DotNetEmail.ApplicationCore.Services;
using Alanyang.DotNetEmail.Infrastructure.Data;
using Alanyang.DotNetEmail.Infrastructure.Services;

namespace Alanyang.DotNetEmail.WebApp.Configuration;

public static class ConfigureCoreServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

        services.AddScoped<IEmailSendingQueue, EmailSendingQueue>();
        services.AddTransient<IEmailSender, EmailSender>();

        return services;
    }
}