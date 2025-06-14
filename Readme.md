# Kashmir.Captain

A **Blazor Server** application built using **Clean Architecture** and the **CQRS pattern** with MediatR, FluentValidation, and scalable project structure.

---

## 📁 Folder Structure

```
Kashmir.Captain/
├── src/
│   ├── Kashmir.Captain.Web/            # Blazor Server UI (Presentation Layer)
│   ├── Kashmir.Captain.Application/    # CQRS: Commands, Queries, Handlers
│   ├── Kashmir.Captain.Domain/         # Domain Models, Entities, Enums
│   ├── Kashmir.Captain.Infrastructure/ # EF Core, Services, DB Context
│   └── Kashmir.Captain.Shared/         # DTOs, Value Objects, Extensions
│
├── tests/
│   └── Kashmir.Captain.Tests/          # Unit Test Project (xUnit)
│
├── Kashmir.Captain.sln                 # Visual Studio Solution File
└── .gitignore                          # Git Ignore Settings
```

---

## 🚀 Prerequisites

- [.NET SDK 7.0 or later](https://dotnet.microsoft.com/download)
- Visual Studio 2022+ or Visual Studio Code
- Git (optional)
- Node.js (optional for future frontend packages)

---

## 🛠️ Project Setup Commands

### ✅ Create Solution & Projects

```bash
mkdir Kashmir.Captain
cd Kashmir.Captain

dotnet new sln -n Kashmir.Captain

# Create Projects
dotnet new blazorserver -n Kashmir.Captain.Web -o src/Kashmir.Captain.Web
dotnet new classlib -n Kashmir.Captain.Application -o src/Kashmir.Captain.Application
dotnet new classlib -n Kashmir.Captain.Domain -o src/Kashmir.Captain.Domain
dotnet new classlib -n Kashmir.Captain.Infrastructure -o src/Kashmir.Captain.Infrastructure
dotnet new classlib -n Kashmir.Captain.Shared -o src/Kashmir.Captain.Shared

# Create Test Project
dotnet new xunit -n Kashmir.Captain.Tests -o tests/Kashmir.Captain.Tests
```

### ✅ Add Projects to Solution

```bash
dotnet sln Kashmir.Captain.sln add src/Kashmir.Captain.Web/Kashmir.Captain.Web.csproj
dotnet sln Kashmir.Captain.sln add src/Kashmir.Captain.Application/Kashmir.Captain.Application.csproj
dotnet sln Kashmir.Captain.sln add src/Kashmir.Captain.Domain/Kashmir.Captain.Domain.csproj
dotnet sln Kashmir.Captain.sln add src/Kashmir.Captain.Infrastructure/Kashmir.Captain.Infrastructure.csproj
dotnet sln Kashmir.Captain.sln add src/Kashmir.Captain.Shared/Kashmir.Captain.Shared.csproj
dotnet sln Kashmir.Captain.sln add tests/Kashmir.Captain.Tests/Kashmir.Captain.Tests.csproj
```

### 🔗 Add References Between Projects

```bash
# Web ➜ Application + Shared
dotnet add src/Kashmir.Captain.Web reference src/Kashmir.Captain.Application
dotnet add src/Kashmir.Captain.Web reference src/Kashmir.Captain.Shared

# Application ➜ Domain + Shared
dotnet add src/Kashmir.Captain.Application reference src/Kashmir.Captain.Domain
dotnet add src/Kashmir.Captain.Application reference src/Kashmir.Captain.Shared

# Infrastructure ➜ Application + Domain + Shared
dotnet add src/Kashmir.Captain.Infrastructure reference src/Kashmir.Captain.Application
dotnet add src/Kashmir.Captain.Infrastructure reference src/Kashmir.Captain.Domain
dotnet add src/Kashmir.Captain.Infrastructure reference src/Kashmir.Captain.Shared

# Tests ➜ Application + Domain + Shared
dotnet add tests/Kashmir.Captain.Tests reference src/Kashmir.Captain.Application
dotnet add tests/Kashmir.Captain.Tests reference src/Kashmir.Captain.Domain
dotnet add tests/Kashmir.Captain.Tests reference src/Kashmir.Captain.Shared
```

---

## 🧪 Run Unit Tests

```bash
dotnet test
```

You can use xUnit, Moq, FluentAssertions, and TestServer for integration testing.

---

## 📄 .gitignore Initialization

To create `.gitignore` file for .NET:

```bash
curl https://raw.githubusercontent.com/github/gitignore/main/VisualStudio.gitignore -o .gitignore
```

Or manually create it and paste standard .NET ignore rules.

---

## 🚀 Run the Blazor App

```bash
cd src/Kashmir.Captain.Web
dotnet run
```

Visit [https://localhost:5001](https://localhost:5001)

---

## 📚 Tools & Libraries

- ✅ Blazor Server
- ✅ MediatR for CQRS
- ✅ FluentValidation
- ✅ Entity Framework Core
- ✅ xUnit for Testing
- ✅ Clean Architecture Principles

---

## 📌 License

This project is licensed under the MIT License.

---

> Created with ❤️ by Bilal Ahmad Sofi
