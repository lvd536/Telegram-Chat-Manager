using ChatManager.Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatManager.Manager.Commands;

public class AdminTools
{
    public async Task MuteUser(ITelegramBotClient botClient, Message msg, int duration, string description)
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
                .ThenInclude(u => u.Mutes)
                .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            var currentUser = userData?.Users?.FirstOrDefault(u => u.UserId == msg.ReplyToMessage.From?.Id);
            if (currentUser is null || userData is null)
            {
                await DbMethods.InitializeUserAsync(msg);
                userData = db.Chats
                    .Include(u => u.Users)
                    .ThenInclude(u => u.Mutes)
                    .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
                currentUser = userData?.Users?.FirstOrDefault(u => u.UserId == msg.ReplyToMessage.From?.Id);
            }

            if (currentUser.Mutes == null) currentUser.Mutes = new List<Mute>();

            var mute = new Mute
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

    public async Task KickUser(ITelegramBotClient botClient, Message msg, string description)
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

            if (currentUser.Kick == null) currentUser.Kick = new Kick();

            currentUser.Kick.Description = description;
            await db.SaveChangesAsync();
            await botClient.BanChatMember(msg.Chat.Id, msg.ReplyToMessage.From.Id);
            await botClient.UnbanChatMember(msg.Chat.Id, msg.ReplyToMessage.From.Id);
            await botClient.SendMessage(msg.Chat.Id,
                $"Пользователь {msg.ReplyToMessage.From.FirstName} успешно кикнут!",
                ParseMode.Html);
        }
    }

    public async Task BanUser(ITelegramBotClient botClient, Message msg, int duration, string description)
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

            if (currentUser.Ban == null) currentUser.Ban = new Ban();

            currentUser.Ban.Description = description;
            await db.SaveChangesAsync();

            await botClient.BanChatMember(msg.Chat.Id, msg.ReplyToMessage.From.Id,
            untilDate: DateTime.UtcNow.AddMinutes(duration));
            await botClient.SendMessage(msg.Chat.Id,
                $"Пользователь {msg.ReplyToMessage.From.FirstName} успешно забанен!",
                ParseMode.Html);
        }
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
        await botClient.SendMessage(msg.Chat.Id,
            $"Пользователь {msg.ReplyToMessage.From.FirstName} успешно разбанен!",
            ParseMode.Html);
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
                .ThenInclude(u => u.Warns)
                .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            var currentUser = userData?.Users?.FirstOrDefault(u => u.UserId == msg.ReplyToMessage.From?.Id);
            if (currentUser is null || userData is null)
            {
                await DbMethods.InitializeUserAsync(msg);
                userData = db.Chats
                    .Include(u => u.Users)
                    .ThenInclude(u => u.Warns)
                    .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
                currentUser = userData?.Users?.FirstOrDefault(u => u.UserId == msg.ReplyToMessage.From?.Id);
            }

            if (currentUser.Warns == null) currentUser.Warns = new List<Warn>();

            var warn = new Warn
            {
                Description = description
            };
            currentUser?.Warns.Add(warn);
            await db.SaveChangesAsync();
            if (currentUser.Warns.Count >= 3)
            {
                currentUser.Warns.Clear();
                await db.SaveChangesAsync();
                await BanUser(botClient, msg, 4320, "3/3 предупреждений");
                await botClient.SendMessage(msg.Chat.Id,
                    $"Пользователь {msg.ReplyToMessage.From?.FirstName} получил 3 предупреждения. Выдал бан на 3 дня.",
                    ParseMode.Html);
            }
            else
            {
                await botClient.SendMessage(msg.Chat.Id,
                    $"Пользователь {msg.ReplyToMessage.From?.FirstName} получил {currentUser.Warns.Count} предупреждение из 3",
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
                .ThenInclude(u => u.Warns)
                .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            var currentUser = userData?.Users?.FirstOrDefault(u => u.UserId == msg.ReplyToMessage.From?.Id);
            if (currentUser is null || userData is null)
            {
                await DbMethods.InitializeUserAsync(msg);
                userData = db.Chats
                    .Include(u => u.Users)
                    .ThenInclude(u => u.Warns)
                    .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
                currentUser = userData?.Users?.FirstOrDefault(u => u.UserId == msg.ReplyToMessage.From?.Id);
            }

            if (currentUser.Warns == null) currentUser.Warns = new List<Warn>();

            currentUser?.Warns.Remove(currentUser.Warns.Last());
            await db.SaveChangesAsync();
            if (currentUser?.Warns.Count <= 0)
            {
                await botClient.SendMessage(msg.Chat.Id,
                    $"Пользователь {msg.ReplyToMessage.From?.FirstName} не имеет предупреждений.", ParseMode.Html);
            }
            else
            {
                await botClient.SendMessage(msg.Chat.Id,
                    $"Успешно снял {msg.ReplyToMessage.From?.FirstName} 1 предупреждение", ParseMode.Html);
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
                .ThenInclude(u => u.Warns)
                .Include(u => u.Users)
                .ThenInclude(u => u.Mutes)
                .Include(u => u.Users)
                .ThenInclude(u => u.Ban)
                .Include(u => u.Users)
                .ThenInclude(u => u.Kick)
                .FirstOrDefault(u => u.ChatId == msg.Chat.Id);
            var currentUser = userData?.Users?
                .FirstOrDefault(u => u.UserId == msg.ReplyToMessage.From?.Id);
            if (currentUser.Ban == null) currentUser.Ban = new Ban();
            if (currentUser.Kick == null) currentUser.Kick = new Kick();
            if (currentUser.Mutes == null) currentUser.Mutes = new List<Mute>();
            if (currentUser.Warns == null) currentUser.Warns = new List<Warn>();
            var banDetails = currentUser?.Ban.Description ?? "Не имеет банов";
            var kickDetails = currentUser?.Kick.Description ?? "Не имеет киков";
            var muteDetails = string.Empty;
            try
            {
                muteDetails = currentUser?.Mutes.Last().Description;
            }
            catch (InvalidOperationException)
            {
                muteDetails = "Нет мутов";
            }
            var message = $"Информация о пользователе: {msg.ReplyToMessage.From?.FirstName}:\n" +
                          $"ID: {msg.ReplyToMessage.From?.Id}\n" +
                          $"Tag: {msg.ReplyToMessage.From?.Username}\n" +
                          $"Причина бана: {banDetails}\n" +
                          $"Причина кика: {kickDetails}\n" +
                          $"Причина мута: {muteDetails}\n" +
                          $"Кол-во предупреждений: {currentUser?.Warns.Count}\n";

            if (currentUser?.Warns != null && currentUser.Warns.Any())
            {
                message += $"Причина послед. пред-я: {currentUser.Warns.Last().Description}";
            }
            else
            {
                message += "Причина послед. пред-я: Нет предупреждений";
            }

            await botClient.SendMessage(msg.Chat.Id, "<blockquote>" + message + "</blockquote>", ParseMode.Html);
        }
    }
}