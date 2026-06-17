# DiaMate

<p align="center">
  <img src="Logo.png" width="180" />
</p>
Welcome to the **DiaMate** (DiaMate is an AI-powered mobile health assistant for diabetic patients, providing smart guidance using a medical RAG chatbot, food-image calorie estimation, diabetic-foot-ulcer analysis, and automated lab-result processing. The system helps users track glucose, medication, and meals with high accuracy.) — a dedicated backend application built on ASP.NET Core to manage data, authentication, and services.
The system provides a secure, flexible API foundation using Entity Framework Core, SQL Server, and JWT Authentication.

---

## Table of Contents

1. [Project Overview](#project-overview)
2. [Architecture](#architecture)
3. [Technology Stack](#technology-stack)
4. [Project Structure](#project-structure)
5. [Features](#features)
7. [Security Features](#security-features)
8. [Background Services](#background-services)
9. [Email Services](#email-services)
10. [Validation](#validation)
6. [Getting Started](#getting-started)
7. [Configuration](#configuration)
8. [Modules](#modules)
9. [Data Flow](#data-flow)
10. [Database Setup](#database-setup)
11. [Business Rules](#business-rules)
12. [Error Handling](#error-handling)
13. [Testing](#testing)
14. [Support](#support)

---

## Project Overview

The **DiaMate Web API** is an ASP.NET Core application built to serve as a secure backend for a single-page application (SPA) or mobile client.

### Key Capabilities:

- **API Service**: Provides RESTful endpoints via Controllers.
- **Security**: Implements user authentication and authorization using ASP.NET Core Identity and custom JWT (JSON Web Token).
- **Data Management**: Uses Entity Framework Core with SQL Server for robust data storage.
- **CORS**: Configured for local development integration with a frontend running on `http://localhost:5173`.
- **Documentation**: Includes Swagger/OpenAPI for easy endpoint discovery and testing.

---

## Architecture

The project follows the standard **ASP.NET Core Web API** structure, utilizing dependency injection and middleware for configuration.

### Layer Structure:

#### 1. **Presentation Layer (API Endpoints)**

- Handled by **Controllers** which receive HTTP requests.
- The layer where routing, model binding, and authorization take place.

#### 2. **Service/Business Logic Layer** (Implied/Planned)

- _Note: While not explicitly shown in `Program.cs`, this layer will contain the core logic and service classes for data manipulation and business validation._

#### 3. **Data Access Layer (DAL)**

- Implemented using **Entity Framework Core** and the `AppDbContext`.
- Handles communication with the SQL Server database.

---

## Technology Stack

### Core Technologies:

- **ASP.NET Core (Latest)**
- **C#** (Backend Language)
- **SQL Server** (Primary database)
- **Entity Framework Core** (ORM/Data Access)
- **ASP.NET Core Identity** (User/Role Management)
- **JWT (JSON Web Tokens)** (Authentication)

### Additional Tools:

- **Swagger/OpenAPI** (API Documentation)
- **CORS Middleware** (Cross-Origin requests)

---

## Project Structure

The structure is based on a single ASP.NET Core Web API project.

```
DiaMate/
│
├── DiaMate/                     # Main Web API Project
│   ├── Controllers/             # API Endpoints (e.g., AccountController)
│   ├── Data/                    # DbContext and Migrations
│   │   ├── AppDbContext.cs      # Database context
│   ├── Extentions/              # Custom services (e.g., JwtAuth)
│   ├── models/                  # EF Core entities (e.g., AppUser)
│   ├── appsettings.json         # Configuration files
│   └── Program.cs               # Service and Pipeline setup
│
└── DiaMate.sln                  # Visual Studio Solution File
```

---

## Features

Instead of basic tracking options, the backend exposes highly granular endpoints categorized into complex operational subsystems.

### 1. Identity & Access Management (IAM)
* **Robust Core Security**: Integrates secure registration, login, token refresh, and credential updating.
* **Role-Based Authorization (RBAC)**: Maps explicit permission sets to defined application roles (`Admin`, `Patient`, `Doctor`).
* **JWT Access Flow**: Emits encrypted JSON Web Tokens containing claim graphs for identification.

### 2. Profile & Relationship Operations
* **Patient Management**: Configures baseline demographics, physical metrics (height, weight), diabetes categorization profiles, and ties data models to security keys.

### 3. Diabetes Tracking Infrastructure
* **Blood Glucose Module**: Captures absolute glucose quantities, logs timestamps, tracks fasting vs. post-prandial states, and monitors contextual metrics.
* **Medication Registry**: Tracks medication inventories, logs dosages, maintains injection schedules, and records consumer compliance.
* **Meal Analytics Interface**: Records caloric intake, labels meal definitions (breakfast, lunch, dinner, snack), and handles historical structural references.

### 4. Enterprise Subsystems
* **Laboratory Operations**: Allows uploading structural biomedical laboratory data definitions and indexes laboratory logs.
* **File Upload Service**: Sanitizes and processes multi-part file-stream uploads for tracking documentation.
* **Notification System**: Dispatches alert indices based on threshold validations (e.g., missed records, critical glucose levels).

---

## Security Features

The DiaMate API relies on strict industry-standard configurations to maintain security and absolute data privacy:

| Security Vector | Implementation Detail | Target Protection |
| :--- | :--- | :--- |
| **Authentication** | Bearer JWT Cryptography | Prevents Session Hijacking & Anonymity |
| **Password Storage** | PBKDF2 with SHA-256 Hashing | Prevents Rainbow Table & Cleartext Exposure |
| **CORS Enforcer** | Explicit Whitelists (`http://localhost:5173`) | Mitigates Cross-Site Scripting Data Extraction |
| **Authorization Filters**| Declarative Role Policies (`[Authorize]`) | Mitigates Privilege Escalation Vulnerabilities |

---

## Background Services

The API incorporates an independent background processing engine utilizing native `IHostedService` interfaces.

### Automated Account Maintenance
The system registers the `UnverifiedUserCleanupService`. This worker runs inside the application background pipeline on a fixed 24-hour interval. It evaluates the application state database to identify user records that have remained unverified past the valid structural period (e.g., 48 hours post-registration) and removes them.

```mermaid
stateDiagram-v2
    [*] --> EvaluatingDatabase : Interval Triggered (24h)
    EvaluatingDatabase --> CheckingVerification : Query AppUsers
    CheckingVerification --> PurgingUser : Verification Confirmed == False AND Age > 48h
    CheckingVerification --> RetainingUser : Verification Confirmed == True OR Age <= 48h
    PurgingUser --> DatabaseCommit : Execute Cascade Delete
    RetainingUser --> [*]
    DatabaseCommit --> [*]
```
## Email Services

Communication operations utilize a decoupled, interface-driven service layer (`IEmailService`) transmitting over secure SMTP connections:

* **Email Verification Tokens**: On account creation, the system generates an ephemeral token and appends it to a verification email required to unlock authorization pathways.
* **Transactional Dispatching**: Implements asynchronous execution blocks to ensure that outbound network performance constraints do not impact active request threads.

---

## Validation

Input validation blocks data corruption attempts before requests reach the execution layers.

```text
Incoming HTTP Request
      │
      ▼
[DataAnnotation Attributes] ───► Invalid? ───► Auto 400 Bad Request Response
      │
      ▼ Valid
[Custom Validation Logic]   ───► Fails?   ───► Custom Validation Exception
      │
      ▼ Passes
Core Service Execution
```
* **Standard DataAnnotations**: Enforces structural validity via attributes like `[Required]`, `[EmailAddress]`, and `[StringLength]`.
* **Custom Validation Attributes**: Incorporates custom health-domain attributes (e.g., `[FutureDateAttribute]`) to protect the database layer against illogical datetimes.

## Getting Started

### Prerequisites

- **.NET SDK** (matching the project's target framework - typically .NET 8 or newer)
- **SQL Server** instance
- **Visual Studio** (Version 17.14.36327.8 or newer, as per the solution file )

### Steps to Run:

1.  **Clone Repository**
2.  **Restore Dependencies**
    ```bash
    dotnet restore
    ```
3.  **Update Database Connection**
    - Configure the `"MyConnection"` string in `appsettings.json` (see Configuration).
4.  **Apply Migrations** (Assuming migrations have been created)
    ```bash
    dotnet ef database update
    ```
5.  **Run the Application**
    ```bash
    dotnet run
    ```
    The API will start, typically accessible at `https://localhost:7xxx`.

---

## Configuration

### Database Connection

The connection string for SQL Server is referenced in `Program.cs` as `"MyConnection"`.

**Location:** `appsettings.json`

### JSON

```
{
  "JwtSettings": {
    "SecurityKey": "YOUR_VERY_LONG_SECRET_KEY_HERE_MINIMUM_32_BYTES",
    "Issuer": "DiaMateAPI",
    "Audience": "DiaMateClient"
  }
}


```

```
{
  "ConnectionStrings": {
    "MyConnection": "Server=...;Database=DiaMateDb;User Id=...;Password=..."
  }
}
```

## Modules

### 1. Identity Module

- `AppUser` and `IdentityRole` management
- Stores user credentials and profile data

### 2. Data Module

- `AppDbContext` for managing tables
- Handles CRUD (Create, Read, Update, Delete) operations

### 3. Authentication Module

- Handles validation and generation of **JWT tokens**
- Secures endpoints using the `[Authorize]` attribute

---

## Data Flow

```mermaid
graph TD
    A[Frontend/Client: http://localhost:5173] -->|HTTP Request| B(CORS Middleware)
    B --> C{Authentication/Authorization Middleware}
    C --> |Authenticated| D(API Controller)
    D --> E(Service/Business Logic)
    E --> F[AppDbContext/Entity Framework Core]
    F --> |SQL Queries| G(SQL Server Database)
    G --> F
    F --> E
    E --> D
    D --> |JSON Response| C
    C --> A
```

## Database Setup

The project uses **Entity Framework Core Code-First**.

### Required Models

- `AppUser` (inherits from `IdentityUser`)
- `IdentityRole`
- Additional domain models (e.g., `Patient`, `Reading`, etc.)

---

## Setup Steps

### 1. Create initial migration

```bash

dotnet ef database update

```

## Business Rules

- All endpoints requiring user-specific data must be protected by `UseAuthentication()` and `UseAuthorization()`.
- `UseCors("AllowLocalDev")` **must come BEFORE** `UseAuthentication()` and `UseAuthorization()` in the middleware pipeline.
- A user must possess a **valid JWT** to access secured resources.
- Additional domain-specific rules will be implemented in the **Service/Business Layer**.

---

## Error Handling

### HTTP Status Codes

- **200 OK**
- **201 Created**
- **400 Bad Request**
- **401 Unauthorized**
- **403 Forbidden**
- **404 Not Found**

### Validation

- Model validation inside **Controllers**
- Business-level validation inside the **Service Layer**

### Database Exceptions

- EF Core exceptions should be **caught** and mapped to meaningful **HTTP responses**.

---

## Testing

### API Testing (Manual / Postman)

- Test secured endpoints **with and without a valid JWT**
- Verify **CRUD operations** for core models
- Test **successful user registration** and **token generation**

---

## Future Enhancements

- Unit tests for Controller and Service Layer
- Integration tests covering the **API request pipeline**, including database access

---

## Support

If something doesn’t work:

1. Verify the `"MyConnection"` string in `appsettings.json`
2. Ensure latest **EF Core migrations** are applied
3. Confirm `JwtSettings` values (`SecurityKey`, `Issuer`, `Audience`) are correct
