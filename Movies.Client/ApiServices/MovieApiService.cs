using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.Models;
using Newtonsoft.Json;

namespace Movies.Client.ApiServices
{
  public class MovieApiService : IMovieApiService
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MovieApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
      _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
      _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public async Task<IEnumerable<Movie>> GetMovies()
    {
      // WAY 1
      var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

      var request = new HttpRequestMessage(HttpMethod.Get, "/api/movies/");

      var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

      response.EnsureSuccessStatusCode();

      var content = await response.Content.ReadAsStringAsync();
      var movieList = JsonConvert.DeserializeObject<List<Movie>>(content);

      return movieList;

      //// WAY 2
      //// 1. Get token from Identity Server, of course we should provide the IS configuration like address, clientId and clientSecret.
      //// 2. Send request to Protected API
      //// 3. Deserialize object to MovieList

      //// 1-1. 'retrieve' our api credentials. This must be registered on Identity Server.
      //var apiClientCredentials = new ClientCredentialsTokenRequest
      //{
      //  Address = "https://localhost:5005/connect/token",

      //  ClientId = "movieClient",
      //  ClientSecret = "secret",

      //  // This is the scope our Protected API requires.
      //  Scope = "movieAPI"
      //};

      //// creates a new HttpClient to talk to our IdentityServer (localhost:5005)
      //var client = new HttpClient();

      //// check if we can reach the Discovery document. Not 100% needed but ...
      //var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5005");
      //if (disco.IsError)
      //{
      //  return null; // throw 500 error
      //}

      //// 1-2. Authentication and get an access token from Identity Server
      //var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiClientCredentials);
      //if (tokenResponse.IsError)
      //{
      //  return null;
      //}

      //// 2-1. Another HttpClient for talking now with our Protected API
      //var apiClient = new HttpClient();

      //// 2-2. Set the access_token in the request Authorization: Bearer <token>
      //apiClient.SetBearerToken(tokenResponse.AccessToken);

      //// 2-3. Send a request to our Protected API
      //var response = await apiClient.GetAsync("https://localhost:5001/api/movies");
      //response.EnsureSuccessStatusCode();

      //var content = await response.Content.ReadAsStringAsync();

      //List<Movie> movieList = JsonConvert.DeserializeObject<List<Movie>>(content);

      //return movieList;
    }

    public Task<Movie> GetMovie(string id)
    {
      throw new NotImplementedException();
    }

    public Task<Movie> CreateMovie(Movie movie)
    {
      throw new NotImplementedException();
    }

    public Task<Movie> UpdateMovie(Movie movie)
    {
      throw new NotImplementedException();
    }

    public Task DeleteMovie(int id)
    {
      throw new NotImplementedException();
    }

    public async Task<UserInfoViewModel> GetUserInfo()
    {
      var idpClient = _httpClientFactory.CreateClient("IDPClient");

      var metaDataResponse = await idpClient.GetDiscoveryDocumentAsync();
      if (metaDataResponse.IsError)
      {
        throw new HttpRequestException("Something went wrong while requesting the access token");
      }

      var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

      var userInfoResponse = await idpClient.GetUserInfoAsync(
        new UserInfoRequest
        {
          Address = metaDataResponse.UserInfoEndpoint,
          Token = accessToken
        });

      if (userInfoResponse.IsError)
      {
        throw new HttpRequestException("Something went wrong while getting user info");
      }

      var userInfoDictionary = new Dictionary<string, string>();
      foreach (var claim in userInfoResponse.Claims)
      {
        userInfoDictionary.Add(claim.Type, claim.Value);
      }

      return new UserInfoViewModel(userInfoDictionary);
    }
  }
}
