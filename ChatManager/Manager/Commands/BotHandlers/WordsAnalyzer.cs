﻿using ChatManager.Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatManager.Manager;

public static class WordsAnalyzer
{
    public static async Task MessageAnalyzer(ITelegramBotClient botClient, Message msg)
    {
        if (msg.Text is null || msg.Text.StartsWith('/')) return;
        var message = msg.Text.Split(' ');
        using (ApplicationContext db = new ApplicationContext())
        {
            var data = await DbMethods.GetUserDataAsync(db, msg);
            if (data.Words.Count <= 0) return;
            foreach (var word in message)
            {
                if (data.Words.Any(w => w.BlockWord.ToLower().Contains(word.ToLower())))
                {
                    await botClient.DeleteMessage(msg.Chat.Id, msg.Id);
                    await botClient.SendMessage(msg.Chat.Id,
                        $"Сообщение с содержанием <em>{msg.Text}</em> было удалено т.к содержит запрещенное выражение.",
                        ParseMode.Html);
                    Console.WriteLine($"Удалено: {msg.Text}, совпадение с {word}");
                }
            }
        }
    }
    public static async Task AddWord(ITelegramBotClient botClient, Message msg, string word)
    {
        var member = await DbMethods.GetMemberAsync(botClient, msg);
        if (member.Status != ChatMemberStatus.Administrator && member.Status != ChatMemberStatus.Creator)
        {
            await botClient.SendMessage(msg.Chat.Id, "<b>У вас недостаточно прав чтобы использовать эту комманду!</b>",
                ParseMode.Html);
            return;
        }
        using (ApplicationContext db = new ApplicationContext())
        {
            var data = await DbMethods.GetUserDataAsync(db, msg);

            if (data.Words.Any(w => w.BlockWord.ToLower().Contains(word.ToLower())))
            {
                await botClient.SendMessage(msg.Chat.Id, $"Запрещенное слово уже существует в списке.");
                return;
            }

            var newWord = new EntityList.Word
            {
                BlockWord = word
            };
            data.Words.Add(newWord);
            await db.SaveChangesAsync();
            await botClient.SendMessage(msg.Chat.Id, $"Слово {word} успешно добавлено в список запрещенных слов!");
        }
    }
    public static async Task RemoveWord(ITelegramBotClient botClient, Message msg, string word)
    {
        var member = await DbMethods.GetMemberAsync(botClient, msg);
        if (member.Status != ChatMemberStatus.Administrator && member.Status != ChatMemberStatus.Creator)
        {
            await botClient.SendMessage(msg.Chat.Id, "<b>У вас недостаточно прав чтобы использовать эту комманду!</b>",
                ParseMode.Html);
            return;
        }
        using (ApplicationContext db = new ApplicationContext())
        {
            var data = await DbMethods.GetUserDataAsync(db, msg);
            
            var currentWord = data.Words.FirstOrDefault(w => w.BlockWord.ToLower().Contains(word.ToLower()));
            if (currentWord is null)
            {
                await botClient.SendMessage(msg.Chat.Id, $"Такого слова нет в списке.");
                return;
            }
            data.Words.Remove(currentWord);
            await db.SaveChangesAsync();
            await botClient.SendMessage(msg.Chat.Id, $"Слово {word} успешно удалено из списка запрещенных слов!");
        }
    }
    public static async Task ListWords(ITelegramBotClient botClient, Message msg)
    {
        var member = await DbMethods.GetMemberAsync(botClient, msg);
        if (member.Status != ChatMemberStatus.Administrator && member.Status != ChatMemberStatus.Creator)
        {
            await botClient.SendMessage(msg.Chat.Id, "У вас недостаточно прав чтобы использовать эту комманду.",
                ParseMode.Html);
            return;
        }
        var message = "Список запрещенных слов: ";
        using (ApplicationContext db = new ApplicationContext())
        {
            var data = await DbMethods.GetUserDataAsync(db, msg);
            for (int i = 0; i < data.Words.Count; i++)
            {
                message += $"\n<blockquote>{i}. {data.Words[i].BlockWord}</blockquote>";
            }
            if (data.Words.Count < 1) message += "Пусто";
        }
        await botClient.SendMessage(msg.Chat.Id, message, ParseMode.Html);
    }
}