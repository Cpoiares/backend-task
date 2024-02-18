using BackendTask.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BackendTask.Extensions
{
    public class CustomProfileService : IProfileService
    {
        private AuthSettings _settings;
        public CustomProfileService(IOptions<AuthSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // Add custom claims to the profile
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Audience, "sample-backend-task"));
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Issuer, _settings.Authority));
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            // The IsActiveAsync method is not used in this example, so we return a completed task
            return Task.CompletedTask;
        }
    }
}
