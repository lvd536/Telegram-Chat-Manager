﻿using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatManager.Manager.Commands.Games;

public static class QuoteCommand
{
    private static readonly string[] quotes = new string[] {
            "Любовь — это когда хочется не только в душу, но и под юбку.",
            "Жизнь как член: то встает, то падает, а ты просто терпишь.",
            "Хороший секс — это как борщ: без специй не то.",
            "Девки как пиво: открываешь, а там половина пены.",
            "Мужики — как трусы: чем проще, тем удобнее.",
            "Женщины любят ушами, а мужчины — глазами, пока не дойдет до постели.",
            "Счастье — это когда утром просыпаешься не один, а с кем-то горячим.",
            "Любовь слепа, но в темноте все кошки серые.",
            "Жизнь — это игра: кто-то трахается, а кто-то смотрит.",
            "Настоящий мужчина — это тот, кто встает даже после третьей бутылки.",
            "Я не толстый, просто гравитация ко мне слишком привязалась.",
            "Диета — это когда ешь салат и мечтаешь о шашлыке.",
            "Работа — это место, где утром хочется спать, а вечером — жить.",
            "Я не пью, я просто проверяю, сколько печень выдержит.",
            "Мой кот думает, что он царь, а я его подданный с миской.",
            "Жизнь — как Wi-Fi: вроде сигнал есть, а связи нет.",
            "Я не опаздываю, я даю другим шанс прийти первыми.",
            "Деньги — как лук: открываешь кошелек, а там слёзы.",
            "Любовь — это когда ты готов делить с ней последнюю картошку фри.",
            "Утро доброе только в рекламе кофе.",
            "Ты не тупой, просто мозг у тебя в режиме ожидания.",
            "Некоторые люди рождены, чтобы напоминать нам, как хорошо мы живем без них.",
            "Твоя жизнь — это ошибка природы, которую я бы исправил.",
            "Улыбайся шире, может, хоть зубы отвлекут от твоей тупости.",
            "Ты как комар: раздражаешь, а прибить жалко.",
            "Не зли меня, я и так еле сдерживаюсь, чтобы не сказать правду.",
            "Ты не бесполезный, ты просто декоративный.",
            "Мир был бы лучше, если бы некоторые молчали навсегда.",
            "Твоё мнение как мусор: его много, а толку нет.",
            "Ты не уникален, ты просто еще один сбой системы.",
            "Доброта — это когда помогаешь, даже если никто не видит.",
            "Самое ценное в жизни — это люди, которые рядом, когда всё рушится.",
            "Улыбка — это бесплатный подарок, который делает день лучше.",
            "Счастье — это когда твои близкие здоровы и рядом.",
            "Любовь — это не брать, а отдавать без остатка.",
            "Мир начинается с того, что ты делаешь для других.",
            "Дари тепло, даже если сам замерзаешь.",
            "Каждый день — это шанс сделать кого-то счастливее.",
            "Добро — это не слова, а поступки, которые остаются в сердце.",
            "Живи так, чтобы люди вспоминали тебя с улыбкой.",
            "Россия — это не просто земля, это душа, которая не сломается.",
            "Сила России в её людях, а не в тех, кто против неё.",
            "Запад гниет, а Россия стоит, как скала.",
            "Русский дух — это когда бьют, а ты встаешь сильнее.",
            "Россия не сдаётся, потому что сдаются только слабые.",
            "Наша страна — это щит, который защищает правду.",
            "Пусть враги боятся: Россия всегда на шаг впереди.",
            "Русские не прощают предательства, но всегда дают шанс.",
            "Россия — это история, написанная кровью и победой.",
            "Мы не выбираем войну, но всегда её заканчиваем.",
            "Жизнь — это комедия для тех, кто смотрит, и трагедия для тех, кто живёт.",
            "Смерть — это просто способ похудеть навсегда.",
            "Любовь — это когда тебя бросают, а ты всё равно покупаешь цветы.",
            "Оптимизм — это когда падаешь в яму и думаешь, что это спа.",
            "Работа — это когда продаешь душу, чтобы купить еду.",
            "Дети — это маленькие люди, которые напоминают, что контрацепция — это важно.",
            "Жизнь коротка, но достаточно длинна, чтобы всё испортить.",
            "Старики — это те, кто дожил до момента, когда можно всех ненавидеть.",
            "Брак — это когда любишь её больше, чем её нытьё.",
            "Надежда умирает последней, но я её уже похоронил.",
            "Жизнь — это шахматы: один ход, и ты в жопе.",
            "Успех — это когда падаешь семь раз, а встаёшь восемь.",
            "Мечты — это планы, которые ты никогда не выполнишь.",
            "Время — это то, что уходит, пока ты читаешь этот список.",
            "Смысл жизни в том, чтобы найти смысл там, где его нет.",
            "Друзья — это те, кто знает твои косяки и всё равно рядом.",
            "Зависть — это когда чужое счастье бесит больше, чем своё горе.",
            "Судьба — это когда всё идёт не по плану, а ты делаешь вид, что так и надо.",
            "Свобода — это когда тебе плевать на всех.",
            "Настоящий герой — это тот, кто встаёт с дивана.",
            "Ты не лузер, просто победитель в номинации 'все плохо'.",
            "Женщины — как вино: с возрастом либо лучше, либо уксус.",
            "Если жизнь — это игра, то я явно пропустил обучение.",
            "Русский народ — это когда бьют, а ты улыбаешься в ответ.",
            "Смерть — это просто выходной, который ты не заказывал.",
            "Любовь — это когда сердце бьётся, а мозг молчит.",
            "Работа — это когда утром жалеешь, что родился.",
            "Добро побеждает зло, но только в сказках.",
            "Россия — это страна, где невозможное становится традицией.",
            "Жизнь — как порно: ждешь экшн, а получаешь рекламу.",
            "Ты не жирный, просто мир тебя обнимает сильнее.",
            "Злость — это когда хочется всех убить, но лень вставать.",
            "Друзья — это те, кто знает, где ты прячешь пиво.",
            "Счастье — это когда тебе не звонят с работы.",
            "Политика — это когда воруют, а ты аплодируешь.",
            "Черный юмор — это когда смешно, а потом стыдно.",
            "Мужик — это тот, кто падает, но всегда встает.",
            "Женщина — это загадка, которую лучше не разгадывать.",
            "Россия не падает, она просто приседает перед прыжком.",
            "Жизнь — это когда каждый день как пятница 13-е.",
            "Любовь — это когда готов терпеть её борщ.",
            "Работа — это способ понять, что выходные — лучшее в жизни.",
            "Доброта — это когда помогаешь, а потом жалеешь.",
            "Запад боится России, потому что зеркало пугает.",
            "Смерть — это когда наконец-то выспишься.",
            "Деньги — это бумага, за которую продаешь душу.",
            "Смех — это когда плакать уже не можешь.",
            "Судьба — это когда всё идёт к черту, а ты улыбаешься.",
            "Свобода — это когда тебе никто не нужен.",
            "Жизнь — это когда проснулся и уже устал.",
            "Ты не дурак, просто мозг у тебя в отпуске.",
            "Любовь — это когда хочется её задушить, но целуешь.",
            "Работа — это когда мечтаешь о пенсии в 25.",
            "Добро — это когда делаешь, а потом думаешь 'зачем'.",
            "Россия — это когда весь мир против, а нам норм.",
            "Смерть — это когда больше не надо платить налоги.",
            "Друзья — это те, кто выпьет с тобой и не сдаст.",
            "Счастье — это когда телефон не разрядился.",
            "Зависть — это когда чужая тачка круче твоей.",
            "Жизнь — это когда каждый день как русская рулетка.",
            "Ты не старый, просто опытный в нытье.",
            "Любовь — это когда она орёт, а ты молчишь.",
            "Работа — это когда выходной — как отпуск.",
            "Доброта — это когда помогаешь, а тебя посылают.",
            "Россия — это когда санкции, а мы всё равно живём.",
            "Смерть — это просто конец подписки на жизнь.",
            "Деньги — это то, что всегда уходит к другим.",
            "Смех — это когда упал, а все снимают.",
            "Судьба — это когда всё плохо, но ты привык.",
            "Свобода — это когда можешь сказать 'пошли все'.",
            "Жизнь — это когда ждешь пятницу, а уже суббота.",
            "Ты не лентяй, просто чемпион по прокрастинации.",
            "Любовь — это когда она ушла, а ты всё равно ждёшь.",
            "Работа — это когда начальник думает, что ты робот.",
            "Добро — это когда помогаешь, а потом плюёшь.",
            "Россия — это когда весь мир в шоке, а мы в порядке.",
            "Смерть — это когда больше не надо вставать в 7.",
            "Друзья — это те, кто знает, какой ты козёл, и всё равно рядом.",
            "Счастье — это когда холодильник полный.",
            "Зависть — это когда у соседа трава зеленее.",
            "Жизнь — это когда каждый день как день сурка.",
            "Ты не толстый, просто слишком много души.",
            "Любовь — это когда она злая, а ты всё равно её любишь.",
            "Работа — это когда выходные — как мираж.",
            "Доброта — это когда помогаешь и жалеешь об этом.",
            "Россия — это когда весь мир против, а мы побеждаем.",
            "Смерть — это когда можно наконец-то расслабиться.",
            "Деньги — это то, что всегда заканчивается в понедельник.",
            "Смех — это когда жизнь бьёт, а ты ржёшь.",
            "Судьба — это когда всё идёт не так, а ты терпишь.",
            "Свобода — это когда можешь плюнуть на всё.",
            "Жизнь — это когда проснулся и сразу хочешь спать.",
            "Ты не дурак, просто слишком честный с собой.",
            "Любовь — это когда она ушла, а ты всё ещё её жаришь в мыслях.",
            "Работа — это когда мечтаешь о больничном.",
            "Добро — это когда делаешь, а потом жалеешь.",
            "Россия — это когда весь мир в панике, а мы пьём чай.",
            "Смерть — это когда больше не надо платить за интернет.",
            "Друзья — это те, кто всегда поможет спрятать бутылку.",
            "Счастье — это когда никто не трогает.",
            "Зависть — это когда у всех отпуск, а ты работаешь.",
            "Жизнь — это когда каждый день как испытание.",
            "Ты не старый, просто слишком мудрый для этого мира.",
            "Любовь — это когда она орёт, а ты её обнимаешь.",
            "Работа — это когда начальник — твой личный кошмар.",
            "Доброта — это когда помогаешь и ждёшь подвоха.",
            "Россия — это когда весь мир тонет, а мы плывём.",
            "Смерть — это когда больше не надо терпеть людей.",
            "Деньги — это то, что уходит быстрее, чем приходит.",
            "Смех — это когда упал, а все ржут, кроме тебя.",
            "Судьба — это когда всё плохо, а ты всё равно идёшь.",
            "Свобода — это когда можешь сказать правду в лицо.",
            "Жизнь — это когда каждый день как лотерея.",
            "Ты не лентяй, просто мастер по отдыху.",
            "Любовь — это когда она ушла, а ты всё ещё готовишь на двоих.",
            "Работа — это когда выходной — как чудо.",
            "Добро — это когда помогаешь и ждёшь, что тебя кинут.",
            "Россия — это когда весь мир против, а мы смеёмся.",
            "Смерть — это когда больше не надо вставать по будильнику.",
            "Друзья — это те, кто знает твои слабости и всё равно рядом.",
            "Счастье — это когда телефон на зарядке и ты тоже.",
            "Зависть — это когда у всех выходной, а ты пашёшь.",
            "Жизнь — это когда каждый день как новый уровень ада.",
            "Ты не толстый, просто слишком много любви к еде.",
            "Любовь — это когда она злая, а ты всё равно её хочешь.",
            "Работа — это когда начальник думает, что ты его раб.",
            "Доброта — это когда помогаешь и потом жалеешь об этом.",
            "Россия — это когда весь мир в кризисе, а мы в строю.",
            "Смерть — это когда больше не надо платить за свет.",
            "Деньги — это то, что всегда уходит к другим.",
            "Смех — это когда жизнь пинает, а ты улыбаешься.",
            "Судьба — это когда всё идёт к черту, а ты держишься.",
            "Свобода — это когда можешь плюнуть на всех.",
            "Жизнь — это когда проснулся и сразу хочешь пива.",
            "Ты не дурак, просто слишком добрый для этого мира.",
            "Любовь — это когда она ушла, а ты всё ещё её ждёшь.",
            "Работа — это когда выходные — как миф.",
            "Добро — это когда помогаешь, а потом плюёшь на это.",
            "Россия — это когда весь мир в шоке, а мы строим планы.",
            "Смерть — это когда наконец-то можно выключить будильник.",
            "Друзья — это те, кто знает, где ты прячешь заначку.",
            "Счастье — это когда холодильник полный, а ты нет.",
            "Зависть — это когда у всех отпуск, а ты в офисе.",
            "Жизнь — это когда каждый день как бой с самим собой.",
            "Ты не старый, просто слишком опытный в жизни.",
            "Любовь — это когда она орёт, а ты её целуешь.",
            "Работа — это когда начальник — твой личный демон.",
            "Доброта — это когда помогаешь и ждёшь, что тебя кинут.",
            "Россия — это когда весь мир падает, а мы стоим.",
            "Смерть — это когда наконец-то можно выключить будильник."
        };
    
    public static async Task QuoteCommandAsync(ITelegramBotClient botClient, Message msg)
    {
        Random rnd = new Random();
        int index = rnd.Next(quotes.Length);
        await botClient.SendMessage(msg.Chat.Id, $"Твоя цитата: «{quotes[index]}»", ParseMode.Html);
    }
}