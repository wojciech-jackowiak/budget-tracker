# Budget Tracker 💰

A full-stack personal finance tracking application built with .NET and React, featuring transaction management, recurring transactions, and comprehensive budget analytics.

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![React](https://img.shields.io/badge/React-18-61DAFB?logo=react)](https://reactjs.org/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5-3178C6?logo=typescript)](https://www.typescriptlang.org/)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)

## ✨ Features

### 💼 Core Functionality
- ✅ **Transaction Management** - Create, read, update, and delete income/expense transactions
- ✅ **Recurring Transactions** - Set up automatic monthly/yearly recurring transactions
- ✅ **Budget Summary** - Visual dashboard with income, expenses, net, and savings rate
- ✅ **Category Breakdown** - Detailed spending analysis by category
- ✅ **Multi-User Support** - Secure authentication with isolated user data

### 🔐 Security
- ✅ **JWT Authentication** - Access and refresh token system
- ✅ **BCrypt Password Hashing** - Industry-standard password security
- ✅ **Protected Endpoints** - Role-based authorization
- ✅ **User Data Isolation** - Users can only access their own data

### 🤖 Advanced Features
- ✅ **Background Service** - Automatic monthly transaction generation from recurring templates
- ✅ **Month Navigation** - Browse budget summaries across different months
- ✅ **Responsive Design** - Mobile-first UI that works on all devices

## 🏗️ Architecture

### Monorepo Structure
```
budget-tracker/
├── api/          # Backend - .NET 10 REST API
└── frontend/     # Frontend - React 18 + TypeScript
```

### Backend - Clean Architecture
```
api/
├── src/
│   ├── BudgetTracker.API            # Presentation Layer
│   ├── BudgetTracker.Application    # Application Layer (CQRS)
│   ├── BudgetTracker.Domain         # Domain Layer (Entities, Value Objects)
│   └── BudgetTracker.Infrastructure # Infrastructure Layer (Data, Services)
└── tests/                           # Unit & Integration Tests
```

### Frontend - Component-Based
```
frontend/
├── src/
│   ├── api/         # API client with Axios
│   ├── components/  # Reusable React components
│   ├── pages/       # Page components
│   ├── context/     # React Context (Auth)
│   ├── styles/      # Styled Components theme
│   └── types/       # TypeScript interfaces
```

## 🛠️ Tech Stack

### Backend
- **Framework:** .NET 10, ASP.NET Core Web API
- **Database:** SQL Server with Entity Framework Core 10
- **Architecture:** Clean Architecture, DDD, CQRS
- **Patterns:** MediatR, Repository Pattern, Unit of Work
- **Validation:** FluentValidation
- **Authentication:** JWT (System.IdentityModel.Tokens.Jwt)
- **Password Hashing:** BCrypt.Net
- **Background Jobs:** IHostedService
- **Testing:** xUnit, Moq, FluentAssertions

### Frontend
- **Framework:** React 18
- **Language:** TypeScript 5
- **Build Tool:** Vite
- **Styling:** Styled Components
- **Routing:** React Router v6
- **HTTP Client:** Axios
- **Date Handling:** date-fns
- **Icons:** Lucide React

## 🚀 Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js 20+](https://nodejs.org/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)

### Installation

#### 1. Clone Repository
```bash
git clone https://github.com/wojciech-jackowiak/budget-tracker.git
cd budget-tracker
```

#### 2. Setup Backend
```bash
# Navigate to API project
cd api/src/BudgetTracker.API

# Restore dependencies
dotnet restore

# Update connection string in appsettings.json
# Default: "Server=localhost;Database=BudgetTracker;Trusted_Connection=true;TrustServerCertificate=true;"

# Apply database migrations
dotnet ef database update

# Run API
dotnet run
```

**API will be available at:**
- Swagger UI: https://localhost:7153/swagger
- API Base: https://localhost:7153/api

#### 3. Setup Frontend
```bash
# Navigate to frontend
cd ../../frontend

# Install dependencies
npm install

# Run development server
npm run dev
```

**Frontend will be available at:**
- http://localhost:5173

### First Time Setup

1. **Register Account:**
   - Open http://localhost:5173/register
   - Create username, email, password
   - Automatically logged in after registration

2. **Explore Dashboard:**
   - View budget summary
   - Navigate months with arrow buttons

3. **Add Transactions:**
   - Use Swagger (https://localhost:7153/swagger) for now
   - Frontend transaction form coming soon!

## 📊 API Endpoints

### Authentication
- `POST /api/auth/register` - Create new user account
- `POST /api/auth/login` - Login and receive JWT tokens
- `POST /api/auth/refresh` - Refresh access token

### Transactions
- `GET /api/transactions` - Get all transactions (with filters)
- `GET /api/transactions/{id}` - Get transaction by ID
- `POST /api/transactions/expenses` - Create expense
- `POST /api/transactions/income` - Create income
- `PUT /api/transactions/{id}` - Update transaction
- `DELETE /api/transactions/{id}` - Delete transaction

### Recurring Transactions
- `GET /api/recurringtransactions` - Get all recurring transactions
- `POST /api/recurringtransactions` - Create recurring transaction
- `PUT /api/recurringtransactions/{id}` - Update recurring transaction
- `DELETE /api/recurringtransactions/{id}` - Delete recurring transaction
- `POST /api/recurringtransactions/{id}/pause` - Pause recurring
- `POST /api/recurringtransactions/{id}/resume` - Resume recurring

### Budget
- `GET /api/budget/summary?monthYear=2026-01` - Get monthly budget summary

### Categories
- `GET /api/categories` - Get all categories

**Total: 19 endpoints**

Full API documentation available at `/swagger` when running the API.

## 🧪 Testing

### Backend Tests
```bash
cd api
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true
```

**Test Coverage:**
- Unit Tests: 35+
- Integration Tests: Coming soon
- Test Projects:
  - `BudgetTracker.Application.Tests`
  - `BudgetTracker.Domain.Tests`

### Frontend Tests
```bash
cd frontend
npm run test
```
*Coming soon: Vitest + React Testing Library*

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👤 Author

**Wojciech Jackowiak**

- GitHub: [@wojciech-jackowiak](https://github.com/wojciech-jackowiak)
