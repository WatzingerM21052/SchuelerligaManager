using System.ComponentModel.DataAnnotations.Schema;

namespace ChessTournamentManager.Shared.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public int? Elo { get; set; }
        public string Country { get; set; } = string.Empty;

        public int? BirthYear { get; set; }
        public string AgeGroup { get; set; } = string.Empty;

        [NotMapped]
        public int TournamentRank { get; set; }

        public int? ClubId { get; set; }
        public Club? Club { get; set; }

        // NEU: Die Liste der Ergebnisse (Turniere), an denen dieser Spieler teilgenommen hat.
        // Ohne diese Zeile weiß der Spieler nichts von seiner "Karriere".
        public List<PlayerTournament> PlayerTournaments { get; set; } = new();
    }
}