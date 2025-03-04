using ChatManager.Manager;
using ChatManager.Manager.Commands;
using ChatManager.Manager.Commands.AdminTools;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("token", cancellationToken: cts.Token);
var me = await bot.GetMe();
bot.OnMessage += OnMessage;
bot.OnUpdate += OnCallbackQuery;
bot.OnError += OnError;
Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel();

async Task OnMessage(Message msg, UpdateType type)
{
    await MessageCounter.MessageCounterAsync(bot, msg, msg.Type);
    await WordsAnalyzer.MessageAnalyzer(bot, msg);
    if (msg.Text is null) return;
    var commandParts = msg.Text.Split(' ');
    var command = commandParts[0];
    var argument = commandParts.Length >= 2 ? commandParts[1] : null;
    var defArgument = commandParts.Length >= 3 ? commandParts[2] : null;
    if (msg.Text.StartsWith('/'))
    {
        switch (command)
        {
            case "/start":
                await StartCommand.StartCmd(bot, msg);
                break;
            case "/id":
                await bot.SendMessage(msg.Chat.Id, $"ID пользователя {msg.From.FirstName}: {msg.From.Id}",
                    ParseMode.Html);
                break;
            case "/profile":
                await ProfileCommand.ProfileCmd(bot, msg);
                break;
            case "/top":
                await TopCommand.TopCmd(bot, msg, 1);
                break;
            case "/mute":
                try
                {
                    if (defArgument is null)
                    {
                        await MuteCommand.MuteUser(bot, msg,int.Parse(argument),"Не указана");
                    }
                    else await MuteCommand.MuteUser(bot, msg,int.Parse(argument), defArgument);
                }
                catch (Exception)
                {
                    await bot.SendMessage(msg.Chat.Id,
                        "Неверно или вовсе не указано значение. Пример: /mute 30 (мут на 30 минут)", ParseMode.Html);
                }

                break;
            case "/unmute":
                await MuteCommand.UnMuteUser(bot, msg);
                break;
            case "/ban":
                try
                {
                    if (defArgument is null)
                    {
                        await BanCommand.BanUser(bot, msg,int.Parse(argument),"Не указана");
                    }
                    else await BanCommand.BanUser(bot, msg,int.Parse(argument), defArgument);
                }
                catch (Exception)
                {
                    await bot.SendMessage(msg.Chat.Id,
                        "Неверно или вовсе не указано значение. Пример: /ban 30 (мут на 30 минут)", ParseMode.Html);
                }
                break;
            case "/unban":
                await BanCommand.UnBanUser(bot, msg);
                break;
            case "/kick":
                if (argument is null)
                {
                    await KickCommand.KickUser(bot, msg, "Не указана");
                }
                else await KickCommand.KickUser(bot, msg, argument);
                break;
            case "/warn":
                if (argument is null)
                {
                    await WarnCommand.WarnUser(bot, msg, "Не указана");
                }
                else await WarnCommand.WarnUser(bot, msg, argument);

                break;
            case "/unwarn":
                await WarnCommand.UnWarnUser(bot, msg);
                break;
            case "/info":
                await UserInfoCommand.UserInfo(bot, msg);
                break;
            case "/help":
                await HelpCommand.HelpCmd(bot, msg);
                break;
            case "/add":
                if (argument is null)
                {
                    await bot.SendMessage(msg.Chat.Id, "Слово не указано.", ParseMode.Html);
                }
                else await WordsAnalyzer.AddWord(bot, msg, argument);
                break;
            case "/blocklist":
                await WordsAnalyzer.ListWords(bot, msg);
                break;
            case "/remove":
                if (argument is null)
                {
                    await bot.SendMessage(msg.Chat.Id, "Слово не указано.", ParseMode.Html);
                }
                else await WordsAnalyzer.RemoveWord(bot, msg, argument);
                break;
            case "/devblog":
                await DevblogCommand.DevblogCommandAsync(bot, msg);
                break;
        }
    }
}

async Task OnCallbackQuery(Update update)
{
    if (update.Type != UpdateType.CallbackQuery) return;
    switch (update.CallbackQuery?.Data)
    {
        case "IdCall":
            await bot.SendMessage(update.CallbackQuery.Message.Chat.Id,
                $"ID пользователя {update.CallbackQuery.From.FirstName}: {update.CallbackQuery.From?.Id}",
                ParseMode.Html);
            break;
        case "TopByLevel":
            await TopCommand.TopCmd(bot, update.CallbackQuery.Message ?? new Message(), 1);
            break;
        case "TopByMessages":
            await TopCommand.TopCmd(bot, update.CallbackQuery.Message ?? new Message(), 2);
            break;
        case "TopByTextMessages":
            await TopCommand.TopCmd(bot, update.CallbackQuery.Message ?? new Message(), 3);
            break;
        case "TopByAudioMessages":
            await TopCommand.TopCmd(bot, update.CallbackQuery.Message ?? new Message(), 4);
            break;
        case "TopByVideoMessages":
            await TopCommand.TopCmd(bot, update.CallbackQuery.Message ?? new Message(), 5);
            break;
        case "TopBySticker":
            await TopCommand.TopCmd(bot, update.CallbackQuery.Message ?? new Message(), 6);
            break;
        case "TopByPhoto":
            await TopCommand.TopCmd(bot, update.CallbackQuery.Message ?? new Message(), 7);
            break;
        case "TopByLocation":
            await TopCommand.TopCmd(bot, update.CallbackQuery.Message ?? new Message(), 8);
            break;
        case "TopByOther":
            await TopCommand.TopCmd(bot, update.CallbackQuery.Message ?? new Message(), 9);
            break;
        case "TopByVoice":
            await TopCommand.TopCmd(bot, update.CallbackQuery.Message ?? new Message(), 10);
            break;
    }
}

async Task OnError(Exception exception, HandleErrorSource handler)
{
    Console.WriteLine(exception);
    await Task.Delay(2000, cts.Token);
}