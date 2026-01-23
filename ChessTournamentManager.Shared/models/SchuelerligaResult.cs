namespace ChessTournamentManager.Shared.Models
{
    public class SchuelerligaResult
    {
        public int Rank { get; set; }
        public Player Player { get; set; } = new();

        // Die Gesamtsumme ALLER erzielten Punkte
        public double TotalPoints { get; set; }

        // Zweitwertung: Anzahl der gespielten Turniere
        public int TournamentsPlayed { get; set; }

        // Liste aller Einzelpunkte (für die Ansicht)
        public List<double> AllScores { get; set; } = new();
    }
}