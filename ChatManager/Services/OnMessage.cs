using ChatManager.Manager;
using ChatManager.Manager.Commands;
using ChatManager.Manager.Commands.AdminTools;
using ChatManager.Manager.Commands.AdminTools.CreatorCommands;
using ChatManager.Manager.Commands.Games;
using ChatManager.Manager.Commands.UserTools.Weather;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatManager.Services;

public class OnMessageService
{
    private static TelegramBotClient _bot = null!;
    public OnMessageService(TelegramBotClient botClient)
    {
        _bot = botClient;
    }
    
    public async Task OnMessage(Message msg, UpdateType type)
    {
        await MessageCounter.MessageCounterAsync(_bot, msg, msg.Type);
        await WordsAnalyzer.MessageAnalyzer(_bot, msg);
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
                    await StartCommand.StartCmd(_bot, msg);
                    break;
                case "/id":
                    if (msg.From is not null) 
                        await _bot.SendMessage(msg.Chat.Id, $"ID пользователя {msg.From.FirstName}: {msg.From.Id}", ParseMode.Html);
                    break;
                case "/profile":
                    await ProfileCommand.ProfileCmd(_bot, msg);
                    break;
                case "/top":
                    await TopCommand.TopCmd(_bot, msg, 1);
                    break;
                case "/mute":
                    try {
                        if (argument is not null)
                        {
                            if (defArgument is null)
                            {
                                await MuteCommand.MuteUser(_bot, msg, int.Parse(argument), "Не указана");
                            }
                            else await MuteCommand.MuteUser(_bot, msg, int.Parse(argument), defArgument);
                        }
                    }
                    catch (Exception) {
                        await _bot.SendMessage(msg.Chat.Id, "Неверно или вовсе не указано значение. Пример: /mute 30 (мут на 30 минут)", ParseMode.Html);
                    }

                    break;
                case "/unmute":
                    await MuteCommand.UnMuteUser(_bot, msg);
                    break;
                case "/ban":
                    try {
                        if (argument is not null)
                        {
                            if (defArgument is null)
                            {
                                await BanCommand.BanUser(_bot, msg, int.Parse(argument), "Не указана");
                            }
                            else await BanCommand.BanUser(_bot, msg, int.Parse(argument), defArgument);
                        }
                    }
                    catch (Exception) {
                        await _bot.SendMessage(msg.Chat.Id, "Неверно или вовсе не указано значение. Пример: /ban 30 причина(не обязательно) (бан на 30 дней)", ParseMode.Html);
                    }

                    break;
                case "/unban":
                    await BanCommand.UnBanUser(_bot, msg);
                    break;
                case "/kick":
                    if (argument is null)
                    {
                        await KickCommand.KickUser(_bot, msg, "Не указана");
                    }
                    else await KickCommand.KickUser(_bot, msg, argument);

                    break;
                case "/warn":
                    if (argument is null)
                    {
                        await WarnCommand.WarnUser(_bot, msg, "Не указана");
                    }
                    else await WarnCommand.WarnUser(_bot, msg, argument);
                    break;
                case "/unwarn":
                    await WarnCommand.UnWarnUser(_bot, msg);
                    break;
                case "/info":
                    await UserInfoCommand.UserInfo(_bot, msg);
                    break;
                case "/help":
                    await HelpCommand.HelpCmd(_bot, msg);
                    break;
                case "/add":
                    if (argument is null)
                    {
                        await _bot.SendMessage(msg.Chat.Id, "Слово не указано.", ParseMode.Html);
                    }
                    else await WordsAnalyzer.AddWord(_bot, msg, argument);
                    break;
                case "/blocklist":
                    await WordsAnalyzer.ListWords(_bot, msg);
                    break;
                case "/remove":
                    if (argument is null)
                    {
                        await _bot.SendMessage(msg.Chat.Id, "Слово не указано.", ParseMode.Html);
                    }
                    else await WordsAnalyzer.RemoveWord(_bot, msg, argument);
                    break;
                case "/devblog":
                    await DevblogCommand.DevblogCommandAsync(_bot, msg);
                    break;
                case "/chance":
                    await ChanceCommand.ChanceCommandAsync(_bot, msg);
                    break;
                case "/quote":
                    await QuoteCommand.QuoteCommandAsync(_bot, msg);
                    break;
                case "/editLevel":
                    if (int.TryParse(argument, out int editLevelValue))
                    {
                        await SetLevelCommand.SetLevelAsync(_bot, msg, editLevelValue);
                    }
                    break;
                case "/checkLevel":
                    await CheckLevelCommand.CheckUserLevel(_bot, msg);
                    break;
                case "/weather":
                    var weather = new WeatherCommand();
                    await weather.WeatherCmd(_bot, msg);
                    break;
            }
        }
    }
}