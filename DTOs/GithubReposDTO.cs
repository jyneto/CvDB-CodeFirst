using Azure.Core.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CvCodeFirst.DTOs
{
    public class GithubReposDTO
    {
        [JsonPropertyName("html_url")]
        public string? HtmlUrl { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("language")]
        public string? Language { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
        
    }
}
