using ChatManager.Database;
using Microsoft.EntityFrameworkCore;
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
            await botClient.SendMessage(msg.Chat.Id, "У вас недостаточно прав чтобы использовать эту комманду.",
                ParseMode.Html);
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

    public async Task KickUser(ITelegramBotClient botClient, Message msg)
    {
        var member = await botClient.GetChatMember(msg.Chat.Id, msg.From.Id);
        if (msg.ReplyToMessage is null) return;
        if (member.Status != ChatMemberStatus.Administrator && member.Status != ChatMemberStatus.Creator)
        {
            await botClient.SendMessage(msg.Chat.Id, "У вас недостаточно прав чтобы использовать эту комманду.",
                ParseMode.Html);
            return;
        }

        await botClient.BanChatMember(msg.Chat.Id, msg.ReplyToMessage.From.Id);
        await botClient.UnbanChatMember(msg.Chat.Id, msg.ReplyToMessage.From.Id);
        await botClient.SendMessage(msg.Chat.Id, $"Пользователь {msg.ReplyToMessage.From.FirstName} успешно кикнут!",
            ParseMode.Html);
    }

    public async Task BanUser(ITelegramBotClient botClient, Message msg, int duration)
    {
        var member = await botClient.GetChatMember(msg.Chat.Id, msg.From.Id);
        if (msg.ReplyToMessage is null) return;
        if (member.Status != ChatMemberStatus.Administrator && member.Status != ChatMemberStatus.Creator)
        {
            await botClient.SendMessage(msg.Chat.Id, "У вас недостаточно прав чтобы использовать эту комманду.",
                ParseMode.Html);
            return;
        }

        await botClient.BanChatMember(msg.Chat.Id, msg.ReplyToMessage.From.Id,
            untilDate: DateTime.UtcNow.AddMinutes(duration));
        await botClient.SendMessage(msg.Chat.Id, $"Пользователь {msg.ReplyToMessage.From.FirstName} успешно забанен!",
            ParseMode.Html);
    }

    public async Task UnBanUser(ITelegramBotClient botClient, Message msg)
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
        await botClient.SendMessage(msg.Chat.Id, $"Пользователь {msg.ReplyToMessage.From.FirstName} успешно разбанен!",
            ParseMode.Html);
    }

    public async Task WarnUser(ITelegramBotClient botClient, Message msg)
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
                .ThenInclude(u => u.Warn)
                .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            var currentUser = userData?.Users?.FirstOrDefault(u => u.UserId == msg.ReplyToMessage.From?.Id);
            if (currentUser is null || userData is null)
            {
                await DbMethods.InitializeUserAsync(msg);
            }

            currentUser.Warn.Warns++;
            await db.SaveChangesAsync();
            if (currentUser.Warn.Warns >= 3)
            {
                currentUser.Warn.Warns = 0;
                await db.SaveChangesAsync();
                await BanUser(botClient, msg, 4320);
                await botClient.SendMessage(msg.Chat.Id,
                    $"Пользователь {msg.ReplyToMessage.From?.FirstName} получил 3 предупреждения. Выдал бан на 3 дня.",
                    ParseMode.Html);
            }
            else
            {
                await botClient.SendMessage(msg.Chat.Id,
                    $"Пользователь {msg.ReplyToMessage.From?.FirstName} получил {currentUser.Warn.Warns} предупреждение из 3",
                    ParseMode.Html);
            }
        }
    }

    public async Task UnWarnUser(ITelegramBotClient botClient, Message msg)
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
                .ThenInclude(u => u.Warn)
                .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            var currentUser = userData?.Users?.FirstOrDefault(u => u.UserId == msg.ReplyToMessage.From?.Id);
            if (currentUser is null || userData is null)
            {
                await DbMethods.InitializeUserAsync(msg);
            }
            switch (currentUser.Warn.Warns)
            {
                case 1:
                    currentUser.Warn.OneDescription = "Not set";
                    break;
                case 2:
                    currentUser.Warn.TwoDescription = "Not set";
                    break;
                case 3:
                    currentUser.Warn.ThreeDescription = "Not set";
                    break;
            }
            currentUser.Warn.Warns--;
            if (currentUser.Warn.Warns < 0)
            {
                currentUser.Warn.Warns = 0;
                await botClient.SendMessage(msg.Chat.Id,
                    $"Пользователь {msg.ReplyToMessage.From?.FirstName} не имеет предупреждений.", ParseMode.Html);
            }
            else
            {
                await botClient.SendMessage(msg.Chat.Id,
                    $"Успешно снял {msg.ReplyToMessage.From?.FirstName} 1 предупреждение", ParseMode.Html);
            }

            await db.SaveChangesAsync();
        }
    }

    public async Task WarnUser(ITelegramBotClient botClient, Message msg, string description)
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
                .ThenInclude(u => u.Warn)
                .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            var currentUser = userData?.Users?
                .FirstOrDefault(u => u.UserId == msg.ReplyToMessage.From?.Id);
            if (currentUser is null || userData is null)
            {
                await DbMethods.InitializeUserAsync(msg);
            }

            currentUser.Warn.Warns++;
            switch (currentUser.Warn.Warns)
            {
                case 1:
                    currentUser.Warn.OneDescription = description;
                    break;
                case 2:
                    currentUser.Warn.TwoDescription = description;
                    break;
                case 3:
                    currentUser.Warn.ThreeDescription = description;
                    break;
            }
            await db.SaveChangesAsync();
            if (currentUser.Warn.Warns >= 3)
            {
                currentUser.Warn.Warns = 0;
                await db.SaveChangesAsync();
                await BanUser(botClient, msg, 4320);
                await botClient.SendMessage(msg.Chat.Id,
                    $"Пользователь {msg.ReplyToMessage.From?.FirstName} получил 3 предупреждения. Выдал бан на 3 дня.",
                    ParseMode.Html);
            }
            else
            {
                await botClient.SendMessage(msg.Chat.Id,
                    $"Пользователь {msg.ReplyToMessage.From?.FirstName} получил {currentUser.Warn.Warns} предупреждение из 3. Причина: {description}",
                    ParseMode.Html);
            }
        }
    }

    public async Task UserInfo(ITelegramBotClient botClient, Message msg)
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
                .ThenInclude(u => u.Warn)
                .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            var currentUser = userData?.Users?
                .FirstOrDefault(u => u.UserId == msg.ReplyToMessage.From?.Id);
            var message = $"Информация о пользователе: {msg.ReplyToMessage.From?.FirstName}:\n" +
                          $"ID: {msg.ReplyToMessage.From?.Id}\n" +
                          $"Tag: {msg.ReplyToMessage.From?.Username}\n" +
                          $"Кол-во предупреждений: {currentUser.Warn.Warns}\n";
            switch (currentUser.Warn.Warns)
            {
                case 1:
                    message += $"Причина первого предупреждения: {currentUser.Warn.OneDescription}";
                    break;
                case 2:
                    message += $"Причина первого предупреждения: {currentUser.Warn.OneDescription}\n" +
                               $"Причина второго предупреждения: {currentUser.Warn.TwoDescription}";
                    break;
                case 3:
                    message += $"Причина первого предупреждения: {currentUser.Warn.OneDescription}\n" +
                               $"Причина второго предупреждения: {currentUser.Warn.TwoDescription}\n"+
                               $"Причина третьего предупреждения: {currentUser.Warn.ThreeDescription}";
                    break;
            }
            await botClient.SendMessage(msg.Chat.Id, "<blockquote>" + message + "</blockquote>", ParseMode.Html);
        }
    }
}