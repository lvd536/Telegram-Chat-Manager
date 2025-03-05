using Microsoft.VisualBasic;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatManager.Manager.Commands.Games;

public static class ChanceCommand
{
    public static async Task ChanceCommandAsync(ITelegramBotClient botClient, Message msg)
    {
        Random rnd = new Random();
        await botClient.SendMessage(msg.Chat.Id, $"Шанс что<i>{msg.Text?.Replace("/chance", string.Empty)}</i> равен <b>{rnd.Next(0, 101)}%</b>", ParseMode.Html);
    }
}