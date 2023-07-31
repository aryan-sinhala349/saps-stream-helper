using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text;

namespace SAPS
{
    public class Event
    {
        public int ID;
        public int NumEntrants;

        public string? Name;

        public int[] Entrants;

        public Event(int id)
        {
            {
                const string query = @"
                    query Event($id: ID!)
                    {
                        event(id: $id)
                        {
                            id
                            numEntrants
                        
                            name
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
                JsonNode? jsonEvent = jsonQuery?["data"]?["event"];

                ID = (int)jsonEvent["id"];
                NumEntrants = (int)jsonEvent["numEntrants"];
                Name = (string)jsonEvent["name"];
            }

            {
                Entrants = new int[NumEntrants];

                const string query = @"
                    query EventEntrants($id: ID!, $perPage: Int!)
                    {
                        event(id: $id)
                        {
                            entrants(query:
                            {
                                page: 1
                                perPage: $perPage
                            })
                            {
                                nodes
                                {
                                    id
                                }
                            }
                        }
                    }
                ";

                var variables = new
                {
                    id = id,
                    perPage = NumEntrants
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
                JsonNode? jsonEntrants = jsonQuery?["data"]?["event"]?["entrants"]?["nodes"];
                JsonArray? jsonEntrantsArray = jsonEntrants.AsArray();

                for (int i = 0; i < NumEntrants; i++)
                    Entrants[i] = (int)jsonEntrantsArray[i]["id"];
            }
        }
    }
}