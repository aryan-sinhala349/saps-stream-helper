using System.Text.Json;
using System.Text;
using System.Text.Json.Nodes;

namespace SAPS
{
    public class Participant
    {
        public int ID;

        public string? Prefix;
        public string? Tag;
        public string? Pronouns;
        
        public Participant(int id)
        {
            const string query = @"
                query Participant($id: ID!)
                {
                    participant(id: $id)
                    {
                        id
                        
                        prefix
                        gamerTag

                        player
                        {
                            id 

                            prefix
                            gamerTag    
                        }  

                        user
                        {
                            id

                            genderPronoun
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
            JsonNode? participant = jsonQuery?["data"]?["participant"];

            ID = (int)participant?["id"];
            Tag = (string)participant?["gamerTag"];
            Pronouns = (string)participant?["user"]?["genderPronoun"];

            Prefix = (string)participant?["prefix"];

            if (Prefix == null || Prefix.Length == 0)
                Prefix = (string)participant?["player"]?["prefix"];
        }
    }
}