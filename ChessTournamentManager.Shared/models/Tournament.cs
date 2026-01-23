namespace ChessTournamentManager.Shared.Models
{
    public class Tournament
    {
        public int TournamentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}