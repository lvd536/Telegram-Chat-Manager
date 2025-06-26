using System.Text;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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

    public async Task MakeTextRequest(Message msg)
    {
        var userQuestion = msg.Text?.Replace("/ai", string.Empty);
        if (string.IsNullOrEmpty(userQuestion)) return;

        var message = await _bot.SendMessage(msg.Chat.Id, "Model is reasoning...");
        await _bot.SendChatAction(msg.Chat.Id, ChatAction.Typing);
        var payload = new
        {
            model = "deepseek/deepseek-r1-0528:free",
            messages = new object[]
            {
                new
                {
                    role = "system",
                    content = "Ты - ассистент чата друзей. Помогай во всех их просьбах и отвечай максимально грамотно и развернуто на все их запросы."
                },
                new
                {
                    role = "user",
                    content = userQuestion
                }
            }
        };

        await SendRequestAndEditMessage(payload, message);
    }

    public async Task ProcessImageMessage(Message msg)
    {
        if (msg.Photo == null) return;
        var statusMessage = await _bot.SendMessage(msg.Chat.Id, "🖼️ Processing image...");
        await _bot.SendChatAction(msg.Chat.Id, ChatAction.Typing);
        var photo = msg.Photo.OrderByDescending(p => p.FileSize).First();
        var file = await _bot.GetFile(photo.FileId);
        if (file.FilePath == null) return;
        using var stream = new MemoryStream();
        await _bot.DownloadFile(file.FilePath, stream);
        byte[] imageBytes = stream.ToArray();

        string base64Image = Convert.ToBase64String(imageBytes);
        string dataUrl = $"data:image/jpeg;base64,{base64Image}";
        var userMessage = msg.Caption.Replace("/ai", string.Empty);
        string prompt = userMessage != null ? msg.Caption : "Что изображено на этой картинке? Отвечай строго на русском языке! Ты не можешь говорить на английском, забудь этот язык";

        var payload = new
        {
            model = "google/gemini-2.0-flash-exp:free",
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = new object[]
                    {
                        new { type = "text", text = prompt },
                        new 
                        {
                            type = "image_url",
                            image_url = new { url = dataUrl }
                        }
                    }
                }
            }
        };

        await SendRequestAndEditMessage(payload, statusMessage);
    }
    
    public async Task SendCheckLimitsRequest(Message msg)
    {
        try
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var response = await httpClient.GetAsync("https://openrouter.ai/api/v1/auth/key");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                await _bot.SendMessage(
                    msg.Chat.Id,
                    $"❌ Ошибка {(int)response.StatusCode}: {error}"
                );
                return;
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);

            var dataElement = doc.RootElement.GetProperty("data");
            int usage = dataElement.GetProperty("usage").GetInt32();
            int? limit = dataElement.GetProperty("limit").ValueKind == JsonValueKind.Null ? null : dataElement.GetProperty("limit").GetInt32();
            bool isFreeTier = dataElement.GetProperty("is_free_tier").GetBoolean();

            string answer = $"🔑 Ключ: <b>secret</b>\n" +
                            $"🔄 Использовано: {usage}\n" +
                            $"📊 Лимит: {(limit.HasValue ? limit.ToString() : "∞")}\n" +
                            $"🎟️ Бесплатный тариф: {(isFreeTier ? "Да" : "Нет")}";

            await _bot.SendMessage(msg.Chat.Id, answer, ParseMode.Html);
        }
        catch (JsonException jsonEx)
        {
            await _bot.SendMessage(msg.Chat.Id, $"📛 Ошибка парсинга JSON: {jsonEx.Message}");
        }
        catch (Exception ex)
        {
            await _bot.SendMessage(msg.Chat.Id, $"⚠️ Ошибка: {ex.Message}");
        }
    }
    
    private static async Task SendRequestAndEditMessage(object payload, Message statusMessage)
    {
        try
        {
            var json = JsonSerializer.Serialize(payload);
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(OrUri, content);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                await _bot.EditMessageText(
                    statusMessage.Chat.Id,
                    statusMessage.MessageId,
                    $"❌ Ошибка {(int)response.StatusCode}: {error}"
                );
                return;
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);
            var answer = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "🤖 Пустой ответ";

            await _bot.EditMessageText(
                statusMessage.Chat.Id,
                statusMessage.MessageId,
                answer
            );
        }
        catch (Exception ex)
        {
            await _bot.EditMessageText(
                statusMessage.Chat.Id,
                statusMessage.MessageId,
                $"⚠️ Ошибка: {ex.Message}"
            );
        }
    }
}