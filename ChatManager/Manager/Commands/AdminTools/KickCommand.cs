using ChatManager.Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
namespace ChatManager.Manager.Commands.AdminTools;

public static class KickCommand
{
    public static async Task KickUser(ITelegramBotClient botClient, Message msg, string description)
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
                .ThenInclude(u => u.Kick)
                .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            var currentUser = userData?.Users?.FirstOrDefault(u => u.UserId == msg.ReplyToMessage.From?.Id);
            if (currentUser is null || userData is null)
            {
                await DbMethods.InitializeUserAsync(msg);
                userData = db.Chats
                    .Include(u => u.Users)
                    .ThenInclude(u => u.Kick)
                    .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
                currentUser = userData?.Users?.FirstOrDefault(u => u.UserId == msg.ReplyToMessage.From?.Id);
            }

            if (currentUser.Kick == null) currentUser.Kick = new EntityList.Kick();

            currentUser.Kick.Description = description;
            await db.SaveChangesAsync();
            await botClient.BanChatMember(msg.Chat.Id, msg.ReplyToMessage.From.Id);
            await botClient.UnbanChatMember(msg.Chat.Id, msg.ReplyToMessage.From.Id);
            await botClient.SendMessage(msg.Chat.Id,
                $"Пользователь {msg.ReplyToMessage.From.FirstName} успешно кикнут!",
                ParseMode.Html);
        }
    }
}