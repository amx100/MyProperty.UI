# Runbook - MyProperty.UI

Operational runbook for frontend support and incident handling.

## Service Details

- Service: `MyProperty.UI`
- Type: Blazor WebAssembly SPA
- Dependency: `MyProperty.API` backend and auth/token flow

## Build / Start

```bash
 dotnet run --project MyProperty.UI/MyProperty.UI.csproj
```

For production, deploy published static files.

## Health Verification

- App root route loads.
- Static assets and `.wasm` download successfully.
- Login/auth state resolves correctly.
- Core API-dependent pages load data.

## Logs and Diagnostics

Key sources:

- Browser DevTools console/network
- Hosting/CDN logs
- Backend API logs (for correlated failures)

Look for:

- CORS failures
- 401/403 authorization errors
- 404 on static assets or SPA routes
- Network timeouts to API

## Common Incidents

### 1) Blank page after deploy
- Check failed `.wasm` or static asset loads.
- Verify MIME type configuration.
- Confirm base href and static paths.

### 2) API calls failing in production
- Confirm API base URL for environment.
- Check CORS allowlist includes UI origin.
- Verify HTTPS certificates and mixed-content issues.

### 3) Login state issues
- Inspect token presence/expiration in local storage.
- Confirm auth state provider refresh behavior.
- Validate backend token issuance/validation.

## Release Management

Before release:

- Build and smoke test locally.
- Validate API compatibility.

After release:

- Smoke test login and main user flows.
- Monitor console/network errors.
- Track frontend and backend error rates together.

## Security Operations

- Enforce HTTPS.
- Avoid storing sensitive data in local storage beyond required tokens.
- Rotate and invalidate compromised tokens promptly.
