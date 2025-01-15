using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
namespace TelegramPortfolio;
public class Portfolio : IDisposable
{
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
                var profile = "Name: <b>Natnael Yirga</b>\n\n";
                foreach (var media in config.GetSection("SocialMedias").Get<SocialMedia[]>() ?? [])
                {
                    profile += $"<b>{media.Name}: </b><i><a href='{media.Link}'>@natidotnet</a></i>\n";
                }
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