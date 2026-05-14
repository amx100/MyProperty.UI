# Contributing - MyProperty.UI

Thanks for contributing to `MyProperty.UI`.

## Development Workflow

1. Create a feature/fix branch.
2. Implement changes with minimal, focused scope.
3. Run build checks locally.
4. Open PR with clear description and UI screenshots/GIFs when applicable.

## Branch Naming

- `feature/<short-description>`
- `fix/<short-description>`
- `chore/<short-description>`

## Commit Guidelines

- Use clear, concise, imperative messages.
- Group related changes logically.

## UI & Code Guidelines

- Keep components readable and small.
- Move API/logic to services.
- Reuse shared components where possible.
- Avoid hardcoded endpoints/secrets.
- Prefer explicit typed models.

## Local Validation

```bash
 dotnet restore
 dotnet build
```

If tests exist:

```bash
 dotnet test
```

## Pull Request Checklist

- [ ] Build passes locally.
- [ ] UI behavior manually tested.
- [ ] No secrets added.
- [ ] Docs updated for user-facing or architectural changes.
- [ ] Screenshots attached for visual changes.
