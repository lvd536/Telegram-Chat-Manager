using ChatManager.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace ChatManager;
internal class Program
{
    private static CancellationTokenSource _cts = null!;
    private static TelegramBotClient _bot = null!;
    private static async Task Main()
    {
        _cts = new CancellationTokenSource();
        _bot = new TelegramBotClient("7558769675:AAFC_k3EIeaL2FxdpEHQN9mvPhqVQarolEM", cancellationToken: _cts.Token);
        var onMessageService = new OnMessageService(_bot);
        var onCallbackQuery = new OnCallbackQueryService(_bot);
        var onErrorService = new OnErrorService(_cts);
        var me = await _bot.GetMe();
        _bot.OnMessage += onMessageService.OnMessage;
        _bot.OnUpdate += onCallbackQuery.OnCallbackQuery;
        _bot.OnError += onErrorService.OnError;
        Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
        Console.ReadLine();
        _cts.Cancel();
    }
}