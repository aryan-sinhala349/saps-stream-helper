using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SAPS
{
	public partial class Homepage : ContentPage
	{
		private string m_ImageURL;
		private string m_GamerTag;

		private static Homepage s_Instance;
		public static Homepage Instance
		{
			get
			{
				if (s_Instance == null)
					s_Instance = new Homepage();

				return s_Instance;
			}
		}

		public Homepage()
		{
			InitializeComponent();

			{
				const string query = @"
					query CurrentUser($imgType: String)
					{
						currentUser
						{
							images(type: $imgType)
							{
								url
							}
							player
							{
								gamerTag
							}
						}
					}
				";

				var variables = new 
				{ 
					imgType = "profile"
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
				{
					m_WelcomeText.Text = response.StatusCode.ToString();
					return;
				}

                Task<string> jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                string jsonResponse = jsonTask.Result;

                JsonNode? jsonQuery = JsonNode.Parse(jsonResponse);

				JsonNode? jsonUser = jsonQuery?["data"]?["currentUser"];
				JsonNode? jsonImage = jsonUser?["images"]?[0];
				JsonNode? jsonPlayer = jsonUser?["player"];

				m_ImageURL = (string)jsonImage?["url"];

				m_GamerTag = (string)jsonPlayer?["gamerTag"];

				m_ProfilePicture.Source = ImageSource.FromUri(new Uri(m_ImageURL));
				m_ProfilePicture.WidthRequest = 64;
				m_ProfilePicture.HeightRequest = 64;

				m_WelcomeText.Text = $"Welcome {m_GamerTag}.";
            }
		}

        private void OnCompleteSlugLink(object sender, EventArgs e)
        {
			{
				const string query = @"
					query TournamentEvents($slug: String)
					{
						tournament(slug: $slug)
						{
							events
							{
								name
							}
						}
					}
				";
			}
        }
    }
}
