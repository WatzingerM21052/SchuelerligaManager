using System;
using System.Collections.Generic;

namespace ChessTournamentManager.Shared.Models
{
    public class Tournament
    {
        public int TournamentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        // NEU: Navigation Property für Entity Framework
        // Damit weiß das Turnier jetzt, welche Spielergebnisse zu ihm gehören.
        public List<PlayerTournament> PlayerTournaments { get; set; } = new();
    }
}