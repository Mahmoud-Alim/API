# Global Authorization Implementation — TODO

## Objective
Implement global authorization (fallback policy) so every endpoint requires an authenticated user by default, keeping explicit `[AllowAnonymous]` for public endpoints and policy/role-based `[Authorize]` for restricted ones.

## Steps
- [x] 0. Analyze existing authorization setup (Program.cs, controllers, DependencyInjection files)
- [x] 1. Confirm plan with user (approved: Option B — fallback policy in API/DependencyInjection.cs)
- [x] 2. Add global fallback authorization policy (RequireAuthenticatedUser) in `API/DependencyInjection.cs`
- [x] 3. Add `.AllowAnonymous()` to `MapOpenApi()` in Development in `API/DependencyInjection.cs`
- [x] 4. Remove redundant `[Authorize]` from `AuthController.RevokeToken` (Logout)
- [x] 5. Remove redundant `[Authorize]` from `RateLimitDemoController.Get` + remove unused `using Microsoft.AspNetCore.Authorization;`
- [x] 6. Verify RoleManagementController / UsersController need no changes
- [x] 7. Run `dotnet build` to confirm compilation (Build succeeded in 19.2s)

