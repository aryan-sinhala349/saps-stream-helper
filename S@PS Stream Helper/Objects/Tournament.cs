using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SAPS
{
    public class Tournament
    {
        public int ID;
        public int NumAttendees;

        public string? Name;
        public string? Slug;
        public string? ShortSlug;

        public int[] Events;

        public Tournament(string slug)
        {
            const string query = @"
                query Tournament($slug: String!)
                {
                    tournament(slug: $slug)
                    {
                        id
                        numAttendees
                        
                        name
                        slug
                        shortSlug

                        events
                        {
                            id
                        }
                    }
                }
            ";

            var variables = new
            {
                slug = slug
            };

            var request = new
            {
                query,
                variables
            };

            string jsonRequest = JsonSerializer.Serialize(request);
            StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            Task<HttpResponseMessage> responseTask = Globals.Client.PostAsync(Globals.Endpoint, content);
            responseTask.Wait();
            HttpResponseMessage response = responseTask.Result;

            if (!response.IsSuccessStatusCode)
                Console.Error.WriteLine(response.StatusCode.ToString());

            Task<string> jsonTask = response.Content.ReadAsStringAsync();
            jsonTask.Wait();
            string jsonResponse = jsonTask.Result;

            JsonNode? jsonQuery = JsonNode.Parse(jsonResponse);
            JsonNode? jsonTournament = jsonQuery?["data"]?["tournament"];

            ID = (int)jsonTournament?["id"];
            NumAttendees = (int)jsonTournament?["numAttendees"];
            Name = (string)jsonTournament?["name"];
            Slug = (string)jsonTournament?["slug"];
            ShortSlug = (string)jsonTournament?["shortSlug"];

            JsonNode? jsonParticipants = jsonTournament?["participants"]?["nodes"];

            int index = 0;

            JsonNode? jsonEvents = jsonTournament?["events"];
            JsonArray? jsonEventsArray = jsonEvents.AsArray();

            index = 0;
            Events = new int[jsonEventsArray.Count];

            foreach (JsonNode node in jsonEventsArray)
            {
                Events[index] = (int)node["id"];
                index++;
            }
        }
    }
}
