# CRUD Test Application

## Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Setup and Installation](#setup-and-installation)
- [Running the Application](#running-the-application)
- [Running Tests](#running-tests)
- [Project Structure](#project-structure)
- [Contributing](#contributing)
- [License](#license)

## Overview
The CRUD Test Application is a full-stack web application designed to manage customer data. It allows users to perform Create, Read, Update, and Delete (CRUD) operations on customer records. The application adheres to best practices in software development, ensuring consistency, scalability, and maintainability.

## Features
- **Add Customer**: Create new customer records with validated input.
- **View Customers**: Display a list of all customers with search and filter capabilities.
- **Edit Customer**: Update existing customer information.
- **Delete Customer**: Remove customer records securely.
- **Responsive UI**: Built with Blazor WebAssembly for a seamless user experience across devices.
- **API Documentation**: Integrated Swagger UI for easy API exploration and testing.
- **Automated Testing**: Comprehensive acceptance tests to ensure application reliability.


## Technologies Used
### Frontend:
- **Blazor WebAssembly** - For building interactive web UIs using C#.

### Backend:
- **ASP.NET Core** - For building robust and scalable APIs.
- **Entity Framework Core** - For ORM and database interactions.
- **MediatR** - For implementing the mediator pattern.
- **FluentValidation** - For model validation.

### Testing:
- **SpecFlow** - For Behavior-Driven Development (BDD) style acceptance tests.
- **xUnit** - For unit and integration testing.



### Other Tools:
- **AutoMapper** - For object-object mapping.
- **Swagger** - For API documentation and testing.

## Setup and Installation
### Prerequisites
- **.NET 7 SDK**

- **Visual Studio 2022** or **Visual Studio Code** with necessary extensions.

### Steps
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/your-repo/crud-test-dotnet.git
   cd crud-test-dotnet
   ```

2. **Navigate to the Server Project and Restore Dependencies**:
   ```bash
   cd src/Presentation.Api
   dotnet restore
   ```

3. **Apply Database Migrations**:
   ```bash
   dotnet ef database update
   ```

4. **Navigate to the Client Project and Restore Dependencies**:
   ```bash
   cd ../Presentation.Client
   dotnet restore
   ```

5. **Navigate to the Shared Project and Restore Dependencies**:
   ```bash
   cd ../Presentation.Shared
   dotnet restore
   ```

6. **Navigate to the Acceptance Tests Project and Restore Dependencies**:
   ```bash
   cd ../../tests/AcceptanceTests
   dotnet restore
   ```

## Running the Application
### Running Locally
1. **Start the Server**:
   ```bash
   cd src/Presentation.Api
   dotnet run
   ```
   The API will be available at [https://localhost:5091/](https://localhost:5091/). Swagger UI can be accessed at [https://localhost:5091/swagger](https://localhost:5091/swagger).

2. **Start the Client**:
   ```bash
   cd ../Presentation.Client
   dotnet run
   ```
   The Blazor WebAssembly client will be available at [https://localhost:7046/](https://localhost:7046/).

3. **Access the Application**:
   Open your browser and navigate to [https://localhost:7046/](https://localhost:7046/) to use the application.




## Running Tests
### Acceptance Tests
1. **Navigate to the Acceptance Tests Project**:
   ```bash
   cd tests/AcceptanceTests
   ```

2. **Run Tests**:
   ```bash
   dotnet test
   ```

### Unit and Integration Tests
- Similar steps apply for running unit and integration tests within their respective test projects.



2. **Navigate to the Project Root**:
   ```bash
   cd crud-test-dotnet
   ```

3. **Build and Run Containers**:
   ```bash
   docker-compose up --build
   ```

4. **Access the Application**:
   - The client is available at [http://localhost:7045/](http://localhost:7045/).
   - The API is available at [http://localhost:5090/](http://localhost:5090/swagger/index.html).

## Project Structure
```
crud-test-dotnet/
├── src/
│   ├── Core/
│   │   ├── Application/            # Application layer - CQRS, Business Logic, MediatR Commands
│   │   └── Domain/                 # Domain entities and business rules
│   ├── Infrastructure/
│   │   └── Persistence/            # Data access, database configurations, repositories
│   ├── Presentation/
│   │   ├── Api/                    # ASP.NET Core API Project
│   │   ├── Client/                 # Blazor WebAssembly Project
│   │   └── Shared/                 # Shared DTOs and contracts between client and API
├── tests/
│   ├── AcceptanceTests/            # SpecFlow-based acceptance testing project
│   └── UnitTests/                  # xUnit-based unit testing
├
└── README.md
```
- **Core.Application**: Business logic, application services, commands, and queries.
- **Core.Domain**: Domain models, including entities and value objects.
- **Infrastructure.Persistence**: Persistence layer for data access, including database context and repository implementations.
- **Presentation.Api**: ASP.NET Core Web API for backend services.
- **Presentation.Client**: Blazor WebAssembly for the frontend UI.
- **Presentation.Shared**: Shared DTOs and validation logic for consistency.
- **AcceptanceTests**: BDD acceptance tests using SpecFlow.
- **UnitTests**: Unit tests for core components using xUnit.



