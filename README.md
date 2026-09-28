# TeslaRent

A .NET-based car rental application originally developed in 2022 as a commercial project for a client.

The source code is published with the client's explicit permission after the original system was taken out of active use.

## Overview

TeslaRent is a multi-project .NET 6 solution with separate customer-facing, administrative, API, business logic and data access layers.

The solution demonstrates a full-stack architecture built around ASP.NET Core, Blazor and Entity Framework Core.

## Architecture

The solution is divided into several projects:

- **TeslaRent_Client** — customer-facing application built with Blazor WebAssembly
- **TeslaRent_Server** — administrative/server-side application
- **TeslaCar_API** — ASP.NET Core REST API
- **Business** — business logic, repositories and object mapping
- **DataAccess** — Entity Framework Core data access and database migrations
- **Models** — DTOs and view models
- **Common** — shared application components and models

## Technology Stack

- C#
- .NET 6
- ASP.NET Core
- Blazor WebAssembly
- Blazor Server
- REST API
- Entity Framework Core 6
- Microsoft SQL Server
- ASP.NET Core Identity
- JWT authentication
- AutoMapper
- Stripe payment integration
- Swagger / OpenAPI
- Mailjet
- Radzen Blazor

## Key Features

- Separate customer-facing and administrative applications
- REST API used by the client application
- User authentication and authorization
- Database persistence with Entity Framework Core
- EF Core migrations
- Layered application structure
- DTO and ViewModel separation
- Repository-based data access
- AutoMapper-based object mapping
- Online payment integration
- API documentation through Swagger

## Project Structure

```text
TeslaRent
│
├── TeslaRent_Client
│   └── Blazor WebAssembly client
│
├── TeslaRent_Server
│   └── Administrative/server-side application
│
├── TeslaCar_API
│   └── ASP.NET Core REST API
│
├── Business
│   ├── Repository
│   └── Mapper
│
├── DataAccess
│   ├── Data
│   └── Migrations
│
├── Models
│   ├── DTO
│   └── ViewModels
│
└── Common
    └── Shared application components
