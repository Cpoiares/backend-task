using IdentityServer4.Models;

namespace BackendTask.Models
{
    public class AuthSettings
    {
        public List<AuthClient> Clients { get; set; }
        public List<ApiResources> Resources { get; set; }
        public List<ApiScope> Scopes { get; set; }
        public List<ApiPolicies> Policies { get; set; }
        public List<ApiTestUsers> TestUsers { get; set; }
        public string Authority { get; set; }
        public string Audience { get; set; }
    }

    public class ApiTestUsers
    {
        public string SubjectId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ApiResources
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public ICollection<string> Scopes { get; set; }
    }

    public class AuthClient
    {
        public string ClientId { get; set; }
        public List<string> Claims { get; set; }
        public string ClientSecret { get; set; }

        public List<string> AllowedScopes { get; set; }
    }

    public class ApiPolicies
    {
        public string Name { get; set; }
        public string RequiredClaim { get; set; }
        public List<string> AllowedScopes { get; set; }

    }
}
