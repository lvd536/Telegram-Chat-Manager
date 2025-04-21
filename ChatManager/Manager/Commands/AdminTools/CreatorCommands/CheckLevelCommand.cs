using ChatManager.Database;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatManager.Manager.Commands.AdminTools.CreatorCommands;

public static class CheckLevelCommand
{
    public static async Task CheckUserLevel(ITelegramBotClient botClient, Message msg)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            if (msg.ReplyToMessage is null) return;
            var chat = await DbMethods.GetUserDataAsync(db, msg);
            var targetUser = await DbMethods.GetReplyUserAsync(msg, chat);
            await botClient.SendMessage(msg.Chat.Id, $"Пользователь {targetUser.UserName} имеет {targetUser.Level} уровень", ParseMode.Html);
        }
    }
}