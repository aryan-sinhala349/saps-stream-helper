using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SAPS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.Write("Please input your auth token: ");
            Globals.AuthToken = Console.ReadLine();

            Console.Write("Please input your desired slug: ");
            Globals.TournamentSlug = Console.ReadLine();
            Globals.Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Globals.AuthToken}");

            Tournament? currentTournament = new Tournament(Globals.TournamentSlug);

            for (int i = 0; i < currentTournament.Events.Length; i++)
            {
                Event currentEvent = new Event(currentTournament.Events[i]);

                Console.WriteLine($"{currentEvent.Name} has {currentEvent.NumEntrants} entrants");

                for (int j = 0; j < currentEvent.Entrants.Length; j++)
                    Console.WriteLine($" - {currentEvent.Entrants[j]}");

                Console.WriteLine();
            }
        }
    }
}
