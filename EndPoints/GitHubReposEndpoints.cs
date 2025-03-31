using CvCodeFirst.Helpers;
using System.Text.Json;

namespace CvCodeFirst.EndPoints
{
    public class GitHubReposEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            //Get repos from github
            app.MapGet("/app.github/{username}", async (string githubUsername, HttpClient httpClient) =>
            {
                //Validate username

                var (isValidUsername, usernameErrors) = InputValidator.ValidateGitHubUsername(githubUsername);
                if (!isValidUsername) return Results.BadRequest(usernameErrors);

                // Create an HTTP client for the request
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("CvApiApp");

                //Fetch repiratories 
                var apiResponse = await httpClient.GetAsync($"https://api.github.com/users/{githubUsername}/repos");
                if (!apiResponse.IsSuccessStatusCode)
                    return Results.NotFound("GitHub user not found");

                //Deserialize the respone
                var jsonResponse = await apiResponse.Content.ReadAsStringAsync();
                var deserializedRepos = JsonSerializer.Deserialize<List<JsonElement>>(jsonResponse);

                //Validate repo using InputValidator
                var (validateRepos, validationErrors) = InputValidator.ValidateGitHubRepositories(deserializedRepos!);

                //
                return Results.Ok(new { Repositories = validateRepos, Errors = validationErrors });

            });
        }   
    }
}
