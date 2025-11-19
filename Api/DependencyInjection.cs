using Application;
using Application.Commands;
using Application.Queries;
using Microsoft.EntityFrameworkCore;

namespace Api;

public static class DependencyInjection
{
    private const string DefaultConnection = "DefaultConnection";

    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // https://stackoverflow.com/a/37373557
        services.AddHttpContextAccessor();

        var connectionString = configuration.GetConnectionString(DefaultConnection);

        services.AddDbContext<AppDbContext>(options => {
            options.UseNpgsql(connectionString);
        });

        services.AddTransient<CreateOneOnOneCommand>();
        services.AddTransient<HardDeleteOneOnOneCommand>();
        services.AddTransient<MenteeOneOnOnesQuery>();
    }
}