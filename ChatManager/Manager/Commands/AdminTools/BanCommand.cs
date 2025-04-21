using ChatManager.Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
namespace ChatManager.Manager.Commands.AdminTools;

public static class BanCommand
{
        public static async Task BanUser(ITelegramBotClient botClient, Message msg, int duration, string description)
    {
        var member = await DbMethods.GetMemberAsync(botClient, msg);
        if (msg.ReplyToMessage is null) return;
        if (member.Status != ChatMemberStatus.Administrator && member.Status != ChatMemberStatus.Creator)
        {
            await botClient.SendMessage(msg.Chat.Id, "У вас недостаточно прав чтобы использовать эту комманду.",
                ParseMode.Html);
            return;
        }

        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = await DbMethods.GetUserDataAsync(db, msg);
            var currentUser = await DbMethods.GetReplyUserAsync(msg, userData);
            if (currentUser?.Ban is null) currentUser.Ban = new EntityList.Ban();
            currentUser.Ban.Description = description;
            await db.SaveChangesAsync();

            await botClient.BanChatMember(msg.Chat.Id, msg.ReplyToMessage.From.Id,
            untilDate: DateTime.UtcNow.AddDays(duration));
            await botClient.SendMessage(msg.Chat.Id,
                $"Пользователь {msg.ReplyToMessage.From.FirstName} успешно забанен!",
                ParseMode.Html);
        }
    }

    public static async Task UnBanUser(ITelegramBotClient botClient, Message msg)
    {
        var member = await DbMethods.GetMemberAsync(botClient, msg);
        if (msg.ReplyToMessage is null) return;
        if (member.Status != ChatMemberStatus.Administrator && member.Status != ChatMemberStatus.Creator)
        {
            await botClient.SendMessage(msg.Chat.Id, "У вас недостаточно прав чтобы использовать эту комманду.",
                ParseMode.Html);
            return;
        }
        
        await botClient.UnbanChatMember(msg.Chat.Id, msg.ReplyToMessage.From.Id);
        await botClient.SendMessage(msg.Chat.Id,
            $"Пользователь {msg.ReplyToMessage.From.FirstName} успешно разбанен!",
            ParseMode.Html);
    }
}