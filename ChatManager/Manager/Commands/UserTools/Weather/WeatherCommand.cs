namespace ChatManager.Manager.Commands.Weather;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public class WeatherCommand
{
    private readonly WeatherService _weatherService;
    private string _city;

    public WeatherCommand()
    {
        _weatherService = new WeatherService("3e9eae6efa142dac8de06fd29fffca12");
    }

    public async Task WeatherCmd(ITelegramBotClient botClient, Message msg)
    {
        var weatherData = await _weatherService.GetWeatherAsync("Samara");
        await botClient.SendMessage(msg.Chat, weatherData.GetFormattedWeather(), ParseMode.Html);
    }
}