# DevOps Lab 1 — Віртуалізація та контейнеризація

## Опис проекту

Мікросервісний застосунок на **.NET 8** для управління задачами. Складається з двох незалежних Web API сервісів.

### Архітектура

```
┌─────────────────────┐     ┌──────────────────────────┐     ┌──────────────┐
│   TaskService       │────▶│   PostgreSQL (taskdb)    │     │Notification  │
│   :5001             │     │   :5432                  │     │Service :5002 │
│   CRUD задач        │     │   Зберігання задач       │     │In-memory     │
└─────────────────────┘     └──────────────────────────┘     └──────────────┘
```

### Сервіси

| Сервіс | Порт | Опис |
|--------|------|------|
| TaskService | 5001 | CRUD операції з задачами, PostgreSQL |
| NotificationService | 5002 | Надсилання/читання сповіщень, in-memory |
| PostgreSQL | 5432 | База даних для TaskService |

---

## Запуск

### Вимоги
- Docker + Docker Compose

### Запустити всі сервіси

```bash
docker-compose up --build
```

### Swagger UI

- TaskService: http://localhost:5001/swagger
- NotificationService: http://localhost:5002/swagger

---

## API — TaskService (`localhost:5001`)

| Метод | URL | Опис |
|-------|-----|------|
| GET | `/api/tasks` | Всі задачі |
| GET | `/api/tasks/{id}` | Задача за ID |
| POST | `/api/tasks` | Створити задачу |
| PUT | `/api/tasks/{id}` | Оновити задачу |
| DELETE | `/api/tasks/{id}` | Видалити задачу |
| GET | `/health` | Health check |

**Приклад створення задачі:**
```json
POST /api/tasks
{
  "title": "Зробити лабораторну",
  "description": "Docker + docker-compose",
  "priority": 2
}
```

Priority: `0` = Low, `1` = Medium, `2` = High

---

## API — NotificationService (`localhost:5002`)

| Метод | URL | Опис |
|-------|-----|------|
| GET | `/api/notifications` | Всі сповіщення |
| GET | `/api/notifications/recipient/{email}` | За отримувачем |
| POST | `/api/notifications` | Надіслати сповіщення |
| PATCH | `/api/notifications/{id}/read` | Позначити як прочитане |
| GET | `/health` | Health check |

---

## Тести

```bash
# Запустити всі unit тести
dotnet test

# Або окремо
dotnet test TaskService.Tests/
dotnet test NotificationService.Tests/
```

---

## Структура проекту

```
DevOpsLab1.sln
├── TaskService/             # Web API + EF Core + PostgreSQL
│   ├── Controllers/
│   ├── Models/
│   ├── Data/                # AppDbContext
│   ├── Services/
│   ├── Migrations/
│   ├── Dockerfile
│   └── appsettings.json
├── NotificationService/     # Web API + In-memory
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── Dockerfile
│   └── appsettings.json
├── TaskService.Tests/       # xUnit тести
├── NotificationService.Tests/
└── docker-compose.yml
```

---

## GitHub

```bash
git init
git add .
git commit -m "feat: initial DevOps lab 1 - containerized microservices"
git remote add origin https://github.com/<your-username>/devops-lab1.git
git push -u origin main
```
