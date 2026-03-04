# Budget Tracker API

Personal finance tracking REST API built with **Clean Architecture**, **CQRS**, and **JWT Authentication**.

## 🎯 Features

### Authentication & Security
- ✅ JWT-based authentication (Access + Refresh tokens)
- ✅ BCrypt password hashing
- ✅ Token rotation for enhanced security
- ✅ Claims-based authorization
- ✅ Protected endpoints with role-based access

### Transaction Management
- ✅ Create income and expense transactions
- ✅ Categorize transactions (system + custom categories)
- ✅ Filter by date, category, and transaction type
- ✅ Automatic monthly grouping

### Budget Analytics
- ✅ Monthly budget summary with category breakdown
- ✅ Income vs Expense analysis
- ✅ Savings rate calculation
- ✅ Budget limit tracking (ready for implementation)
- ✅ Over-budget detection

### API Features
- ✅ RESTful API design
- ✅ Swagger/OpenAPI documentation
- ✅ Global exception handling with RFC 7807 Problem Details
- ✅ FluentValidation for request validation
- ✅ CORS support

---

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:
```
BudgetTracker/
├── Domain/              # Enterprise business rules
│   ├── Entities/        # Core domain entities (User, Transaction, Category)
│   ├── Enums/           # Domain enumerations
│   └── Common/          # Base classes and exceptions
│
├── Application/         # Application business rules
│   ├── Auth/            # Authentication commands (Register, Login, RefreshToken)
│   ├── Transactions/    # Transaction CQRS (Commands/Queries)
│   ├── Budget/          # Budget summary queries
│   ├── Categories/      # Category queries
│   └── Common/          # Interfaces, Behaviors, DTOs, Exceptions
│
├── Infrastructure/      # External concerns
│   ├── Data/            # EF Core DbContext, Configurations, Migrations
│   ├── Services/        # JWT Service, external integrations
│   └── Settings/        # Configuration models
│
└── API/                 # Presentation layer
    ├── Controllers/     # API endpoints
    └── Common/          # Global exception handler
```

### Design Patterns
- **CQRS** (Command Query Responsibility Segregation) with MediatR
- **Repository Pattern** via EF Core DbContext
- **Dependency Injection** throughout all layers
- **Options Pattern** for configuration
- **Pipeline Behavior** for cross-cutting concerns (validation)

---

## 🚀 Tech Stack

### Backend
- **.NET 10** - Latest .NET framework
- **ASP.NET Core Web API** - REST API framework
- **Entity Framework Core 10** - ORM for database access
- **SQL Server** - Relational database
- **MediatR** - CQRS implementation
- **FluentValidation** - Input validation
- **BCrypt.Net** - Password hashing
- **Swashbuckle** - Swagger/OpenAPI documentation

### Testing
- **xUnit** - Unit testing framework
- **FluentAssertions** - Fluent assertion library
- **InMemory Database** - Integration testing

---

## 📋 Prerequisites

- .NET 10 SDK
- SQL Server 2022 (or SQL Server Express)
- Visual Studio 2022 or VS Code (optional)
- SQL Server Management Studio (optional)

---

## ⚙️ Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/yourusername/budget-tracker.git
cd budget-tracker/api
```

### 2. Configure Database

Update connection string using User Secrets:
```bash
cd src/BudgetTracker.API
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=BudgetTracker;Integrated Security=true;TrustServerCertificate=true"
```

### 3. Apply Migrations
```bash
cd src/BudgetTracker.Infrastructure
dotnet ef database update --startup-project ../BudgetTracker.API
```

This will create the database and seed 20 system categories.

### 4. Run the API
```bash
cd src/BudgetTracker.API
dotnet run
```

API will be available at:
- **Swagger UI:** http://localhost:5000/swagger

---

## 🔑 API Endpoints

### Authentication

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/register` | Register new user | ❌ |
| POST | `/api/auth/login` | Login with credentials | ❌ |
| POST | `/api/auth/refresh` | Refresh access token | ❌ |

### Transactions

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/transactions/expenses` | Create expense | ✅ |
| POST | `/api/transactions/income` | Create income | ✅ |
| GET | `/api/transactions` | Get transactions with filters | ✅ |
| GET | `/api/transactions/{id}` | Get transaction by ID | ✅ |

### Budget

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/budget/summary` | Get monthly budget summary | ✅ |

### Categories

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/categories` | Get all categories | ✅ |

---

## 📖 Usage Examples

### Register a new user
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username": "john", "email": "john@example.com", "password": "SecurePass123!"}'
```

### Login and get tokens
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email": "john@example.com", "password": "SecurePass123!"}'
```

### Create an expense
```bash
curl -X POST http://localhost:5000/api/transactions/expenses \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN" \
  -d '{"categoryId": 4, "amount": 150.50, "type": 1, "description": "Weekly groceries", "date": "2026-01-23T16:00:00Z"}'
```

### Get monthly budget summary
```bash
curl -X GET "http://localhost:5000/api/budget/summary?monthYear=2026-01" \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN"
```

---

## 🧪 Running Tests
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/BudgetTracker.Application.Tests
```

---

## 📊 Database Schema

### Main Tables

- **Users** - User accounts with authentication data
- **Categories** - Transaction categories (system + custom)
- **Transactions** - Income and expense records
- **BudgetLimits** - Monthly spending limits per category
- **RefreshTokens** - JWT refresh tokens with rotation tracking
- **RecurringTransactions** - Scheduled recurring transactions

### Key Relationships

- User → Transactions (1:N)
- User → Categories (1:N for custom categories)
- User → BudgetLimits (1:N)
- Category → Transactions (1:N)

---

## 🔒 Security Features

- **Password Hashing:** BCrypt with auto-salt
- **JWT Tokens:** 15-minute access tokens, 7-day refresh tokens
- **Token Rotation:** Refresh tokens are rotated on use
- **Claims-Based Auth:** User ID extracted from JWT claims
- **User Isolation:** Users can only access their own data
- **CORS:** Configurable for frontend integration

---

## 🛠️ Development

### Adding a new migration
```bash
cd src/BudgetTracker.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../BudgetTracker.API
dotnet ef database update --startup-project ../BudgetTracker.API
```

---

## 📝 Configuration

### JWT Settings (appsettings.json)
```json
{
  "JwtSettings": {
    "SecretKey": "your-secret-key-minimum-32-characters",
    "Issuer": "BudgetTrackerAPI",
    "Audience": "BudgetTrackerClient",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  }
}
```

---

## 🚧 Roadmap

### Implemented ✅
- [x] User authentication (Register/Login)
- [x] JWT-based authorization
- [x] Transaction CRUD operations
- [x] Monthly budget summary
- [x] Category management
- [x] Input validation
- [x] Exception handling
- [x] Unit tests
- [x] Swagger documentation

### Planned 🔜
- [ ] Update/Delete transaction endpoints
- [ ] Recurring transactions processor
- [ ] Budget limit warnings
- [ ] Export transactions (CSV/PDF)
- [ ] Email notifications
- [ ] Docker deployment
- [ ] Frontend (React)

---

## 📄 License

This project is licensed under the MIT License.

---
---

**Built with .NET 10 and Clean Architecture principles**
