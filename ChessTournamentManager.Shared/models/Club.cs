namespace ChessTournamentManager.Shared.Models
{
    public class Club
    {
        public int ClubId { get; set; }
        public string Name { get; set; } = string.Empty;

        // NEU: Die Liste aller Spieler, die zu diesem Verein gehören.
        // Ohne diese Zeile wüsste der Verein nicht, wer seine Mitglieder sind!
        public List<Player> Players { get; set; } = new();
    }
}