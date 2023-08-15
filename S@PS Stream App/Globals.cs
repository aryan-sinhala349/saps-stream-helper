namespace SAPS
{
    public static class Globals
    {
        internal static HttpClient Client = new HttpClient();

        public static string AuthToken = string.Empty;
        public static string TournamentSlug = string.Empty;
        public const string Endpoint = "https://api.start.gg/gql/alpha";
    }
}