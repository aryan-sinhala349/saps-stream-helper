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
                {
                    Entrant currentEntrant = new Entrant(currentEvent.Entrants[j]);

                    Console.WriteLine($" - {currentEntrant.Name} - {currentEntrant.Participants.Length} player(s)");

                    if (currentEntrant.Participants.Length < 2)
                        continue;

                    for (int k = 0; k < currentEntrant.Participants.Length; k++)
                    {
                        Participant currentParticipant = new Participant(currentEntrant.Participants[k]);

                        string prefix = (currentParticipant.Prefix != null && currentParticipant.Prefix.Length != 0) ? $"{currentParticipant.Prefix} | " : "";
                        string suffix = (currentParticipant.Pronouns != null && currentParticipant.Pronouns.Length != 0) ? $" ({currentParticipant.Pronouns})" : "";

                        Console.WriteLine($"   - {prefix}{currentParticipant.Tag}{suffix}");
                    }
                }

                Console.WriteLine();
            }
        }
    }
}
