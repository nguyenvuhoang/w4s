# O24API (O24OpenAPI)

**O24API** là nền tảng **OpenAPI & Core Banking Integration Platform** được thiết kế để **kết nối – mở rộng – điều phối** các dịch vụ tài chính/ngân hàng trong hệ sinh thái **O24**.

Hệ thống hỗ trợ **Microservices**, **Event-Driven**, **Transaction-Driven**, **Real-time**, phù hợp cho các nghiệp vụ ngân hàng cốt lõi như **Wallet, Payment, Transfer, Loan, Notification, SMS, CDC, Reporting** và các kênh số **Web / Mobile / Partner API**.

---

## 🚀 Key Features

### 🔐 Secure OpenAPI Gateway
- API Key / Signature / Token-based Authentication
- End-to-end request signing & verification
- Rate limiting, IP whitelist, access control
- Audit & request tracing

### 🏦 Core Banking Integration
- Oracle / SQL Server
- Wallet, Deposit, Loan, Repayment, GL Posting
- Transaction History & Reconciliation
- Branch / Product / Currency aware

### 🔄 Event-Driven Architecture
- RabbitMQ / Integration Events
- Transaction Queue & Fallback mechanism
- Reliable retry, idempotency, compensation
- Eventual consistency cho nghiệp vụ phân tán

### 📡 Real-time Communication
- SignalR (logout, notification, transaction status)
- Firebase Push Notification (FCM)
- Smart OTP / SMS OTP
- Multi-channel delivery

### 📊 CDC & Data Processing
- SQL Server Change Data Capture (CDC)
- LSN-based incremental synchronization
- Audit trail & Data Warehouse ready
- Near-real-time reporting

### 🌐 Multi-Channel Support
- Web: **Next.js / React**
- Mobile: **React Native**
- API Consumers: **Third-party / Partner / Internal services**

---

## 🧱 Architecture Overview

### 🏗️ Architecture Style

O24API được thiết kế theo mô hình:

- **Clean Architecture**
- **DDD (Domain-Driven Design)**
- **Microservices**
- **Event-Driven / Transaction-Driven**

Mục tiêu:
- Tách bạch rõ ràng responsibility
- Dễ test, dễ mở rộng, giảm coupling
- Phù hợp cho transaction-heavy systems
- Đảm bảo khả năng scale theo domain

---

## 🏛️ Layered Architecture

### 1️⃣ Presentation Layer
- ASP.NET Core Web API
- Swagger / OpenAPI
- Authentication & Authorization
- Middleware (logging, exception, versioning)
- Mapping DTO ↔ Application Use Case

### 2️⃣ Application Layer (Use Cases)
- CQRS: Command / Query
- Orchestrate nghiệp vụ & workflow
- Transaction boundary (khi cần)
- Publish domain / integration events
- Không phụ thuộc DB hay framework hạ tầng

### 3️⃣ Domain Layer (DDD Core)
- Aggregates / Entities / Value Objects
- Domain Services
- Domain Events  
  (vd: `TransactionCreated`, `OtpVerified`, `WalletDeposited`)
- Business rules thuần nghiệp vụ
- Không phụ thuộc Infrastructure

### 4️⃣ Infrastructure Layer
- Persistence: EF Core / LinqToDB
- Messaging: RabbitMQ
- Cache: Redis
- External Providers:
  - Core Banking
  - SMS (SOAP)
  - Firebase
- Observability: Loki / Promtail / Grafana

---

## 🧩 DDD Bounded Contexts

- **Identity & Access**
  - Authentication, Authorization
  - Role, Permission

- **Transaction**
  - Transaction orchestration
  - Status tracking
  - History & reconciliation

- **Wallet**
  - Wallet account
  - Mapping & synchronization
  - Balance management

- **Payment / Transfer**
  - Internal / Interbank
  - Fee calculation
  - Validation & limit

- **Loan**
  - Repayment schedule
  - Auto posting
  - Reminder & notification

- **Notification**
  - SMS / Push / SignalR
  - Template & routing
  - Multi-provider fallback

- **Configuration**
  - ConnectConfig
  - Parameters, Code lists
  - Dynamic form & rule config

- **Audit / Logging**
  - Business log
  - Technical log
  - Tracing & monitoring

---

## 📁 Project Structure

```text
O24OpenAPI.TenService/
├── O24OpenAPI.TenService.API/
│   ├── Application/
│   │   ├── BackgroundJobs/
│   │   │   └── HealthCheckJob.cs
│   │   ├── Features/
│   │   │   └── CreateTenService.cs
│   │   ├── Migrations/
│   │   │   └── DataTenServiceMigration.cs
│   │   └── ApplicationExtensions.cs
│   ├── Controllers/
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── StaticConfig/
│   │   └── BackgroundJobs.json
│   ├── Program.cs
│   ├── appsettings.json
│   └── appsettings.Development.json
├── O24OpenAPI.TenService.Domain/
│   └── AggregatesModel/
│       └── TenServiceAggregate/
│           ├── TenServiceDomain.cs
│           └── ITenServiceRepository.cs
└── O24OpenAPI.TenService.Infrastructure/
    ├── Configurations/
    │   └── TenServiceConfig.cs
    ├── EntityConfigurations/
    │   └── TenServiceConfiguration.cs
    ├── Migrations/
    │   └── EntityMigration.cs
    ├── Persistances/
    ├── Repositories/
    │   └── TenServiceRepository.cs
    └── InfrastructureExtensions.cs
