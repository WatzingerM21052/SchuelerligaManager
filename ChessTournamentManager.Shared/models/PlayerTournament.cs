namespace ChessTournamentManager.Shared.Models
{
    public class PlayerTournament
    {
        public int PlayerTournamentId { get; set; }

        public int TournamentId { get; set; }
        public Tournament? Tournament { get; set; }

        public int PlayerId { get; set; }
        public Player? Player { get; set; }

        public double Points { get; set; }

        // NEU: Hier speichern wir jetzt auch den exakten Platz, den der Spieler im Turnier erreicht hat!
        public int Rank { get; set; }
    }
}