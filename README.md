# Test
Тестовое задание. Описание в reed me

### Задание

Необходимо сделать десктопный прототип.

Перед игроком стоит пугало с 1000hp. Если hp кончаются, оно исчезает. Если нажать на клавиатуре R пугало восстановится или вылечится. HP должно быть видно.
Также пугало может намокать по шкале от 0 до 100 и гореть. Когда пугало мокрое, оно имеет синий оттенок, когда горит — красный, 
*Можно намутить hp-bar и шкалу мокрости, меняющий цвет в зависимости от оставшегося hp.

Рядом с игроком в воздухе висит пара пистолетов и две пары камней: огненные и водные.
Игрок может ходить. Он может брать камни или пистолет в левую или правую руку кнопками Q и E соответственно. Повторное нажатие отпускает предмет.

При помощи левой кнопки мыши можно использовать предмет в левой руке. С правой аналогично.

Пистолет может стрелять и наносить 20 урона за простое попадание (хит-скан или прожектель, не важно, как будет удобно), за попадание по мокрому на 10 урона меньше и на 10 больше за попадание по горящему.

Огненный камень позволяет пускать огонь из  руки (простенькая система частиц с эмиссией в 10 частиц/секунду). Попадая по врагу частица наносит 1 урона и поджигает его/обновляет горение. Если цель мокрая, то не поджигает его, а сушит на одну единицу. Когда пугало загорается, оно теряет 5hp/секунду в последующие 10 секунд.

Водный камень выпускает водный шарик, который при касании тушит огонь и мочит цель на 10 единиц. При этом сам шарик исчезает (даже если ничего не намочил). Движется он, как брошенный мячик.

Требуемая версия Unity: 2020.3.20
В каком виде нужно прислать решение: билд + проект на гитхабе + сопроводительный ролик, как это все работает.

### Видео с обьяснением:
https://www.youtube.com/watch?v=gJD0DbggER8&ab_channel=AndreyDioX
