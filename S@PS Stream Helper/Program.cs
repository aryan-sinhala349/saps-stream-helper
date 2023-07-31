using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

public class Program
{
    private static HttpClient s_Client = new HttpClient();

    private struct Tournament
    {
        public int id;
        public string? name;
    }

    public static async Task Main(string[] args)
    {
        const string endpoint = "https://api.start.gg/gql/alpha";
        Console.Write("Please input your auth token: ");
        string? authToken = Console.ReadLine();

        Console.Write("Please input your desired slug: ");
        string? slug = Console.ReadLine();

        const string query = @"
            query TournamentName($slug: String!)
            {
                tournament(slug: $slug)
                {
                    id
                    name
                }
            }
        ";

        var variables = new
        {
            slug = slug
        };

        s_Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

        var request = new
        {
            query,
            variables
        };

        string jsonRequest = JsonSerializer.Serialize(request);
        StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await s_Client.PostAsync(endpoint, content);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();

            JsonNode jsonQuery = JsonNode.Parse(jsonResponse);
            JsonNode data = jsonQuery["data"];
            Console.WriteLine(data);
        }
    }
}