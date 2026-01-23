namespace ChessTournamentManager.Shared.Models
{
    // Dieses Hilfsmodell nutzen wir nur für die Anzeige der Endtabelle
    public class LeaderboardRow
    {
        public int Rank { get; set; }
        public int PlayerId { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string ClubName { get; set; } = string.Empty;
        public string AgeGroup { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int TournamentsCount { get; set; }
        public double TotalPoints { get; set; }
    }
}