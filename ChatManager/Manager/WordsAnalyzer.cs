using ChatManager.Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatManager.Manager;

public class WordsAnalyzer
{
    public async Task MessageAnalyzer(ITelegramBotClient botClient, Message msg)
    {
        if (msg.Text is null || msg.Text.StartsWith('/')) return;
        var member = await botClient.GetChatMember(msg.Chat.Id, msg.From.Id);
        if (member.Status != ChatMemberStatus.Administrator && member.Status != ChatMemberStatus.Creator)
        {
            await botClient.SendMessage(msg.Chat.Id, "У вас недостаточно прав чтобы использовать эту комманду.",
                ParseMode.Html);
            return;
        }
        using (ApplicationContext db = new ApplicationContext())
        {
            var data = db.Chats
                .Include(w => w.Words)
                .FirstOrDefault(x => x.ChatId == msg.Chat.Id);
            if (data is null)
            {
                await DbMethods.InitializeUserAsync(msg);
                data = db.Chats
                    .Include(w => w.Words)
                    .FirstOrDefault(x => x.ChatId == msg.Chat.Id);
            }
            if (data.Words == null) data.Words = new List<Word>();
            
            if (data.Words.Any(w => w.BlockWord.ToLower().Contains(msg.Text.ToLower())))
            {
                await botClient.DeleteMessage(msg.Chat.Id, msg.Id);
                await botClient.SendMessage(msg.Chat.Id, $"Сообщение с содержанием <em>{msg.Text}</em> было удалено т.к содержит запрещенное выражение.", ParseMode.Html);
            }
        }
    }
    public async Task AddWord(ITelegramBotClient botClient, Message msg, string word)
    {
        var member = await botClient.GetChatMember(msg.Chat.Id, msg.From.Id);
        if (member.Status != ChatMemberStatus.Administrator && member.Status != ChatMemberStatus.Creator)
        {
            await botClient.SendMessage(msg.Chat.Id, "<b>У вас недостаточно прав чтобы использовать эту комманду!</b>",
                ParseMode.Html);
            return;
        }
        using (ApplicationContext db = new ApplicationContext())
        {
            var data = db.Chats
                .Include(w => w.Words)
                .FirstOrDefault(x => x.ChatId == msg.Chat.Id);
            if (data is null)
            {
                await DbMethods.InitializeUserAsync(msg);
                data = db.Chats
                    .Include(w => w.Words)
                    .FirstOrDefault(x => x.ChatId == msg.Chat.Id);
            }
            if (data.Words == null) data.Words = new List<Word>();

            if (data.Words.Any(w => w.BlockWord.ToLower().Contains(word.ToLower())))
            {
                await botClient.SendMessage(msg.Chat.Id, $"Запрещенное слово уже существует в списке.");
                return;
            }

            var newWord = new Word
            {
                BlockWord = word
            };
            data.Words.Add(newWord);
            await db.SaveChangesAsync();
            await botClient.SendMessage(msg.Chat.Id, $"Слово {word} успешно добавлено в список запрещенных слов!");
        }
    }
}