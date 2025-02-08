using ChatManager.Database;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatManager.Manager.Commands;

public class StartCommand
{
    public async Task StartCmd(ITelegramBotClient botClient, Message msg)
    {
        await DbMethods.InitializeUserAsync(msg);
        await botClient.SendMessage(msg.Chat.Id, "Тест", ParseMode.Html);
    }
}