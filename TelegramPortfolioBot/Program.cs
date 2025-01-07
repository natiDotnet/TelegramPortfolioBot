// See https://aka.ms/new-console-template for more information
using Telegram.Bot;
using TelegramPortfolio;
Console.WriteLine("Hello, World!");
var bot = new TelegramBotClient("7285055954:AAF-hpCkBEibjkzifAtG2g_AqsPJsJnqu1c");
        var me = await bot.GetMe();
                var portfolio = new Portfolio(bot);
                        bot.OnMessage += portfolio.OnMessage;
                                Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
                                        Console.ReadLine();