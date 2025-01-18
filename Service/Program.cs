using Service;
using TelegramPortfolio;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<Worker>();
        builder.Services.AddSingleton<Portfolio>();

        var host = builder.Build();
        host.Run();

    }
}
