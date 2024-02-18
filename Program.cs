using BackendTask;
using BackendTask.Configuration;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile($"conf/appsettings.json")
            .AddEnvironmentVariables()
            .Build();
        builder.Services.ConfigureServices(configuration);

        var app = builder.Build();

        Startup.Start(app);
    }
}
