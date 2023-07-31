namespace SAPS
{
    public static class Globals
    {
        internal static HttpClient Client = new HttpClient();

        public static string AuthToken = "";
        public static string TournamentSlug = "";
        public const string Endpoint = "https://api.start.gg/gql/alpha";
    }
}