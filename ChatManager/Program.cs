using System.Collections;
using System.Reflection;
using ChatManager.Services;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChatManager;
internal class Program
{
    private static CancellationTokenSource _cts = null!;
    private static TelegramBotClient _bot = null!;
    
    private static async Task Main()
    {
        var executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var projectRoot = Path.GetFullPath(Path.Combine(executableLocation, @"..\..\.."));

        var configuration = new ConfigurationBuilder()
            .SetBasePath(projectRoot)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var botToken = configuration.GetSection("BotConfiguration:launchType").Value == "1" ? configuration.GetSection("BotConfiguration:prodToken").Value : configuration.GetSection("BotConfiguration:devToken").Value;
        if (string.IsNullOrEmpty(botToken)) throw new ArgumentNullException("Bot token is not configured in appsettings.json");
        var openRouterApiKey = configuration.GetSection("OtherSettings:OpenRouterKey").Value;
        if (string.IsNullOrEmpty(openRouterApiKey)) throw new ArgumentNullException("OpenRouterKey is not configured in appsettings.json");
        
        _cts = new CancellationTokenSource();
        _bot = new TelegramBotClient(botToken, cancellationToken: _cts.Token);
        var aiService = new AiService(openRouterApiKey, _bot);
        
        var onMessageService = new OnMessageService(_bot, aiService);
        var onCallbackQuery = new OnCallbackQueryService(_bot);
        var onErrorService = new OnErrorService(_cts, _bot);
        
        var me = await _bot.GetMe();
        var botCommands = await GetCommands();
        await _bot.SetMyCommands(botCommands, languageCode: me.LanguageCode, cancellationToken: _cts.Token);
        _bot.OnMessage += onMessageService.OnMessage;
        _bot.OnUpdate += onCallbackQuery.OnCallbackQuery;
        _bot.OnError += onErrorService.OnError;
        
        Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
        Console.ReadLine();
        _cts.Cancel();
    }

    private static async Task<List<BotCommand>> GetCommands()
    {
        List<BotCommand> commands = new List<BotCommand>();
        var commandList = new Dictionary<string, string>
        {
            { "/start", "Запуск бота" },
            { "/id", "Показать ваш ID" },
            { "/profile", "Профиль пользователя" },
            { "/top", "Топ пользователей" },
            { "/mute", "Замутить пользователя" },
            { "/unmute", "Размутить пользователя" },
            { "/ban", "Забанить пользователя" },
            { "/unban", "Разбанить пользователя" },
            { "/kick", "Кикнуть пользователя" },
            { "/warn", "Выдать предупреждение" },
            { "/unwarn", "Снять предупреждение" },
            { "/info", "Информация о пользователе" },
            { "/help", "Помощь по командам" },
            { "/add", "Добавить слово в блок-лист" },
            { "/blocklist", "Показать блок-лист слов" },
            { "/remove", "Удалить слово из блок-листа" },
            { "/devblog", "Дев Блог" },
            { "/chance", "Случайный шанс" },
            { "/editlevel", "Изменить уровень (админ)" },
            { "/checklevel", "Проверить уровень" },
            { "/weather", "Погода в Самаре" },
            { "/ai", "Чат с ИИ. Принимает текст или фото" }
        };

        commands.AddRange(commandList.Select(cmd => new BotCommand { Command = cmd.Key, Description = cmd.Value }));
        return commands;
    }
}