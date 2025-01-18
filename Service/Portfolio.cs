using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
namespace TelegramPortfolio;
public class Portfolio : IDisposable
{
    private class Profile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
    private class SocialMedia
    {
        public string Name { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }
    private class Project
    {
        public string Name { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public bool WithUrl { get; set; }
    }
    private readonly TelegramBotClient bot;
    private readonly IConfiguration config;

    public Portfolio(IConfiguration config)
    {
        this.bot = new TelegramBotClient(config["Token"]!);
        this.config = config;
        bot.OnMessage += OnMessage;
    }

    public void Dispose()
    {
        bot.OnMessage -= OnMessage;
    }

    public async Task SendMessage(long userid, string message)
    {
        await bot.SendMessage(userid, message);
    }
    public async Task OnMessage(Message msg, UpdateType type)
    {
        var profile = config.GetSection(nameof(Profile)).Get<Profile>();
        switch (msg.Text)
        {
            case "/start":
                var replyMarkup = GetMenu();
                await bot.SendMessage(msg.Chat, "Welcome to my portfolio bot! Use.",
                    replyMarkup: replyMarkup);
                break;
            case "/about" or "Profile":
                var about = $"Name: <b>{profile?.FirstName} {profile?.LastName}</b>\n\n";
                foreach (var media in config.GetSection("SocialMedias").Get<SocialMedia[]>() ?? [])
                {
                    about += $"<b>{media.Name}: </b><i><a href='{media.Link}'>@{profile?.UserName}</a></i>\n";
                }
                await bot.SendPhoto(
                    msg.Chat,
                    config["Image"]!,
                    about
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

                var contact = $"""
                            BEGIN:VCARD
                            VERSION:3.0
                            N:{profile?.LastName};{profile?.FirstName}
                            ORG:.NET Developer
                            TEL;TYPE=voice,work,pref:{profile?.PhoneNumber}
                            EMAIL:{profile?.Email}
                            END:VCARD
                            """;
                await bot.SendContact(msg.Chat, phoneNumber: profile?.PhoneNumber!, firstName: profile?.FirstName!, lastName: profile?.LastName, vcard: contact);
                break;
            default:
                var cc = msg.Chat.Id;
                var ct = new ChatId(737328646);
                await bot.SendMessage(ct, "Please select from the menu", replyMarkup: GetMenu());
                break;

        }

    }

    private ReplyKeyboardMarkup GetMenu()
    {
        // var bt = new KeyboardButton
        // {
        //     Text = "Profile",
        //     RequestChat = new KeyboardButtonRequestChat
        //     {

        //     }
        // };
        return new ReplyKeyboardMarkup(true)
                .AddButtons("Profile", KeyboardButton.WithRequestPoll("Skills", PollType.Regular))
                .AddNewRow("Projects")
                .AddNewRow(KeyboardButton.WithWebApp("Website", config["Website"]!))
                .AddButton("Contact me ✉️");
    }
    private InlineKeyboardMarkup GetSkills()
    {
        var skills = new InlineKeyboardMarkup();
        int index = 0;
        foreach (string skill in config.GetSection("Skills").Get<string[]>() ?? [])
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
        var projects = new InlineKeyboardMarkup();
        foreach (var project in config.GetSection("Projects").Get<Project[]>() ?? [])
        {
            if (project.WithUrl)
            {
                projects.AddButton(InlineKeyboardButton.WithUrl(project.Name, project.Link));
            }
            else
            {
                projects.AddButton(project.Name, project.Link);
            }
            projects.AddNewRow();
        }
        return projects;
    }

}