using BackendTask.Database;
using BackendTask.Database.Repository;
using BackendTask.Database.Repository.Implementation;
using BackendTask.Extensions;
using BackendTask.Middleware;
using BackendTask.Models;
using BackendTask.Services;
using BackendTask.Services.Implementation;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace BackendTask.Configuration
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {

            serviceCollection.AddSingleton(configuration.GetSection("AuthSettings").Get<AuthSettings>());
            var authSettings = configuration.GetSection("AuthSettings");
            var authConf = authSettings.Get<AuthSettings>();

            serviceCollection.AddDbContext<AppDbContext>();
            serviceCollection.AddScoped<ICategoryRepository, CategoryRepository>();
            serviceCollection.AddScoped<ICategoryService, CategoryService>();
            
            serviceCollection.AddEndpointsApiExplorer();            
            serviceCollection.AddSwaggerGen(options =>
            {
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                options.SwaggerDoc("v1", new() { Title = "BackendTask", Version = "v1" });
                options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.OAuth2,
                    Flows = new Microsoft.OpenApi.Models.OpenApiOAuthFlows
                    {
                        ClientCredentials = new Microsoft.OpenApi.Models.OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("https://localhost:7220/connect/token"),
                        }
                    }
                });
                options.OperationFilter<AuthorizeOperationFilter>();
            });

            List<Client> apiClients = authConf.Clients.Select(c => new Client
            {
                ClientId = c.ClientId,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                ClientSecrets =
            {
                new Secret(c.ClientSecret.Sha256())
            },
                AllowedScopes = c.AllowedScopes,
                AccessTokenLifetime = 18000, // 3 hour in seconds
                IdentityTokenLifetime = 18000, // 3 hours in seconds
                RefreshTokenExpiration = TokenExpiration.Absolute,
            }).ToList();

            List<ApiScope> apiScopes = authConf.Scopes.Select(s => new ApiScope(s.Name, s.DisplayName)).ToList();

            serviceCollection.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
            })
            .AddInMemoryApiScopes(apiScopes)
            .AddInMemoryClients(apiClients)
            .AddInMemoryIdentityResources(new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            })
            .AddProfileService<CustomProfileService>().AddDeveloperSigningCredential();


            serviceCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(options =>
                    {
                        options.ClaimsIssuer = authConf.Authority; // URL of your IdentityServer instance
                        options.RequireHttpsMetadata = false; // Only set to false in development environment

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true, // Validate the issuer of the JWT token
                            ValidIssuer = authConf.Authority, // The expected issuer of the JWT token
                            ValidateAudience = false, // Do not validate the audience of the JWT token
                            ValidateIssuerSigningKey = false, // Disable signature validation
                            SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                            {
                                var jwt = new JwtSecurityToken(token);

                                return jwt;
                            },
                            RequireSignedTokens = false
                        };
                        options.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                                return Task.CompletedTask;
                            }
                        };
                    });


            foreach (var p in authConf.Policies)
            {
                serviceCollection.AddAuthorization(options =>
                {
                    options.AddPolicy(p.Name, policy =>
                    {
                        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                        policy.RequireClaim(p.RequiredClaim, p.AllowedScopes);
                    });
                });
            }

            serviceCollection.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            serviceCollection.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;

            });

            return serviceCollection;

        }
    }
}
