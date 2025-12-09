# Budget Tracker

Personal finance tracking application built with modern technologies and best practices.

## Tech Stack

- **Backend:** .NET 10, ASP.NET Core Web API, Entity Framework Core
- **Frontend:** React 18, TypeScript, Vite
- **Database:** PostgreSQL
- **Architecture:** Clean Architecture, CQRS with MediatR

## Project Structure
```
budget-tracker/
├── src/
│   ├── BudgetTracker.Domain/           # Entities, Value Objects
│   ├── BudgetTracker.Application/      # CQRS, MediatR Handlers
│   ├── BudgetTracker.Infrastructure/   # EF Core, Database
│   └── BudgetTracker.WebAPI/           # Controllers, API
├── tests/
│   ├── BudgetTracker.Domain.Tests/
│   ├── BudgetTracker.Application.Tests/
│   └── BudgetTracker.WebAPI.Tests/
└── docker/
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- Node.js 20+
- Docker Desktop
- PostgreSQL (or use Docker)

### Running locally
```bash
# Start database
docker-compose up -d

# Run API
cd src/BudgetTracker.WebAPI
dotnet run

# Run frontend (when ready)
cd src/frontend
npm install
npm run dev
```

## Features

- [ ] User authentication (JWT)
- [ ] Expense tracking
- [ ] Categories (system + custom)
- [ ] Monthly statistics
- [ ] Charts and visualizations