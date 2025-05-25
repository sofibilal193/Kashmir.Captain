# Kashmir.Captain - N-Tier ASP.NET Core Web API

This repository contains a well-structured **N-Tier architecture** for an ASP.NET Core Web API project named `Kashmir.Captain`. It separates responsibilities cleanly into multiple layers for better scalability, testability, and maintainability.

---

## 🧱 Project Structure

Kashmir.Captain.sln
│
├── src/
│ ├── Kashmir.Captain.API --> Web API Layer (Controllers, Middleware, Swagger)
│ ├── Kashmir.Captain.Application --> Application Layer (DTOs, Interfaces, Services)
│ ├── Kashmir.Captain.Domain --> Domain Layer (Entities, Enums, ValueObjects)
│ └── Kashmir.Captain.Infrastructure --> Infrastructure Layer (EF Core, Repositories, External Services)
│
└── tests/
└── Kashmir.Captain.Tests --> Unit & Integration Tests

---

## 🚀 Getting Started

### 1. Create Solution

```bash
dotnet new sln -n Kashmir.Captain
```

2. Create Projects

# Create API Project

dotnet new webapi -n Kashmir.Captain.API

# Create Class Libraries

dotnet new classlib -n Kashmir.Captain.Application
dotnet new classlib -n Kashmir.Captain.Domain
dotnet new classlib -n Kashmir.Captain.Infrastructure

# Create Test Project

dotnet new xunit -n Kashmir.Captain.Tests

3. Add Projects to Solution
   dotnet sln Kashmir.Captain.sln add ./Kashmir.Captain.API/Kashmir.Captain.API.csproj
   dotnet sln Kashmir.Captain.sln add ./Kashmir.Captain.Application/Kashmir.Captain.Application.csproj
   dotnet sln Kashmir.Captain.sln add ./Kashmir.Captain.Domain/Kashmir.Captain.Domain.csproj
   dotnet sln Kashmir.Captain.sln add ./Kashmir.Captain.Infrastructure/Kashmir.Captain.Infrastructure.csproj
   dotnet sln Kashmir.Captain.sln add ./Kashmir.Captain.Tests/Kashmir.Captain.Tests.csproj

4. Add Project References
   dotnet add Kashmir.Captain.API reference Kashmir.Captain.Application
   dotnet add Kashmir.Captain.API reference Kashmir.Captain.Infrastructure
   dotnet add Kashmir.Captain.Application reference Kashmir.Captain.Domain
   dotnet add Kashmir.Captain.Infrastructure reference Kashmir.Captain.Domain

5. Add NuGet Packages (EF Core Example)
   dotnet add Kashmir.Captain.Infrastructure package Microsoft.EntityFrameworkCore
   dotnet add Kashmir.Captain.Infrastructure package Microsoft.EntityFrameworkCore.SqlServer
   dotnet add Kashmir.Captain.Infrastructure package Microsoft.EntityFrameworkCore.Tools

6. Run the API
   cd Kashmir.Captain.API
   dotnet run
