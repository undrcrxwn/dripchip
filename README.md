# DripChip RESTful API

Задание первого этапа конкурса «Прикладное программирование if...else», проводимого в рамках международной олимпиады в сфере информационных технологий «IT-Планета 2023».

### Структура решения
- `src/DripChip.Api` — слой представления, ASP.NET Core Web Api (.NET 8.0), REST;
- `src/DripChip.Infrastructure` — слой инфраструктуры, PostgreSQL (EF Core Npgsql), ASP.NET Core Identity;
- `src/DripChip.Application` — use-cases, feature-sliced CRUD с использованием Mediator и FluentValidation;
- `src/DripChip.Domain` — доменный слой, POCO сущности.

Луковая архитектура с feature slices в application слое, расщеплением пользователя на инфраструктурного user (ASP.NET Core Identity) и доменную сущность account. Application слой зависит от EF Core, специфика СУБД реализована в слое persistence.

### Зависимости
[FluentValidation](https://github.com/FluentValidation/FluentValidation),
[Mapster](https://github.com/MapsterMapper/Mapster),
[Mediator](https://github.com/martinothamar/Mediator),
[Serilog](https://github.com/serilog/serilog),
[Npgsql](https://www.npgsql.org/efcore/),
[Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore),
ASP.NET Core, ASP.NET Core Identity, EF Core.