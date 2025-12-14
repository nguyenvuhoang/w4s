# O24API (O24OpenAPI)

**O24API** là nền tảng **OpenAPI & Core Banking Integration Platform** được thiết kế để kết nối, mở rộng và điều phối các dịch vụ tài chính – ngân hàng trong hệ sinh thái O24.

Hệ thống hỗ trợ kiến trúc **microservices**, **event-driven**, **real-time**, đáp ứng các nghiệp vụ như Wallet, Payment, Transfer, Loan, Notification, SMS, CDC, Reporting và Digital Channels (Web / Mobile).

---

## 🚀 Key Features

- 🔐 **Secure OpenAPI Gateway**
  - API Key / Signature / Token-based Authentication
  - End-to-end request signing & verification
  - Rate limiting & access control

- 🏦 **Core Banking Integration**
  - Oracle / SQL Server
  - Wallet, Deposit, Loan, Repayment, GL Posting
  - Transaction History & Reconciliation

- 🔄 **Event-Driven Architecture**
  - RabbitMQ / Integration Events
  - Transaction Queue & Fallback mechanism
  - Reliable retry & compensation handling

- 📡 **Real-time Communication**
  - SignalR for logout, notification, transaction status
  - Firebase Push Notification (FCM)
  - Smart OTP / SMS OTP

- 📊 **CDC & Data Processing**
  - SQL Server Change Data Capture (CDC)
  - LSN-based incremental sync
  - Audit & Data Warehouse ready

- 🌐 **Multi-channel Support**
  - Web (Next.js / React)
  - Mobile (React Native)
  - API Consumers (Third-party / Partner)

---

## 🧱 Architecture Overview

### 🏗️ Architecture (Clean Architecture + DDD + Microservices)

O24API được thiết kế theo hướng **Clean Architecture** kết hợp **DDD (Domain-Driven Design)** để đảm bảo:
- Tách bạch rõ trách nhiệm (UI / Application / Domain / Infrastructure)
- Dễ test, dễ mở rộng, giảm coupling
- Phù hợp cho nghiệp vụ phức tạp (transaction-heavy) và tích hợp Core Banking

Đồng thời hệ thống vận hành theo **Microservices** + **Event-driven / Transaction-driven** để tối ưu:
- Scale theo từng domain/service
- Xử lý bất đồng bộ, retry, eventual consistency
- Theo dõi trạng thái giao dịch theo luồng (workflow/queue)

---


### 1) Presentation Layer
- ASP.NET Core Web API, Swagger/OpenAPI
- AuthN/AuthZ, request validation, middleware
- Mapping DTO ↔ Use Case

### 2) Application Layer (Use Cases)
- Orchestrate nghiệp vụ: command/query, workflow steps
- Transaction boundary (khi cần)
- Publish domain events / integration events
- Không phụ thuộc DB framework cụ thể

### 3) Domain Layer (DDD Core)
- Entities / Value Objects / Aggregates
- Domain Services
- Domain Events (vd: `TransactionCreated`, `OtpVerified`, `WalletDeposited`)
- Business rules thuần nghiệp vụ, không phụ thuộc hạ tầng

### 4) Infrastructure Layer
- Repositories (EF Core / LinqToDB)
- Messaging (RabbitMQ)
- Cache (Redis)
- External providers (Core Banking, SMS SOAP, Firebase)
- Observability (Loki/Promtail/Grafana)

---

## 🧩 DDD Bounded Contexts (gợi ý theo O24)

Mỗi domain lớn nên tách thành **Bounded Context** và thường ánh xạ thành service:

- **Identity & Access** (Auth, Role, Permission)
- **Transaction** (Transaction orchestration, history, status)
- **Wallet** (Wallet account, mapping, sync)
- **Payments/Transfer** (Internal/Interbank, fee, validation)
- **Loan** (Repayment schedule, remind, posting)
- **Notification** (SMS/Push/SignalR, template, routing)
- **Configuration** (ConnectConfig, para, code lists)
- **Audit/Logging** (business log, technical log, tracing)

---

