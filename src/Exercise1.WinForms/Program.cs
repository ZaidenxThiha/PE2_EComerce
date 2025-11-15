using System;
using System.Configuration;
using System.Windows.Forms;
using Exercise1.WinForms.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PE2.ECommerce.Domain.Data;
using PE2.ECommerce.Services.Implementations;
using PE2.ECommerce.Services.Interfaces;

namespace Exercise1.WinForms;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        var services = new ServiceCollection();
        ConfigureServices(services);
        using var provider = services.BuildServiceProvider();
        var loginForm = ActivatorUtilities.CreateInstance<LoginForm>(provider, provider);
        Application.Run(loginForm);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["ECommerceDb"]?.ConnectionString;
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Missing ECommerceDb connection string");
        }

        services.AddDbContext<ECommerceDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IAgentService, AgentService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IReportingService, ReportingService>();
        services.AddScoped<MainForm>();
    }
}
