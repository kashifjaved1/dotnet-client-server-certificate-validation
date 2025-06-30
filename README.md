# .NET Client-Server Certificate Validation

[![License](https://img.shields.io/badge/license-GPL-blue)](LICENSE)

A robust .NET implementation for secure client-server communication using certificate-based authentication.

## 🚀 Features

- 🔒 Secure certificate-based authentication
- 🔄 Automatic certificate validation
- 🔐 TLS/SSL encryption support

## 📋 Prerequisites

- .NET 8.0 or higher
- Visual Studio 2022 or .NET CLI
- Valid SSL/TLS certificates

## 🛠️ Installation

1. Clone the repository:
```bash
git clone https://github.com/kashifjaved1/dotnet-client-server-certificate-validation.git
cd dotnet-client-server-certificate-validation
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the solution:
```bash
dotnet build
```

## 🔧 Configuration

The project requires the following configuration:

1. Certificate paths:
   - Client certificate
   - Server certificate
   - CA certificate (optional)

2. Connection settings:
   - Server URL
   - Port number
   - Timeout settings

## 🏃 Running the Application

To run the application:

```bash
dotnet run
```

## 🛠️ Development

The solution contains two projects:

- `Client`: Handles client-side certificate validation and secure communication
- `Server`: Manages server-side as well as client-side certificate validation and secure endpoints