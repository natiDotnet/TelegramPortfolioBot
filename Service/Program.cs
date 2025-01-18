using Service;

public class Program
{
    public static void Main()
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<Worker>();

        var host = builder.Build();
        host.Run();
    }
}
