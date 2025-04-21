using ChatManager.Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatManager.Manager.Commands.AdminTools;

public static class MuteCommand
{
        public static async Task MuteUser(ITelegramBotClient botClient, Message msg, int duration, string description)
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

            var mute = new EntityList.Mute
            {
                Description = description + $" | Продолжительность: {duration}"
            };
            currentUser.Mutes.Add(mute);
            await db.SaveChangesAsync();
            await botClient.RestrictChatMember(msg.Chat.Id, msg.ReplyToMessage.From.Id, new ChatPermissions()
            {
                CanSendMessages = false,
                CanSendOtherMessages = false,
                CanAddWebPagePreviews = false,
                CanSendVoiceNotes = false,
                CanSendDocuments = false,
                CanPinMessages = false,
                CanSendPhotos = false,
                CanSendAudios = false,
                CanSendVideos = false,
                CanSendPolls = false,
                CanSendVideoNotes = false
            }, untilDate: DateTime.UtcNow.AddMinutes(duration));
            await botClient.SendMessage(msg.Chat.Id,
                $"Успешно выдал мут пользователю {msg.ReplyToMessage.From.FirstName} на {duration} минут!");
        }
    }

    public static async Task UnMuteUser(ITelegramBotClient botClient, Message msg)
    {
        var member = await DbMethods.GetMemberAsync(botClient, msg);
        if (msg.ReplyToMessage is null) return;
        if (member.Status != ChatMemberStatus.Administrator && member.Status != ChatMemberStatus.Creator)
        {
            await botClient.SendMessage(msg.Chat.Id, "У вас недостаточно прав чтобы использовать эту комманду.",
                ParseMode.Html);
            return;
        }

        await botClient.RestrictChatMember(msg.Chat.Id, msg.ReplyToMessage.From.Id, new ChatPermissions()
        {
            CanSendMessages = true,
            CanSendOtherMessages = true,
            CanAddWebPagePreviews = true,
            CanSendVoiceNotes = true,
            CanSendDocuments = true,
            CanPinMessages = true,
            CanSendPhotos = true,
            CanSendAudios = true,
            CanSendVideos = true,
            CanSendPolls = true,
            CanSendVideoNotes = true
        });
        await botClient.SendMessage(msg.Chat.Id,
            $"Успешно снят мут пользователю {msg.ReplyToMessage.From.FirstName}!");
    }
}