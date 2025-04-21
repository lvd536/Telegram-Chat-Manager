using ChatManager.Database;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatManager.Manager.Commands.AdminTools.CreatorCommands;

public static class SetLevelCommand
{
    public static async Task SetLevelAsync(ITelegramBotClient botClient, Message msg, int level)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            if (msg.ReplyToMessage is null) return;
            var chat = await DbMethods.GetUserDataAsync(db, msg);
            var targetUser = await DbMethods.GetReplyUserAsync(msg, chat);
            var chatUserInfo = await botClient.GetChatMember(msg.Chat.Id, targetUser.UserId);
            /*if (targetUser.UserName != "lvd.") return;*/
            targetUser.Level = level;
            await db.SaveChangesAsync();
            await botClient.SendMessage(msg.Chat.Id, $"Успешно установил уровень {level} пользователю @{chatUserInfo.User.Username}", ParseMode.Html);
        }
    }
}