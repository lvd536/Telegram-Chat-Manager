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
        var member = await botClient.GetChatMember(msg.Chat.Id, msg.From.Id);
        if (msg.ReplyToMessage is null) return;
        if (member.Status != ChatMemberStatus.Administrator && member.Status != ChatMemberStatus.Creator)
        {
            await botClient.SendMessage(msg.Chat.Id, "У вас недостаточно прав чтобы использовать эту комманду.",
                ParseMode.Html);
            return;
        }

        using (ApplicationContext db = new ApplicationContext())
        {
            var userData = db.Chats
                .Include(u => u.Users)
                .ThenInclude(u => u.Ban)
                .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            var currentUser = userData?.Users?.FirstOrDefault(u => u.UserId == msg.ReplyToMessage.From?.Id);
            if (currentUser is null || userData is null)
            {
                await DbMethods.InitializeUserAsync(msg);
                userData = db.Chats
                    .Include(u => u.Users)
                    .ThenInclude(u => u.Ban)
                    .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
                currentUser = userData?.Users?.FirstOrDefault(u => u.UserId == msg.ReplyToMessage.From?.Id);
            }

            if (currentUser.Ban == null) currentUser.Ban = new EntityList.Ban();

            currentUser.Ban.Description = description;
            await db.SaveChangesAsync();

            await botClient.BanChatMember(msg.Chat.Id, msg.ReplyToMessage.From.Id,
            untilDate: DateTime.UtcNow.AddMinutes(duration));
            await botClient.SendMessage(msg.Chat.Id,
                $"Пользователь {msg.ReplyToMessage.From.FirstName} успешно забанен!",
                ParseMode.Html);
        }
    }

    public static async Task UnBanUser(ITelegramBotClient botClient, Message msg)
    {
        var member = await botClient.GetChatMember(msg.Chat.Id, msg.From.Id);
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