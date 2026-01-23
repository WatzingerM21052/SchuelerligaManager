using ChessTournamentManager.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ChessTournamentManager.Client.Services
{
    public class SchuelerligaService
    {
        private readonly IDbContextFactory<ChessDbContext> _dbFactory;

        public SchuelerligaService(IDbContextFactory<ChessDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<SchuelerligaResult>> CalculateLeaderboardAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();

            // 1. Alle Ergebnisse aus der DB laden (inklusive Spieler-Daten)
            var allResults = await db.PlayerTournaments
                .Include(pt => pt.Player)
                .ThenInclude(p => p.Club)
                .ToListAsync();

            var leaderboard = new List<SchuelerligaResult>();

            // 2. Gruppieren nach Spieler
            var resultsByPlayer = allResults.GroupBy(r => r.PlayerId);

            foreach (var group in resultsByPlayer)
            {
                var player = group.First().Player;

                // Alle Punkte dieses Spielers sammeln
                var scores = group.Select(r => r.Points).ToList();

                // HIER IST DIE KORREKTUR: Wir summieren ALLE Punkte auf (kein Take(4) mehr)
                double totalPoints = scores.Sum();

                leaderboard.Add(new SchuelerligaResult
                {
                    Player = player!,
                    TotalPoints = totalPoints,
                    TournamentsPlayed = scores.Count,
                    AllScores = scores
                });
            }

            // 3. SORTIERUNG DER RANGLISTE (Wie in Excel)
            // 1. Nach Punkten absteigend
            // 2. Bei Gleichstand: Nach Anzahl Turniere absteigend
            leaderboard = leaderboard
                .OrderByDescending(x => x.TotalPoints)
                .ThenByDescending(x => x.TournamentsPlayed)
                .ToList();

            // 4. RÄNGE VERGEBEN (1, 2, 3...)
            for (int i = 0; i < leaderboard.Count; i++)
            {
                leaderboard[i].Rank = i + 1;
            }

            return leaderboard;
        }
    }
}