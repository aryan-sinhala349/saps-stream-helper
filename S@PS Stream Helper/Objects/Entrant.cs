using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text;

namespace SAPS
{
    public class Entrant
    {
        public int ID;

        public string? Name;

        public int[] Participants;

        public Entrant(int id)
        {
            const string query = @"
                query Entrant($id: ID!)
                {
                    entrant(id: $id)
                    {
                        id
                    
                        name

                        participants
                        {
                            id
                        }
                    }
                }
            ";

            var variables = new
            {
                id = id
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
            JsonNode? jsonEntrant = jsonQuery?["data"]?["entrant"];

            ID = (int)jsonEntrant["id"];
            Name = (string)jsonEntrant["name"];

            JsonArray? jsonParticipantsArray = jsonEntrant?["participants"].AsArray();

            Participants = new int[jsonParticipantsArray.Count];
            for (int i = 0; i < jsonParticipantsArray.Count; i++)
                Participants[i] = (int)jsonParticipantsArray[i]["id"];
        }
    }
}