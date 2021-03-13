using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Movies.Client.Models;
using Newtonsoft.Json;

namespace Movies.Client.ApiServices
{
  public class MovieApiService : IMovieApiService
  {
    public async Task<IEnumerable<Movie>> GetMovies()
    {
      // 1. Get token from Identity Server, of course we should provide the IS configuration like address, clientId and clientSecret.
      // 2. Send request to Protected API
      // 3. Deserialize object to MovieList

      // 1-1. 'retrieve' our api credentials. This must be registered on Identity Server.
      var apiClientCredentials = new ClientCredentialsTokenRequest
      {
        Address = "https://localhost:5005/connect/token",

        ClientId = "movieClient",
        ClientSecret = "secret",

        // This is the scope our Protected API requires.
        Scope = "movieAPI"
      };

      // creates a new HttpClient to talk to our IdentityServer (localhost:5005)
      var client = new HttpClient();

      // check if we can reach the Discovery document. Not 100% needed but ...
      var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5005");
      if (disco.IsError)
      {
        return null; // throw 500 error
      }

      // 1-2. Authentication and get an access token from Identity Server
      var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiClientCredentials);
      if (tokenResponse.IsError)
      {
        return null;
      }

      // 2-1. Another HttpClient for talking now with our Protected API
      var apiClient = new HttpClient();

      // 2-2. Set the access_token in the request Authorization: Bearer <token>
      apiClient.SetBearerToken(tokenResponse.AccessToken);

      // 2-3. Send a request to our Protected API
      var response = await apiClient.GetAsync("https://localhost:5001/api/movies");
      response.EnsureSuccessStatusCode();

      var content = await response.Content.ReadAsStringAsync();

      List<Movie> movieList = JsonConvert.DeserializeObject<List<Movie>>(content);

      return movieList;

      //var movieList = new List<Movie>();
      //movieList.Add(
      //  new Movie
      //  {
      //    Id = 1,
      //    Genre = "Comics",
      //    Title = "asd",
      //    Rating = "9.2",
      //    ImageUrl = "images/src",
      //    ReleaseDate = DateTime.Now,
      //    Owner = "shockz"
      //  });

      //return await Task.FromResult(movieList);
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
  }
}
