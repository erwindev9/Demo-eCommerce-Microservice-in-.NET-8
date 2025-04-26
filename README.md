# 🛒 eCommerce Microservices with .NET 8 & Ocelot

A modular eCommerce backend built using **.NET 8**, implementing **Clean Architecture** principles and **Ocelot** as the API Gateway. This solution separates business logic, infrastructure, and presentation layers into clearly defined modules for better scalability and maintainability.

---

## 🧱 Project Architecture

The solution follows a **layered architecture** across all services. Each service is structured with the following key layers:

### 1. 📦 Shared Library

- Contains **reusable components** and **common contracts** across all services (e.g., DTOs, custom exceptions, result wrappers).
- Promotes consistency and reduces code duplication.

### 2. 🧠 Application Layer (`.Application`)

- Contains all **use cases**, **interfaces**, and **business workflows**.
- Acts as a bridge between domain and infrastructure.
- No dependency on external frameworks.

### 3. 🧬 Domain Layer (`.Domain`)

- Defines the **core business logic** and **domain entities**.
- Fully isolated from other layers.
- Contains domain rules, enums, aggregates, and value objects.

### 4. 🛠️ Infrastructure Layer (`.Infrastructure`)

- Implements interfaces defined in the Application layer.
- Handles **database access**, **external API calls**, and other IO operations (e.g., EF Core, HTTP clients, file storage).
- Depends on both Application and Domain layers.

### 5. 🌐 Presentation Layer (`.Presentation`)

- Exposes **RESTful APIs** using ASP.NET Core Web API.
- Handles HTTP requests and returns responses.
- References Application layer to trigger business use cases.

📁 [ServiceName] │ ├── 📁 [ServiceName].Shared ├── 📁 [ServiceName].Application ├── 📁 [ServiceName].Domain ├── 📁 [ServiceName].Infrastructure └── 📁 [ServiceName].Presentation

## 🌐 API Gateway with Ocelot

- Built using **Ocelot** to manage routing between microservices.
- Configuration is stored in `ocelot.json`.
- Supports:
  - Routing
  - Basic rate limiting
  - Centralized entry point for client apps

---

## 🚀 Getting Started