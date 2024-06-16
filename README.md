# Учет сотрудников компании
Веб-приложение для учета сотрудников какой-либо компании. Реализован посредством шаблона MVC.

## Содержание
- [Стек проекта](#Стек-проекта)
- [Функциональность проекта](#Функциональность-проекта)
- [Описание сервисов](#Описание-сервисов)
  - [Идентификация](#Account)
  - [Сотрудники](#Employee)
- [База данных](#База-данных)
- [Docker](#Docker)
- [UnitTests](#UnitTests)
- [Запуск проекта](#Запуск-проекта)
- [P.S.](#P.S.)
---
### <a id="Стек-проекта">Стек проекта</a>
* [Asp Net Core MVC](https://learn.microsoft.com/ru-ru/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-8.0)
* [Entity Framework Core](https://docs.microsoft.com/ru-ru/ef/core/)
* [MS Unit Tests](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest)
* [Docker](https://www.docker.com/)
* [AutoMapper](https://automapper.org/)
---
### <a id="Функциональность-проекта">Функциональность проекта</a>
1. Сервис Account
    - Аутентификация и авторизация

2. Сервис Employee
    - Добавление пользователя
    - Получение списка пользователей (с учетом сортировки и фильтрации)
    - Удаление пользователя
    - Получение id пользователя

---
### <a id="Описание-сервисов">Описание сервисов</a>
Каждый сервис имеет свои представления по методам. Данные попадают в БД через Automapper. Имеется общий шаблон веб-страницы. Скрипты и стили подключаются отдельно. Сделана фильтрация сортировка и пагинация данных о сотрудниках.   
База данных использует Postgres. Контроллеры и сервисы логируют информацию по валидным/не валидным операциям.

#### <a id="Account">Account</a>
> https://localhost:7129/Account/Register или https://localhost:7129/Account/Login

Сервис осуществляет контроль пользователей в системе. Процесс происходит с помощью подключения библиотеки с пакетом Identity.EntityFrameworkCore.
 * Регистрация 
 С помощью userManager библиотеки Identity выполняется регистрация пользователя и помещения его в базу данных. 
 * Аутентификация и авторизация  
 С помощью signInManager библиотеки Identity проверяются входные данные (логин и пароль) и выполняется вход пользователя в систему.
 * Выход
 С помощью signInManager библиотеки Identity выполняется выход из системы.

#### <a id="Employee">Employee</a>
> https://localhost:7129/Employee  

Сервис выполняет администрирование сотрудников. 
 * Получить список сотрудников  
 Метод возвращает всех сотрудников. В полях можно укзаать сортировку по столбцам или поиск определенной информации по сотрудникам.
 * Добавить сотрудника 
 Метод добавляет сотрудника в БД.  
 * Редактировать сотрудника 
Метод редактирует информацию о сотруднике.  
 * Удалить сотрудника 
Метод удаляет сотрудника из базы данных. Выполнено с модальным окном для подтверждения удаления.  
---

### <a id="База-данных">База данных</a>
> http://localhost:8080  

В проекте используются 2 базы данных. 

 * Для Сервиса Account
 * Для Сервиса Employee

Для входа в adminer введите следующие данные:

* Движок - PostgreSQL
* Сервер - db
* Имя пользователя - postgres
* Пароль - example
* База данных - employeeStaff

Подход проектирования БД: Code first  
Тип хранения данных: PostgreSQL & Docker  
ORM: Entity Framework Core  

---
### <a id="Docker">Docker</a>
Для запуска подготовлен docker-compose файл в корне решения. dockerfile располагается в проекте.

---
### <a id="UnitTests">UnitTests</a>
Проект EmployeeTests содержит Unit-тесты на функционал сервисов и контроллеров Account и Employee.

---
### <a id="Запуск-проекта">Запуск проекта</a>
<!-- #### 1-й вариант   
Можно запуститься через программу docker. Для запуска в терминале необходимо прописать следующие команды:  
 * git clone <адрес репозитория на Github>
 * docker-compose up    

После этого перейдите по указанным адресам.
#### 2-й вариант  -->
Можно запуститься через среду разработки. В docker запустить контейнеры с базой данных, adminer. Затем в Visual Studio (или другой IDE) запустить проект.

Для запуска необходимо прописать следующие команды:  
 * git clone <адрес репозитория на Github>
 * docker-compose up <Имя контейнера>

---
### <a id="P.S.">P.S.</a>
Это мой второй проект, так что прошу привнести максимально объективную критику.