using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
namespace TelegramPortfolio;
public class Portfolio : IDisposable
{
    private readonly TelegramBotClient bot;

    public Portfolio(TelegramBotClient bot)
    {
        this.bot = bot;
    }

    public void Dispose()
    {
        bot.OnMessage -= OnMessage;
    }

    public async Task OnMessage(Message msg, UpdateType type)
    {
        switch (msg.Text)
        {
            case "/start":
                Console.WriteLine("start...");

                var replyMarkup = GetMenu();
                await bot.SendMessage(msg.Chat, "Welcome to my portfolio bot! Use.",
                    replyMarkup: replyMarkup);
                break;
            case "/about" or "Profile":
                Console.WriteLine("about message");

                await bot.SendPhoto(
                    msg.Chat,
                    "https://telegrambots.github.io/book/docs/photo-ara.jpg",
                    "<b>Natnael Yirga</b>", ParseMode.Html);
                break;
            case "/skills" or "Skills":
            Console.WriteLine("Skills click");
                var inlineKeys = GetSkills();
                await bot.SendMessage(msg.Chat, "- - - - - - - - - - My Skills - - - - - - - - - -", replyMarkup: inlineKeys);
                break;
            case "/projects" or "Projects":
                var projects = GetProjects();
                await bot.SendMessage(msg.Chat, "My Projects", replyMarkup: projects);
                break;

        }

    }

    private ReplyKeyboardMarkup GetMenu()
    {
        return new ReplyKeyboardMarkup(true)
                .AddButtons("Profile", "Skills")
                .AddNewRow("Projects")
                .AddNewRow("Contact me ✉️")
                .AddButton("Website", "https://nati-net-portfolio.vercel.app/en");
    }
    private InlineKeyboardMarkup GetSkills()
    {
        return new InlineKeyboardMarkup()
                .AddButton("C#", "csharp")
                .AddButton("Typescript", "ts")
                .AddButton("Java", "java")
                .AddNewRow()
                .AddButton(".NET", "dotnet")
                .AddButton("ASP.NET","asp")
                .AddButton("Angular","ng")
                .AddNewRow()
                .AddButton("SQL","sql")
                .AddButton("MySQL","mysql")
                .AddButton("EF Core", "ef");
    }
    private InlineKeyboardMarkup GetProjects()
    {
        return new InlineKeyboardMarkup()
        .AddButton("Oromia Civil Registration System", "or")
        .AddNewRow()
        .AddButton(InlineKeyboardButton.WithUrl("Telebirr C2B Payment API integration", "https://github.com/natiDotnet/Appdiv.Payment/tree/master/Appdiv.Payment.Telebirr")) 
        .AddNewRow()
        .AddButton(InlineKeyboardButton.WithUrl("CBE birr C2B Payment API integration", "https://github.com/natiDotnet/Appdiv.Payment/tree/master/Appdiv.Payment.CBEBirr")) 
        .AddNewRow()
        .AddButton("Attendance Management With ZKTeco SDK", "zkteco")
        .AddNewRow()
        .AddButton("Ethiopian to Gregorian Date Convertor", "dateconvertor");

    }

}