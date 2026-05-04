# StaffManagement

Проект для управления персоналом: отделы, должности и сотрудники.  
Реализован на **ASP.NET Core 10 Web API** с использованием современных практик и архитектурных шаблонов.

## 🚀 Основные возможности

- Управление отделами (CRUD)
- Управление должностями (CRUD)
- Управление сотрудниками (CRUD)
- Пагинация и фильтрация списков
- Валидация данных на сервере
- Централизованная обработка ошибок
- Автоматическое создание/обновление БД через миграции
- Запуск в Docker (PostgreSQL + API)

## 🧱 Архитектура и стек

- **ASP.NET Core 8** – Web API
- **Entity Framework Core 8** – ORM
- **PostgreSQL 16** – база данных
- **Docker / Docker Compose** – контейнеризация
- **MediatR** – реализация паттерна CQRS
- **AutoMapper** – маппинг между сущностями и DTO
- **FluentValidation** – валидация входных моделей
- **Clean Architecture** – разделение на слои (Domain, Application, Infrastructure, WebAPI)
- **Swagger / OpenAPI** – документирование и тестирование API

## 📁 Структура решения
StaffManagement.sln
├── StaffManagement.Domain – сущности, справочники
├── StaffManagement.Application – CQRS команды/запросы, DTO, интерфейсы
├── StaffManagement.Infrastructure – DbContext, миграции, реализация интерфейсов
├── StaffManagement.WebAPI – точка входа, контроллеры, middleware
└── StaffManagement.Tests – модульные и интеграционные тесты (не полностью реализованы)
