using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace AuthProviders
{
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly TokenStorage _tokenStorage;

        public TokenAuthenticationStateProvider(TokenStorage tokenStorage)
        {
            _tokenStorage = tokenStorage;
        }

        public void StateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private async Task<IEnumerable<Claim>> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            Console.WriteLine("JWT Payload contents:");
            foreach (var kvp in keyValuePairs)
            {
                Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
            }

            // Dodaj AccountId kao NameIdentifier claim
            if (keyValuePairs.TryGetValue("accountId", out object accountId))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, accountId.ToString()));
                Console.WriteLine($"Added NameIdentifier claim with value: {accountId}");
            }

            // Dodaj email/name claim
            if (keyValuePairs.TryGetValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", out object email))
            {
                claims.Add(new Claim(ClaimTypes.Name, email.ToString()));
            }

            // Dodaj role claim
            if (keyValuePairs.TryGetValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out object role))
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            // Dodaj custom claim za AccountId ako nije već dodan
            if (!claims.Any(c => c.Type == ClaimTypes.NameIdentifier))
            {
                var storedAccountId = await _tokenStorage.GetAccountId();
                if (!string.IsNullOrEmpty(storedAccountId))
                {
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, storedAccountId));
                    Console.WriteLine($"Added NameIdentifier claim from storage: {storedAccountId}");
                }
            }

            return claims;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _tokenStorage.GetAccessToken();
                
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("No access token found");
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                Console.WriteLine($"Token found: {token}");
                var claims = await ParseClaimsFromJwt(token);
                
                // Debug ispis
                foreach (var claim in claims)
                {
                    Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
                }
                
                var identity = new ClaimsIdentity(claims, "jwt");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAuthenticationStateAsync: {ex.Message}");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}