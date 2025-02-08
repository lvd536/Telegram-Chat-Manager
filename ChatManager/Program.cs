using ChatManager.Manager;
using ChatManager.Manager.Commands;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("7371147310:AAEwln2CDIWVzYNTFHMdUwbzyzHod1qgDDQ", cancellationToken: cts.Token);
var me = await bot.GetMe();
var messageHandler = new MessageCounter();
var startCommand = new StartCommand();
var profileCommand = new ProfileCommand();
bot.OnMessage += OnMessage;
bot.OnUpdate += OnCallbackQuery;
bot.OnError += OnError;
Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel();

async Task OnMessage(Message msg, UpdateType type)
{
    await messageHandler.MessageCounterAsync(bot, msg, msg.Type);
    if (msg.Text is null) return;
    var commandParts = msg.Text.Split(' ');
    var command = commandParts[0];
    var argument = commandParts.Length >= 2 ? commandParts[1] : null;
    var defArgument = commandParts.Length >= 3 ? commandParts[2] : null;
    if (msg.Text.StartsWith('/'))
    {
        switch (command)
        {
            case "/start":
                await startCommand.StartCmd(bot, msg);
                break;
            case "/id":
                await bot.SendMessage(msg.Chat.Id, $"ID пользователя {msg.From.FirstName}: {msg.From.Id}", ParseMode.Html);
                break;
            case "/profile":
                await profileCommand.ProfileCmd(bot, msg);
                break;
        }
    }
}

async Task OnCallbackQuery(Update update)
{
    if (update.Type != UpdateType.CallbackQuery) return;
    switch (update.CallbackQuery?.Data)
    {
    }
}

async Task OnError(Exception exception, HandleErrorSource handler)
{
    Console.WriteLine(exception);
    await Task.Delay(2000, cts.Token);
}