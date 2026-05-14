# Deployment Guide - MyProperty.UI

This guide covers generic deployment options for Blazor WebAssembly output.

## Build for Production

```bash
 dotnet publish MyProperty.UI/MyProperty.UI.csproj -c Release -o ./publish/ui
```

Deploy static output from the publish directory to any static hosting platform/CDN.

## Hosting Requirements

Because this is Blazor WebAssembly, hosting must support:

- Static file serving
- Correct MIME types for `.wasm`
- SPA fallback routing (serve `index.html` for client-side routes)
- HTTPS

## Environment / API Endpoint Strategy

Ensure UI points to correct API base URL per environment:

- Local development
- Staging
- Production

Use one of:

- Environment-specific config files
- Build-time variable replacement
- Runtime config endpoint pattern

## CORS and API Integration

Backend API must allow frontend origin(s):

- Configure allowed origins explicitly.
- Allow required headers (Authorization, Content-Type).
- Allow required methods (GET/POST/PUT/DELETE...).

## Post-Deployment Checklist

- App loads successfully.
- Routes work with hard refresh (SPA fallback confirmed).
- Authentication/login flow works.
- API calls succeed without CORS errors.
- Static assets and WASM files are cached appropriately.

## Rollback Strategy

- Keep previous static artifact versions.
- Repoint hosting to previous version on regressions.
- Validate API contract compatibility before rollback.
