// // See https://aka.ms/new-console-template for more information
using Telegram.Bot;
// using TelegramPortfolio;
using Microsoft.Extensions.Configuration;
IConfiguration builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .Build();
// var bot = new TelegramBotClient(builder["Token"]!);
// var portfolio = new Portfolio(bot, builder);
Console.ReadLine();