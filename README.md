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

