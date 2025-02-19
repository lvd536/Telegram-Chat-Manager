﻿using ChatManager.Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
namespace ChatManager.Manager.Commands.AdminTools;

public static class WarnCommand
{
        public static async Task WarnUser(ITelegramBotClient botClient, Message msg, string description)
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

            if (currentUser.Warns == null) currentUser.Warns = new List<EntityList.Warn>();

            var warn = new EntityList.Warn
            {
                Description = description
            };
            currentUser?.Warns.Add(warn);
            await db.SaveChangesAsync();
            if (currentUser.Warns.Count >= 3)
            {
                currentUser.Warns.Clear();
                await db.SaveChangesAsync();
                await BanCommand.BanUser(botClient, msg, 4320, "3/3 предупреждений");
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

    public static async Task UnWarnUser(ITelegramBotClient botClient, Message msg)
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

            if (currentUser.Warns == null) currentUser.Warns = new List<EntityList.Warn>();

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
}