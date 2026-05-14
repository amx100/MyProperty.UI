# Architecture - MyProperty.UI

## High-Level Context

`MyProperty.UI` is the Blazor WebAssembly frontend of the MyProperty platform. It consumes backend APIs and provides interactive user flows for property and reservation features.

## Main Building Blocks

## 1) Application Bootstrap (`Program.cs`)
- Registers root components.
- Configures DI services.
- Configures MudBlazor and local storage.
- Registers auth state provider and API services.

## 2) Routing and App Shell (`App.razor`, `Layout/`)
- Defines route handling and authorization-aware rendering.
- Provides shared layout/navigation patterns.

## 3) Pages (`Pages/`)
- Feature-specific routeable views.
- Coordinate user actions and bind data from services/view models.

## 4) Shared Components (`Components/`)
- Reusable UI building blocks.
- Cross-page controls and patterns.

## 5) Services (`Services/`)
- Typed wrappers around HttpClient.
- Separate concerns by domain capability:
  - Authentication
  - Properties
  - Reservations
  - Account/User features

## 6) Auth State and Token Handling
- `TokenStorage` for persisted token management.
- `TokenAuthenticationStateProvider` to integrate with Blazor auth model.
- Authorization-aware rendering through `AuthenticationStateProvider`.

## Request/Data Flow (Conceptual)

1. User action in page/component triggers service call.
2. Service builds HTTP request using configured HttpClient.
3. API response mapped into UI models/view models.
4. Component state updates and re-renders.

## Architectural Principles

- UI components focus on rendering and interaction.
- Services encapsulate API and integration concerns.
- Authentication is centralized and reusable.
- Dependency injection keeps components testable and decoupled.

## Suggested Future Enhancements

- Add centralized API error handling/interceptors.
- Add loading/error state components for consistency.
- Add UI/component testing (bUnit/Playwright).
- Add caching for frequently requested data.
