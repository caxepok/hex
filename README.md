# hex
Решение для хакатона для отслеживания контейнеров на складах.
 - .NET Core, может запускаться как на Windows так и на Linux, в docker контейнерах или без
 - ASP.NET Core - движок web-приложения
 - Entity Framework Core - ORM для базы данных
 - PostgreSQL - движок БД сущностей
 - Clickhouse - движок БД сырых данных телеметрии, быстрый и компактный, must have для IoT задач
 - SignalR - протокол взаимодействия для обмена телеметрий между сервером и клиентов, работает поверх HTTP, что повзоляет использовать SSL шфирование, балансировку, проксирование и все фичи HTTP. 
 Очень легковесный в режиме потоковой отправки телеметрии.
 - BlueZ + DBus - библиотеки для работы с Linux подсистемой Bluetooth.

# Описание проектов решения
## hex.api
Сервер приложения. 
- Принимает соединения от устройств обнаружения маячков через SignalR, откуда получает данные телеметрии и обрабатывает их.
- Рисует web-интерфейс прилоложения
- Содержит REST API для интеграции с внешними сервисами

Основные файлы 
* ```Hubs/ObserverHub.cs``` - SignalR причёмник
* ```Controllers/ApiController.cs``` - REST API
* ```Controllers/HomeController.cs``` - Контроллер web интерфейса
* ```Services/ContainerStateService.cs``` - Сервис принимающий данные из SignalR приёмника, и обрабатывающий пакеты телеметрии. Содержит снимок текущего местоположения\состояния контейнеров, а так же кладёт в базу изменения местоположений\состояний контейнеров
* ```Service/WarehouseService.cs``` - Сервис взаимодйсвия с БД
* ```Models/*.cs``` сущности, используемые в системе

Дамп базы - файл в корне солюшена: dump-hex-202009270415.sql

## hex.common
Общая библиотека устройства обнаружения и сервера содержащая модели пакетов телеметрии

## hex.observer
Приложение для Raspberry PI которое мониторит вокруг себя Bluetooth LE устройства, и отправляет телеметрию на сервер.
Телеметрия включает в себя идентификатор устройства и мощность его сигнала.
Для сокращения объёмов траффика рекомендуется использовать сериализацию в формат MessagePack вместо Json.

- В случае решения с RFID метками приложение обслуживает сканеры RFID меток и шлёт на сервер их идентификаторы и события обнаружения
- В случае использования Bluetooth LE Beacon`ов приложение шлёт данные уровня сигнала до маячка.
При большом количестве устройство обнаружения на территории склада возможно использование триангуляции по уровням сигнала
Это обеспечит точное определение позиции контейнера на площадке

Настройка Raspberry Pi для запуска:

1. Обновиться
```
sudo aupt update && sudo apt upgrade
```
2. Установка .NET Core

.net core https://dotnet.microsoft.com/download
для ARM64 https://docs.microsoft.com/ru-ru/dotnet/core/install/sdk?pivots=os-linux#download-and-manually-install

3. Установка bluetooth стека и нужных библотек

```
sudo apt install pi-bluetooth
sudo apt install bluetooth bluez bluez-tools rfkill
sudo systemctl start bluetooth
sudo systemctl enable bluetooth
```
 
## hex.observer.common
Общие компоненты приложения устройства обнаружения

## hex.observer.simulator
Симулятор устройства обнаружения, подключается к серверу и шлёт каждые 5 секунд пакет телеметрии, содержащий идентификаторы двух маячков контейнеров
Один контейнер остаётся неподвижным, второй контейнер рандомного "прыгает" с места на место.
(Показывает работу устройства слежения, установленного на "Складе производства")

## hex.observer.windows
UWP Windows приложение, которое мониторит вокруг себя Bluetooth LE устройства, и отправляет телеметрию на сервер. 
(Показывает работу устройства слежения, установленного на "Складе отгрузки")