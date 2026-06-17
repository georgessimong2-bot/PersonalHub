## Features

### User Management

- Authentication & Authorization
- User Administration
- Profile Management
- Profile Picture Upload
- Password Management

### Notes

- Personal Knowledge Base
- Markdown Support
- CRUD Operations

### Goals

- Goal Management
- Progress Tracking
- Deadlines and Targets
- AI-Assisted Goal Generation

### Fund Management

- Fund Types
- Funds
- Sub-Funds (In Progress)
- Share Classes (Planned)

---

## Architecture

The solution follows Clean Architecture principles.

```text
PersonalHub
│
├── PersonalHub.Api
├── PersonalHub.Application
├── PersonalHub.Domain
├── PersonalHub.Infrastructure
└── PersonalHub.Web
```

### Technologies

- .NET 10
- ASP.NET Core
- Blazor Server
- MudBlazor
- Entity Framework Core
- SQL Server
- MediatR
- FluentValidation
- JWT Authentication
- Docker
- Nginx
- GitHub Actions
- OpenAI

---

### Architectural Patterns

### Clean Architecture

The solution is organized into distinct layers to enforce separation of concerns and maintain long-term maintainability.

### CQRS (Command Query Responsibility Segregation)

Commands and Queries are separated using MediatR, allowing clear distinction between read and write operations.

### Mediator Pattern

MediatR is used as the central mediator between the API layer and application handlers.

### Dependency Injection

Services and application components are registered through ASP.NET Core's built-in dependency injection container.

### Pipeline Behaviors

Cross-cutting concerns such as validation are handled through MediatR pipeline behaviors, keeping handlers focused on business logic.

## Artificial Intelligence

OpenAI integration is used to enhance the user experience.

Current capabilities include:

- AI-assisted goal generation
- Smart productivity assistance

Future enhancements may include:

- Note summarization
- Semantic search
- Personal AI assistant

---

## Deployment

The application is fully containerized and automatically deployed.

### Infrastructure

- Ubuntu VPS
- Docker Compose
- SQL Server
- Nginx Reverse Proxy
- Let's Encrypt SSL Certificates

### CI/CD

GitHub Actions automatically:

1. Build the solution
2. Build Docker images
3. Deploy to the VPS
4. Restart containers

---

## Roadmap

- Sub-Fund Management
- Share Class Management
- Benchmark Management
- Portfolio Tracking
- Additional AI Features

---

## Author

Georges Simon

Senior .NET Developer specialized in:

- ASP.NET Core
- Blazor
- SQL Server
- Python
- Investment Fund Platforms
- Fund Administration Systems