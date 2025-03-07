﻿using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatManager.Manager.Commands.Games;

public static class QuoteCommand
{
    private static readonly string[] quotes = new string[]
    {
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
        "Смерть — это когда наконец-то можно выключить будильник.",
        "Я так люблю свою работу, что готов работать за еду, лишь бы не видеть это ебаное начальство.",
        "Моя жизнь — как туалетная бумага: длинная, белая и вся в дерьме.",
        "Я так ленив, что даже мой кот охуел от моей лени.",
        "Если бы я был супергероем, моя суперсила была бы спать до пиздеца долго.",
        "Я так хуево готовлю, что даже собака сказала 'бери свою жратву и пиздец'.",
        "Моя девка сказала, что я должен быть романтичнее, так я купил ей хуйню с сердечками.",
        "Я так стар, что помню времена, когда интернет был только у пиздец каких богатых.",
        "Если бы я был президентом, я бы нахуй сделал понедельник выходным.",
        "Я так люблю спать, что готов выебать свою подушку от страсти.",
        "Моя жизнь — как ебаный фильм ужасов, только без бабла на спецэффекты.",
        "Я так хуево пою, что даже в душе мне кричат 'заткни ебало'.",
        "Если бы я был животным, я бы был ленивцем, потому что эти пиздец как спят.",
        "Я такой неуклюжий, что падаю на ровном месте, как ебаный идиот.",
        "Моя девка сказала быть спонтанным, так я нахуй ушел к другой.",
        "Я так люблю пиво, что пью его вместо этой ебаной воды.",
        "Если бы я был миллионером, я бы купил остров и назвал его 'Пиздец Лени'.",
        "Я так хуево танцую, что все думают, что у меня ебаный припадок.",
        "Моя жизнь — как коробка конфет: открываешь, а там сплошное дерьмо.",
        "Я так люблю кровать, что готов лежать в ней до пиздеца.",
        "Если бы я был супергероем, мой враг был бы этот ебаный будильник.",
        "Я так хуево вожу, что даже GPS говорит 'иди нахуй'.",
        "Моя девка сказала быть внимательнее, так я купил ей наушники, чтоб не пиздела.",
        "Я так люблю работу, что готов работать за бесплатно, но только если мне пиздец заплатят.",
        "Если бы я был животным, я бы был пандой — жру и сплю, как ебаный король.",
        "Я такой неуклюжий, что даже тень моя падает, как пиздец.",
        "Ты такой тупой, что даже собаки на тебя смотрят как на ебаное недоразумение.",
        "Твоя рожа такая страшная, что зеркало трещит от пиздеца.",
        "Ты такой жирный, что когда садишься, стул кричит 'пиздец мне'.",
        "Ты такая шлюха, что даже бляди на районе тебе в ебало плюют.",
        "Ты такой слабак, что твой член даже не знает, как встать, ебаный лузер.",
        "Твоя жизнь такая хуйня, что даже смерть говорит 'нахуй мне это'.",
        "Ты такой урод, что мама твоя охуела, когда тебя родила.",
        "Ты такая дура, что даже пиздец умнее тебя на три головы.",
        "Ты такой лузер, что даже в игре в жизнь тебе пиздец приходит.",
        "Твоя башка такая пустая, что там ветер гуляет, как в ебаном поле.",
        "Ты такой никчемный, что даже черви тебя жрать не будут, пиздец.",
        "Ты такая страшная, что твое ебало — лучшее средство от стояка.",
        "Ты такой тупой, что даже до двух сосчитать — для тебя пиздец.",
        "Твоя жизнь такая скучная, что смерть говорит 'нахуй ты мне сдался'.",
        "Ты такой слабак, что даже телефон поднять — для тебя ебаный подвиг.",
        "Ты такая шлюха, что даже собаки тебя ебут, а ты рада.",
        "Ты такой урод, что в цирке тебя бесплатно показывать будут, пиздец.",
        "Твоя башка такая большая, что туда можно складывать весь твой хуевый мозг.",
        "Ты такой ленивый, что даже дышать тебе в пиздец лень.",
        "Ты такая дура, что думаешь, что пиздец — это комплимент.",
        "Ты такой никчемный, что даже кот твой на тебя срать хотел.",
        "Твоя жизнь такая хуйня, что даже Бог охуел, когда тебя создал.",
        "Ты такой тупой, что путаешь жопу с головой, ебаный дебил.",
        "Ты такая страшная, что твое ебало — оружие массового пиздеца.",
        "Ты такой слабак, что даже дверь открыть — для тебя ебаный квест.",
        "Ты пиздец какой молодец, продолжай, и все у тебя будет заебись.",
        "Ты охуенно умный, не сдавайся, пиздец как круто будет.",
        "Ты такой красивый, что все пиздец как завидуют, не слушай хуйню.",
        "Ты сильный, как ебаный танк, все трудности тебе нахуй не страшны.",
        "Ты добрый, пиздец как редко таких встретишь, мир тебя любит.",
        "Ты охуенно креативный, твои идеи — просто пиздец как круты.",
        "Ты надежный, как ебаный бетон, на тебя всегда можно положиться.",
        "Ты веселый, пиздец как смешно с тобой, не меняйся.",
        "Ты трудяга, охуенно целеустремленный, успех тебе в кармане.",
        "Ты щедрый, пиздец как заботливый, все вокруг тебя ценят.",
        "Ты умный, ебать как эрудированный, мозги у тебя золотые.",
        "Ты красивый, пиздец как стильный, все бабы от тебя в ахуе.",
        "Ты сильный, как ебаный медведь, все преграды тебе нахуй.",
        "Ты добрый, пиздец как отзывчивый, помогаешь всем, кто в дерьме.",
        "Ты охуенно изобретательный, твои решения — просто пиздец.",
        "Ты надежный, ебать как ответственный, с тобой не пропадешь.",
        "Ты веселый, пиздец как жизнерадостный, с тобой все оживает.",
        "Ты трудяга, охуенно упорный, не боишься никакого дерьма.",
        "Ты щедрый, пиздец как бескорыстный, всегда выручишь.",
        "Ты умный, ебать как проницательный, видишь все насквозь.",
        "Ты красивый, пиздец как обаятельный, все от тебя в восторге.",
        "Ты сильный, охуенно мужественный, за свое стоишь до конца.",
        "Ты добрый, пиздец как милосердный, заботишься о слабых.",
        "Ты талантливый, ебать как круто творишь, все в ахуе.",
        "Ты преданный, пиздец как надежный, всегда рядом, когда хуйня.",
        "Россия — пиздец какая великая страна, Запад нам нахуй не указ.",
        "Путин — охуенно сильный лидер, держит Россию в кулаке.",
        "Россия всегда была супердержавой, и пиздец кто нас остановит.",
        "Запад хочет нас наебать, но мы им покажем хуй.",
        "Русские не сдаются, пиздец как будем драться до конца.",
        "Россия — ебать какая богатая страна, гордимся до пиздеца.",
        "Путин — охуенный гарант, с ним России пиздец как повезло.",
        "Санкции нам нахуй, Россия выкрутится из любого дерьма.",
        "Запад завидует нашей пиздец какой мощи, и пусть сосут.",
        "Россия — страна возможностей, тут можно пиздец как подняться.",
        "Путин — охуенно мудрый, знает, как России пиздец нужно.",
        "Россия — ебать какой оплот веры, традиции нам нахуй не сломать.",
        "Запад пиздит нам свои ценности, а мы их нахуй посылаем.",
        "Русские — пиздец какой сильный народ, выживем в любом дерьме.",
        "Россия — страна героев, пиздец как чтим наших предков.",
        "Путин — охуенный лидер, правду в ебало всем говорит.",
        "Западу нас не наебать, мы сами знаем, что нам пиздец нужно.",
        "Западные СМИ пиздец врут, а мы правду знаем.",
        "Россия — пиздец какой потенциал, будем процветать нахуй всем.",
        "Путин — охуенный президент, народу своему не даст пиздеца.",
        "Твоя мама такая жирная, что когда она худеет, всем пиздец голодать.",
        "Твоя сестра такая страшная, что ворон ебут от ее ебала.",
        "Твоя девка такая шлюха, что ебется даже с тобой, пиздец.",
        "Твоя жена такая тупая, что думает, секс — это ебаный фитнес.",
        "Твоя бабка такая старая, что динозавры ей пиздец как кланялись.",
        "Твоя дочка такая уродина, что в музее ей место, ебать.",
        "Твоя подруга такая жирная, что машины от нее пиздец разбегаются.",
        "Твоя коллега такая дура, что жопу свою найти не может, пиздец.",
        "Твоя начальница такая стерва, что дьявол ей в ебало плюет.",
        "Твоя соседка такая громкая, что ее стоны до Марса пиздец доходят.",
        "Твоя теща такая злая, что змеи от нее пиздец убегают.",
        "Твоя бывшая такая шлюха, что ее можно в аренду сдать, ебать.",
        "Твоя тетя такая жирная, что на ней можно прыгать, как на ебаном батуте.",
        "Твоя кузина такая страшная, что ее ебало — пиздец контрацептив.",
        "Твоя подруга такая тупая, что до десяти сосчитать — ей пиздец.",
        "Твоя девка такая ленивая, что дышать ей в пиздец лень.",
        "Твоя жена такая уродина, что в цирке ей пиздец как рады.",
        "Твоя сестра такая шлюха, что с твоим корешем ебется, пиздец.",
        "Твоя мама такая старая, что Бога в молодости ебала.",
        "Твоя дочка такая жирная, что как шар летает, ебать мой хуй.",
        "Твоя коллега такая стерва, что ей пиздец оружие не нужно.",
        "Твоя начальница такая тупая, что лево-право путает, ебаная дура.",
        "Твоя соседка такая шумная, что будильник ей в жопу, пиздец.",
        "Твоя теща такая злая, что пугало ей пиздец как завидует.",
        "Твоя бывшая такая шлюха, что сортир общественный ей пиздец.",
        "Твоя тетя такая жирная, что диван под ней кричит 'ебать'.",
        "Твоя кузина такая страшная, что комары от нее пиздец дохнут.",
        "Твоя подруга такая тупая, что плоскую Землю ищет, ебаная идиотка.",
        "Твоя девка такая ленивая, что телефон поднять — ей пиздец.",
        "Твоя жена такая уродина, что от нее стояк падает, пиздец."
    };

    public static async Task QuoteCommandAsync(ITelegramBotClient botClient, Message msg)
    {
        Random rnd = new Random();
        int index = rnd.Next(quotes.Length);
        await botClient.SendMessage(msg.Chat.Id, $"Твоя цитата: «{quotes[index]}»", ParseMode.Html);
    }
}