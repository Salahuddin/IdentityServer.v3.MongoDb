using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Thinktecture.IdentityServer.Core.Models;

namespace Core.MongoDb.Tests
{
    public static class TestData
    {
        public static Client ClientAllProperties()
        {
            return new Client
            {
                AbsoluteRefreshTokenLifetime = 10,
                AccessTokenLifetime = 20,
                AccessTokenType = AccessTokenType.Reference,
                AllowLocalLogin = false,
                AllowRememberConsent = true,
                AuthorizationCodeLifetime = 30,
                ClientId = "123",
                ClientName = "TEST",
                ClientSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes("secret")),
                ClientUri = "clientUri",
                Enabled = true,
                Flow = Flows.AuthorizationCode,
                IdentityProviderRestrictions = new[] { "idpr" }.ToList(),
                IdentityTokenLifetime = 40,
                IdentityTokenSigningKeyType = SigningKeyTypes.ClientSecret,
                LogoUri = "uri:logo",
                PostLogoutRedirectUris = { "uri:logout" },
                RedirectUris = { "uri:redirect" },
                RefreshTokenExpiration = TokenExpiration.Sliding,
                RefreshTokenUsage = TokenUsage.ReUse,
                RequireConsent = true,
                ScopeRestrictions = { "restriction1", "restriction2", "restriction3" },
                SlidingRefreshTokenLifetime = 50
            };
        }

        public static Scope ScopeAllProperties()
        {
            return new Scope
            {
                Name = "all",
                DisplayName = "displayName",
                Claims = new List<ScopeClaim>
                {
                    new ScopeClaim
                    {
                        Name = "claim1",
                        AlwaysIncludeInIdToken = false,
                        Description = "claim1 description"
                    },
                    new ScopeClaim
                    {
                        Name = "claim2",
                        AlwaysIncludeInIdToken = true,
                        Description = "claim2 description"
                    },
                },
                ClaimsRule = "claimsRule",
                Description = "Description",
                Emphasize = true,
                Enabled = false,
                IncludeAllClaimsForUser = true,
                Required = true,
                ShowInDiscoveryDocument = false,
                Type = ScopeType.Identity
            };
        }

        public static Scope ScopeMandatoryProperties()
        {
            return new Scope
            {
                Name = "mandatory",
                DisplayName = "displayName"
            };
        }

        public static AuthorizationCode AuthorizationCode(string subjectId = null)
        {
            return new AuthorizationCode
            {
                IsOpenId = true,
                CreationTime = new DateTimeOffset(2000, 1, 1, 1, 1, 1, 0, TimeSpan.Zero),
                Client = Client(),
                RedirectUri = "uri:redirect",
                RequestedScopes = Scopes(),
                Subject = Subject(subjectId),
                WasConsentShown = true,
                Nonce = "test"
            };
        }

        private static Client Client()
        {
            return ClientAllProperties();
        }

        private static IEnumerable<Scope> Scopes()
        {
            yield return ScopeAllProperties();
            yield return ScopeMandatoryProperties();
        }

        private static ClaimsPrincipal Subject(string subjectId)
        {
            return new ClaimsPrincipal(
                new ClaimsIdentity(
                    Claims(subjectId), "authtype"
                    ));
        }

        private static List<Claim> Claims(string subjectId)
        {
            return new List<Claim>
            {
                new Claim("sub", subjectId ?? "foo"),
                new Claim("name", "bar"),
                new Claim("email", "baz@qux.com"),
                new Claim("scope", "scope1"),
                new Claim("scope", "scope2"),
            };
        }

        public static RefreshToken RefreshToken(string subject = null)
        {
            return new RefreshToken
            {
                AccessToken = Token(subject),
                CreationTime = new DateTimeOffset(2000, 1, 1, 1, 1, 1, 0, TimeSpan.Zero),
                LifeTime = 100,
            };
        }

        public static Token Token(string subject = null)
        {
            return new Token
            {
                Audience = "audience",
                Claims = Claims(subject),
                Client = ClientAllProperties(),
                CreationTime = new DateTimeOffset(2000, 1, 1, 1, 1, 1, 0, TimeSpan.Zero),
                Issuer = "issuer",
                Lifetime = 200,
                Type = "tokenType"
            };
        }

        private static readonly JsonSerializer Serializer = new JsonSerializer { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
        
        public static string ToTestableString<T>(T subject)
        {
            return JObject.FromObject(subject, Serializer).ToString();
        }
    }
}