# Unife API - University Management System

[![.NET](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)](https://github.com/yourusername/unife-api)

## ?? Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Running the Application](#running-the-application)
- [API Documentation](#api-documentation)
- [Database](#database)
- [Authentication & Authorization](#authentication--authorization)
- [Logging](#logging)
- [Caching](#caching)
- [Testing](#testing)
- [Deployment](#deployment)
- [Contributing](#contributing)
- [License](#license)

## ?? Overview

Unife API is a comprehensive university management system built with .NET 8, implementing Clean Architecture principles, CQRS pattern, and Domain-Driven Design (DDD). The system provides robust APIs for managing university operations including student enrollment, staff administration, academic departments, and comprehensive authentication systems.

## ??? Architecture

The project follows **Clean Architecture** principles with clear separation of concerns:

```
???????????????????????????????????????????????????????????????
?                        WebAPI Layer                         ?
?              (Controllers, Middleware, etc.)               ?
???????????????????????????????????????????????????????????????
?                      Business Layer                        ?
?          (CQRS, Services, DTOs, Validators, etc.)         ?
???????????????????????????????????????????????????????????????
?                    Data Access Layer                       ?
?           (Repositories, DbContext, etc.)                 ?
???????????????????????????????????????????????????????????????
?                      Domain Layer                          ?
?              (Entities, Enums, etc.)                      ?
???????????????????????????????????????????????????????????????
?                       Core Layer                           ?
?        (Utilities, Base Classes, Interfaces)              ?
???????????????????????????????????????????????????????????????
```

### Key Architectural Patterns

- **Clean Architecture**: Dependency inversion and separation of concerns
- **CQRS (Command Query Responsibility Segregation)**: Using MediatR for command and query handling
- **Domain-Driven Design (DDD)**: Rich domain models and clear business logic encapsulation
- **Repository Pattern**: Data access abstraction
- **Unit of Work Pattern**: Transaction management
- **Specification Pattern**: Query specification and reusability

## ? Features

### Core Features
- ?? **JWT Authentication & Authorization** with refresh tokens
- ?? **Multi-role User Management** (Admin, Staff, Student)
- ?? **University Management** (Universities, Faculties, Departments)
- ?? **Email Verification System** with OTP
- ?? **Phone Verification System** with OTP
- ?? **Password Management** (Change, Reset, Forgot Password)
- ?? **User Profile Management**
- ?? **Session Management** with Redis
- ?? **Comprehensive Logging** with Serilog
- ?? **Redis Caching** for performance optimization
- ??? **Response Compression** (Zstd, Brotli, Gzip, Deflate)

### Technical Features
- ? **FluentValidation** for request validation
- ??? **AutoMapper** for object mapping
- ?? **Swagger/OpenAPI** documentation
- ?? **Health Checks** for system monitoring
- ?? **High Performance** with optimized queries and caching
- ?? **Configurable Settings** via appsettings.json
- ?? **CORS** support for cross-origin requests
- ?? **Docker** support for containerization

## ??? Technology Stack

### Backend
- **.NET 8.0** - Latest LTS version
- **ASP.NET Core** - Web API framework
- **Entity Framework Core 9.0.6** - ORM for database operations
- **PostgreSQL** - Primary database with Npgsql provider
- **Redis** - Caching and session storage
- **MediatR 12.5.0** - CQRS implementation
- **AutoMapper 13.0.1** - Object-to-object mapping
- **FluentValidation 12.0.0** - Model validation

### Security & Authentication
- **JWT Bearer Authentication** - Token-based authentication
- **System.IdentityModel.Tokens.Jwt 8.12.1** - JWT handling
- **Microsoft.AspNetCore.Authentication.JwtBearer 8.0.10** - JWT middleware

### Logging & Monitoring
- **Serilog 4.3.0** - Structured logging
- **Serilog.AspNetCore 9.0.0** - ASP.NET Core integration
- **Serilog.Sinks.Console 6.0.0** - Console logging
- **Serilog.Sinks.File 7.0.0** - File logging

### Performance & Compression
- **ZstdNet 1.4.5** - Zstandard compression
- **Built-in Brotli/Gzip** - Additional compression options
- **StackExchange.Redis 2.8.41** - Redis client

### Documentation & Testing
- **Swashbuckle.AspNetCore 6.6.2** - Swagger/OpenAPI
- **Microsoft.VisualStudio.Azure.Containers.Tools.Targets** - Docker support

## ?? Project Structure

```
Solution1/
??? Core/                           # Core utilities and base classes
?   ??? Entities/                   # Base entity classes
?   ??? Security/JWT/               # JWT utilities and configurations
?   ??? Utilities/                  # Common utilities (Password, OTP, etc.)
?   ??? ObjectStorage/              # Object storage abstractions
?
??? Domain/                         # Domain entities and business rules
?   ??? Entities/                   # Domain entities
?   ?   ??? AuthorizationModuleEntities/  # User management entities
?   ?   ??? UniversityModul/       # University-related entities
?   ?   ??? AcademicModulEntities/  # Academic entities
?   ??? Enums/                      # Domain enumerations
?   ??? Repositories/               # Repository interfaces
?
??? DataAccess/                     # Data access layer
?   ??? Concrete/EntityFramework/   # EF Core implementations
?   ??? Database/Context/           # Database context
?   ??? ObjectStorage/Redis/        # Redis implementations
?
??? Business/                       # Business logic layer
?   ??? Features/CQRS/             # CQRS commands and queries
?   ?   ??? Auth/                  # Authentication features
?   ?   ??? User/                  # User management features
?   ?   ??? Universities/          # University management features
?   ??? DTOs/                      # Data Transfer Objects
?   ??? Services/                  # Business services
?   ??? Validators/                # FluentValidation validators
?   ??? Mappings/                  # AutoMapper profiles
?   ??? Extensions/                # Service collection extensions
?
??? WebAPI/                        # Presentation layer
    ??? Controllers/               # API controllers
    ??? Middlewares/               # Custom middlewares
    ??? Compression/               # Compression providers
    ??? HealthChecks/              # Health check implementations
```

## ?? Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 12+](https://www.postgresql.org/download/)
- [Redis 6+](https://redis.io/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Docker](https://www.docker.com/) (optional, for containerization)

## ?? Installation

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/unife-api.git
cd unife-api/Solution1
```

### 2. Restore NuGet Packages
```bash
dotnet restore
```

### 3. Database Setup

#### PostgreSQL Setup
1. Install PostgreSQL
2. Create a new database named `unife_db`
3. Update connection string in `appsettings.json`

#### Redis Setup
1. Install Redis
2. Start Redis server
3. Update Redis connection string in `appsettings.json`

### 4. Apply Database Migrations
```bash
dotnet ef database update --project DataAccess --startup-project WebAPI
```

## ?? Configuration

### 1. Database Configuration
Update `appsettings.json` in the WebAPI project:

```json
{
  "ConnectionStrings": {
    "UnifeDatabase": "Server=localhost;Database=unife_db;Port=5432;User Id=postgres;Password=yourpassword;",
    "UnifeObjectStorageConnectionString": "localhost:6379,password=yourredispassword,ssl=false"
  }
}
```

### 2. JWT Configuration
```json
{
  "Jwt": {
    "SecretKey": "your-super-secret-key-that-must-be-at-least-32-characters-long",
    "Issuer": "UnifeAPI",
    "Audience": "UnifeAPI",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  }
}
```

### 3. Email Configuration
```json
{
  "EmailSettings": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587",
    "Username": "your-email@gmail.com",
    "Password": "your-app-password",
    "EnableSsl": "true",
    "FromAddress": "your-email@gmail.com"
  }
}
```

## ????? Running the Application

### Development Mode
```bash
cd WebAPI
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5085`
- Swagger UI: `http://localhost:5085/swagger`

### Production Mode
```bash
dotnet run --environment Production
```

### Using Docker
```bash
docker build -t unife-api .
docker run -p 5085:80 unife-api
```

## ?? API Documentation

### Swagger/OpenAPI
Access the interactive API documentation at `http://localhost:5085/swagger`

### Authentication
Most endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

### Key Endpoints

#### Authentication
- `POST /api/auth/signin` - User sign in
- `POST /api/auth/signup` - User registration
- `POST /api/auth/refresh-token` - Refresh access token
- `POST /api/auth/logout` - User logout
- `POST /api/auth/verify-email` - Email verification
- `POST /api/auth/verify-phone` - Phone verification

#### User Management
- `GET /api/user/profile` - Get user profile
- `PUT /api/user/profile` - Update user profile
- `POST /api/user/disable-account` - Disable user account
- `POST /api/auth/change-password` - Change password
- `POST /api/auth/forgot-password` - Forgot password

#### University Management
- `GET /api/universities` - Get universities (paginated)
- `POST /api/universities` - Create university
- `PUT /api/universities/{id}` - Update university
- `DELETE /api/universities/{id}` - Delete university

#### Health & Monitoring
- `GET /health` - Application health check

## ??? Database

### Entity Relationship Overview

#### Authorization Module
- **Admin**: System administrators
- **Staff**: University staff members
- **Student**: University students
- **Role/Permission**: Role-based access control
- **SecurityEvent**: Security audit trail

#### University Module
- **University**: University information
- **Faculty**: University faculties
- **Department**: Academic departments
- **Rector**: University leadership

#### Academic Module
- **Academician**: Academic staff
- **AcademicDepartment**: Academic department details

### Database Features
- **Soft Delete**: Entities support soft deletion
- **Audit Trail**: Comprehensive logging for all entities
- **UUID Primary Keys**: Using GUIDs for better security
- **Optimistic Concurrency**: Preventing data conflicts
- **Indexing**: Optimized for performance

## ?? Authentication & Authorization

### JWT Implementation
- **Access Tokens**: Short-lived (15 minutes default)
- **Refresh Tokens**: Long-lived (7 days default)
- **Token Rotation**: Automatic token refresh
- **Session Management**: Redis-based session tracking

### User Types
1. **Admin** (UserTypeId: 1)
   - Full system access
   - User management
   - University management

2. **Staff** (UserTypeId: 2)
   - Limited administrative access
   - Faculty/department management

3. **Student** (UserTypeId: 3)
   - Personal profile management
   - Academic information access

### Security Features
- **OTP Verification**: Email and phone verification
- **Password Security**: Bcrypt hashing with salt
- **Rate Limiting**: Protection against brute force attacks
- **Session Security**: Secure session management
- **CORS Configuration**: Controlled cross-origin access

## ?? Logging

### Serilog Configuration
- **Structured Logging**: JSON-formatted logs
- **Multiple Sinks**: Console and file outputs
- **Log Levels**: Configurable per namespace
- **Request Logging**: HTTP request/response logging
- **Performance Tracking**: Response time monitoring

### Log Locations
- **Console**: Real-time development logging
- **Files**: `logs/unife-{date}.log` (7-day retention)
- **Structured Data**: Machine-readable format

## ?? Caching

### Redis Implementation
- **Session Storage**: User session management
- **Cache Storage**: Application data caching
- **Verification Storage**: OTP and verification codes

### Cache Strategy
- **Multi-level Caching**: Memory + Redis
- **Cache Invalidation**: Automatic and manual
- **Performance Optimization**: Reduced database load
- **Distributed Caching**: Scalable across instances

## ?? Testing

### Running Tests
```bash
# Run all tests
dotnet test

# Run specific project tests
dotnet test Tests/Unit.Tests/
dotnet test Tests/Integration.Tests/
```

### Test Coverage
- **Unit Tests**: Business logic and utilities
- **Integration Tests**: API endpoints and database
- **Validation Tests**: FluentValidation rules
- **Security Tests**: Authentication and authorization

## ?? Deployment

### Environment Configuration
Create environment-specific appsettings:
- `appsettings.Development.json`
- `appsettings.Staging.json`
- `appsettings.Production.json`

### Docker Deployment
```bash
# Build image
docker build -t unife-api:latest .

# Run container
docker run -d \
  --name unife-api \
  -p 80:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  unife-api:latest
```

### Production Checklist
- [ ] Update JWT secret keys
- [ ] Configure production database
- [ ] Set up Redis cluster
- [ ] Configure HTTPS certificates
- [ ] Set up monitoring and alerting
- [ ] Configure log aggregation
- [ ] Set up backup strategies

## ?? Contributing

### Development Guidelines
1. Follow Clean Architecture principles
2. Write comprehensive unit tests
3. Use meaningful commit messages
4. Update documentation for new features
5. Follow C# coding conventions

### Pull Request Process
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add/update tests
5. Update documentation
6. Submit a pull request

### Code Style
- Use C# 12.0 features appropriately
- Follow Microsoft C# coding conventions
- Use meaningful variable and method names
- Add XML documentation for public APIs

## ?? License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## ?? Support

For support and questions:
- **Email**: support@unife.com
- **Issues**: [GitHub Issues](https://github.com/yourusername/unife-api/issues)
- **Documentation**: [Wiki](https://github.com/yourusername/unife-api/wiki)

## ?? Acknowledgments

- Microsoft for .NET 8 and ASP.NET Core
- Entity Framework Core team
- MediatR community
- AutoMapper contributors
- FluentValidation team
- Serilog community
- Redis team
- All open-source contributors

---

**Built with ?? using .NET 8 and Clean Architecture principles**