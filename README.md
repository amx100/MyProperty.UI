# MyProperty.UI

Frontend client for the MyProperty platform built with Blazor WebAssembly.

## Overview

`MyProperty.UI` provides the user-facing web interface for property browsing, authentication, account interactions, reservations, and related workflows.

The project uses Blazor WebAssembly with service-based API integration and modern UI components.

## Solution Structure

- `MyProperty.UI/` – Main Blazor WebAssembly app project.
  - `Pages/` – Routeable UI pages.
  - `Layout/` – App layouts and shared page structure.
  - `wwwroot/` – Static assets and host page.
  - `App.razor` – Root routing component.
  - `Program.cs` – DI/service setup and app bootstrap.
- `Services/` – API client services and feature service abstractions/implementations.
- `Components/` – Shared reusable UI components.
- `Program.cs` (root) – additional host/service wiring for repository-level components.

## UI Architecture

- **Presentation layer:** Razor components/pages.
- **State/Auth layer:** custom authentication state provider + token storage.
- **Service layer:** typed services (`IApiService`, `IPropertyService`, `IReservationService`, etc.) for backend communication.

## Technology Stack

- Blazor WebAssembly
- MudBlazor UI library
- Blazored.LocalStorage
- ASP.NET Core authentication abstractions (`AuthenticationStateProvider`)
- HttpClient for API communication

## Getting Started

### Prerequisites

- .NET SDK (version required by `MyProperty.UI/MyProperty.UI.csproj`)

### Run locally

```bash
# from repository root
 dotnet restore
 dotnet build
 dotnet run --project MyProperty.UI/MyProperty.UI.csproj
```

Then open the local URL shown in terminal output.

## Configuration

Typical frontend configuration includes:

- API base URL
- auth/token behaviors
- environment-specific settings

Review `Program.cs` and service implementations for base URL and client setup.

## Authentication Flow (High Level)

1. User signs in via authentication service.
2. Token is stored through `TokenStorage` (local storage wrapper).
3. `TokenAuthenticationStateProvider` exposes user auth state.
4. Authenticated API calls include required token headers.

## Development Notes

- Keep UI logic in components/pages minimal.
- Move API communication and business workflows into services.
- Keep reusable UI in `Components`.
- Use strongly typed models/contracts for API communication.

## Build & Quality

```bash
 dotnet format
 dotnet build
 dotnet test
```

(If test projects are added/available.)

## Related Repository

Backend API lives in: `amx100/MyProperty.API`
