using Telegram.Bot;
using TelegramPortfolio;

namespace Service;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration config;
    private readonly Portfolio portfolio;

    public Worker(ILogger<Worker> logger, IConfiguration config, Portfolio portfolio)
    {
        _logger = logger;
        this.config = config;
        this.portfolio = portfolio;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var bot = new TelegramBotClient(config["Token"]!);
        // var portfolio = new Portfolio(config);
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}