using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer
{
  public class Config
  {
    public static IEnumerable<Client> Clients => new Client[]
    {
      new Client
      {
        ClientId = "movieClient",
        AllowedGrantTypes = GrantTypes.ClientCredentials,
        ClientSecrets =
        {
          new Secret("secret".Sha256())
        },
        AllowedScopes = {"movieAPI"}
      },
      new Client
      {
        ClientId = "movies_mvc_client",
        ClientName = "Movies MVC Web App",
        AllowedGrantTypes = GrantTypes.Code,
        AllowRememberConsent = false,
        RedirectUris = new List<string>()
        {
          "https://localhost:5002/signin-oidc" // this is client app port
        },
        PostLogoutRedirectUris = new List<string>()
        {
          "https://localhost:5002/signout-callback-oidc"
        },
        ClientSecrets = new List<Secret>
        {
          new Secret("secret".Sha256())
        },
        AllowedScopes = new List<string>
        {
          IdentityServerConstants.StandardScopes.OpenId,
          IdentityServerConstants.StandardScopes.Profile
        }
      }
    };

    public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
    {
    };

    public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
    {
      new ApiScope("movieAPI", "Movie API")
    };

    public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
    {
      new IdentityResources.OpenId(), // client scope 추가에 따른 identity resource 추가
      new IdentityResources.Profile() // client scope 추가에 따른 identity resource 추가
    };

    public static List<TestUser> TestUsers => new List<TestUser>
    {
      new TestUser
      {
        SubjectId = "46A73B00-6F08-4CCE-BBFA-CDC80767E33B",
        Username = "shockz",
        Password = "shockz",
        Claims = new List<Claim>
        {
          new Claim(JwtClaimTypes.GivenName, "Jun"),
          new Claim(JwtClaimTypes.FamilyName, "Yu")
        }
      }
    };
  }
}
