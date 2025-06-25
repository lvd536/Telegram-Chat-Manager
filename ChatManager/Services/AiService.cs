using System.Text;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
namespace ChatManager.Services;

public class AiService
{
    private static string _apiKey = string.Empty;
    private static readonly Uri OrUri = new("https://openrouter.ai/api/v1/chat/completions");
    private static ITelegramBotClient _bot = null!;
    public AiService(string apiKey, ITelegramBotClient botClient)
    {
        _apiKey = apiKey;
        _bot = botClient;
    }
    
    public async Task MakeAiRequest(ITelegramBotClient botClient, Message msg)
    {
        var userQuestion = msg.Text?.Replace("/chance", string.Empty);
        if (string.IsNullOrEmpty(userQuestion)) return;
        
        var message = await botClient.SendMessage(msg.Chat.Id,"Model is reasoning...");
        
        var payload = new
        {
            model = "deepseek/deepseek-r1-0528:free",
            messages = new object[]
            {
                new
                {
                    role = "system",
                    content = "Ты - ассистент чата друзей. Помогай во всех их просьбах и отвечамй максимально грамотно и развернуто на все их запросы."
                },
                new
                {
                    role = "user",
                    content = userQuestion
                }
            }
        };

        var json = JsonSerializer.Serialize(payload);
        using var hc = new HttpClient();
        hc.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        var resp = await hc.PostAsync(OrUri, content);
        if (!resp.IsSuccessStatusCode)
        {
            var err = await resp.Content.ReadAsStringAsync();
            await _bot.SendMessage(
                chatId: msg.Chat.Id,
                text: $"❌ Ошибка OpenRouter {(int)resp.StatusCode}:\n{err}"
            );
            return;
        }

        var respJson = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(respJson);
        var answer = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();
        if (string.IsNullOrWhiteSpace(answer))
            answer = "🤖 (модель вернула пустой ответ)";

        await _bot.EditMessageText(
            chatId: message.Chat.Id,
            messageId: message.MessageId,
            text: answer
        );
    }
}