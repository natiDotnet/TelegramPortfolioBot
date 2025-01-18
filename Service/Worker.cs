using Telegram.Bot;
using TelegramPortfolio;

namespace Service;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration config;

    public Worker(ILogger<Worker> logger, IConfiguration config)
    {
        _logger = logger;
        this.config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var bot = new TelegramBotClient(config["Token"]!);
                var portfolio = new Portfolio(bot, config);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}