using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
namespace TelegramPortfolio;
public class Portfolio : IDisposable
{
    private readonly TelegramBotClient bot;
    private readonly IConfiguration config;

    public Portfolio(TelegramBotClient bot, IConfiguration config)
    {
        this.bot = bot;
        this.config = config;
        bot.OnMessage += OnMessage;
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
                var replyMarkup = GetMenu();
                await bot.SendMessage(msg.Chat, "Welcome to my portfolio bot! Use.",
                    replyMarkup: replyMarkup);
                break;
            case "/about" or "Profile":
            var profile = 
            """
            Name: <b>Natnael Yirga</b> <b style='background-color: #5c2d91; color: white; padding: 5px; border-radius: 5px;'>.NET</b>
            <b>LinkedIn: </b><i><a href='https://www.linkedin.com/in/natidotnet'>@natidotnet</a></i>
            <b>X: </b><a href='https://x.com/natidotnet'>@natidotnet</a>
            <b>Telegram: </b><a href='https://t.me/natidotnet'>@natidotnet</a>
            <b>Github: </b><a href='https://github.com/natiDotnet'>@natidotnet</a>

            """;
                await bot.SendPhoto(
                    msg.Chat,
                    config["Image"]!,
                    profile
                    , ParseMode.Html);
                break;
            case "/skills" or "Skills":
                var inlineKeys = GetSkills();
                await bot.SendMessage(msg.Chat, "- - - - - - - - - - My Skills - - - - - - - - - -", replyMarkup: inlineKeys);
                break;
            case "/projects" or "Projects":
                var projects = GetProjects();
                await bot.SendMessage(msg.Chat, "My Projects", replyMarkup: projects);
                break;
            case "/contact" or "Contact me ✉️":
                Console.WriteLine("contact clicked");
                var contact = "BEGIN:VCARD\n" +
                              "VERSION:3.0\n" +
                              "N:Yirga;Natnael\n" +
                              "ORG:.NET Developer\n" +
                              "TEL;TYPE=voice,work,pref:+251905410217\n" +
                              "EMAIL:natidotnet@gmail.com\n" +
                              "END:VCARD"; 
                await bot.SendContact(msg.Chat, phoneNumber: "+251905410217", firstName: "Natnael", lastName: "Yirga", vcard: contact);
                break;

        }

    }

    private ReplyKeyboardMarkup GetMenu()
    {
        return new ReplyKeyboardMarkup(true)
                .AddButtons("Profile", "Skills")
                .AddNewRow("Projects")             
                .AddNewRow(KeyboardButton.WithWebApp("Website", "https://nati-net-portfolio.vercel.app/en"))
                .AddButton("Contact me ✉️");
    }
    private InlineKeyboardMarkup GetSkills()
    {
        var skills = new InlineKeyboardMarkup();
        int index = 0;
        foreach (string skill in config.GetSection("Skills").Get<string[]>() ?? [] )
        {
            skills.AddButton(skill, skill);
            if (index % 3 == 0)
            {
                skills.AddNewRow();
            }
            index++;
        }
        return skills;
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