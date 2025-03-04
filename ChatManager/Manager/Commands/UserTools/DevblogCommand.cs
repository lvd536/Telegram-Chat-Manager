using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatManager.Manager.Commands;

public static class DevblogCommand
{
    public static async Task DevblogCommandAsync(ITelegramBotClient botClient, Message msg)
    { 
        HttpClient client = new HttpClient();
        string url = "https://raw.githubusercontent.com/lvd536/Telegram-Chat-Manager/refs/heads/master/ChatManager/devblog.txt";
        string content = await client.GetStringAsync(url);
        await botClient.SendMessage(msg.Chat.Id, content, ParseMode.Html);
    }
}