using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatManager.Manager.Commands;

public class AdminTools
{
    public async Task MuteUser(ITelegramBotClient botClient, Message msg, int duration)
    {
        var member = await botClient.GetChatMember(msg.Chat.Id, msg.From.Id);
        if (msg.ReplyToMessage is null) return;
        if (member.Status != ChatMemberStatus.Administrator && member.Status != ChatMemberStatus.Creator)
        {
            await botClient.SendMessage(msg.Chat.Id, "У вас недостаточно прав чтобы использовать эту комманду.", ParseMode.Html);
            return;
        }
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

    public async Task UnMuteUser(ITelegramBotClient botClient, Message msg)
    {
        var member = await botClient.GetChatMember(msg.Chat.Id, msg.From.Id);
        if (msg.ReplyToMessage is null) return;
        if (member.Status != ChatMemberStatus.Administrator && member.Status != ChatMemberStatus.Creator)
        {
            await botClient.SendMessage(msg.Chat.Id, "У вас недостаточно прав чтобы использовать эту комманду.", ParseMode.Html);
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

    public async Task KickUser(ITelegramBotClient botClient, Message msg)
    {
        var member = await botClient.GetChatMember(msg.Chat.Id, msg.From.Id);
        if (msg.ReplyToMessage is null) return;
        if (member.Status != ChatMemberStatus.Administrator && member.Status != ChatMemberStatus.Creator)
        {
            await botClient.SendMessage(msg.Chat.Id, "У вас недостаточно прав чтобы использовать эту комманду.", ParseMode.Html);
            return;
        }
        await botClient.BanChatMember(msg.Chat.Id, msg.ReplyToMessage.From.Id);
        await botClient.UnbanChatMember(msg.Chat.Id, msg.ReplyToMessage.From.Id);
        await botClient.SendMessage(msg.Chat.Id, $"Пользователь {msg.ReplyToMessage.From.FirstName} успешно кикнут!", ParseMode.Html);
    }
    
    public async Task BanUser(ITelegramBotClient botClient, Message msg, int duration)
    {
        var member = await botClient.GetChatMember(msg.Chat.Id, msg.From.Id);
        if (msg.ReplyToMessage is null) return;
        if (member.Status != ChatMemberStatus.Administrator && member.Status != ChatMemberStatus.Creator)
        {
            await botClient.SendMessage(msg.Chat.Id, "У вас недостаточно прав чтобы использовать эту комманду.", ParseMode.Html);
            return;
        }
        await botClient.BanChatMember(msg.Chat.Id, msg.ReplyToMessage.From.Id, untilDate: DateTime.UtcNow.AddMinutes(duration));
        await botClient.SendMessage(msg.Chat.Id, $"Пользователь {msg.ReplyToMessage.From.FirstName} успешно забанен!", ParseMode.Html);
    }
    public async Task UnBanUser(ITelegramBotClient botClient, Message msg)
    {
        var member = await botClient.GetChatMember(msg.Chat.Id, msg.From.Id);
        if (msg.ReplyToMessage is null) return;
        if (member.Status != ChatMemberStatus.Administrator && member.Status != ChatMemberStatus.Creator)
        {
            await botClient.SendMessage(msg.Chat.Id, "У вас недостаточно прав чтобы использовать эту комманду.", ParseMode.Html);
            return;
        }
        await botClient.UnbanChatMember(msg.Chat.Id, msg.ReplyToMessage.From.Id);
        await botClient.SendMessage(msg.Chat.Id, $"Пользователь {msg.ReplyToMessage.From.FirstName} успешно разбанен!", ParseMode.Html);
    }
}