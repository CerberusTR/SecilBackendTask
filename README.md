 Configuration and Data Fetching System

This project is a system that collects data from different services at regular intervals and stores this data in a database. The project consists of three main components:
1. **Configuration API**: Manages the configurations of other services and performs background data fetching operations.
2. **Service A**: A service that provides data.
3. **Service B**: Another service that provides data.

## Technologies Used

- .NET 8 SDK
- ASP.NET Core
- Entity Framework Core
- SQL Server
- Swagger

## Design Patterns

- **Repository Pattern**: Used to abstract and manage data access logic.
- **Unit of Work Pattern**: Used to manage working with multiple repositories in a single operation.
- **Dependency Injection**: Used to manage service dependencies and improve testability.
- **Background Service**: Used to manage tasks running in the background.

## Requirements

- .NET 8 SDK
- Docker
- Visual Studio or Visual Studio Code
