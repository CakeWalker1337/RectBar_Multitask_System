# RectBar Multitask System (RBMS)
## Описание
Приложение представляет собой систему для учета данных школы и содержит расписание, журнал посещений и панель администрирования.
Интуитивно понятный интерфейс и большое количество подсказок позволяет вести учет данных с комфортом. 
> NOTE: Для начала необходимо установить MySQL-сервер и MySQL Connector, а также создать базу данных с любым названием (в дальнейшем название используется для подключения).

Типы данных системы:
* Группы
* Ученики
* Преподаватели
* Администраторы

## Дополнительные утилиты
В сборку входят дополнительные утилиты:
* DB Private - утилита, создающая приватный файл с логином и паролем от БД. Файл закидывается в папку с программой RBMS.
* SSMS Root Changer - утилита для смены логина и пароля главного администратора системы.
* DB Dropper - утилита для сброса данных в таблицах (можно использовать для создания таблиц). Требует данных о БД (айпи сервера, название БД, имя рут-пользователя, пароль).

## Скриншоты

Окно Авторизации:

![alt text](https://raw.github.com/CakeWalker1337/RectBar_Multitask_System/master/github/screenshots/log.PNG)

Главное окно - Расписание:

![alt text](https://raw.github.com/CakeWalker1337/RectBar_Multitask_System/master/github/screenshots/main1.PNG)

Главное окно - Журнал посещений:

![alt text](https://raw.github.com/CakeWalker1337/RectBar_Multitask_System/master/github/screenshots/main2.PNG)

Главное окно - Админ-панель:

![alt text](https://raw.github.com/CakeWalker1337/RectBar_Multitask_System/master/github/screenshots/main3.PNG)

Окно редактирования - Группа:

![alt text](https://raw.github.com/CakeWalker1337/RectBar_Multitask_System/master/github/screenshots/editmenu2.PNG)

Окно редактирования - Ученик:

![alt text](https://raw.github.com/CakeWalker1337/RectBar_Multitask_System/master/github/screenshots/editmenu1.PNG)

## Технологии
* MySQL
* WPF
