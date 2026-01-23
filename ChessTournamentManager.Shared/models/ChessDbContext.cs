using Microsoft.EntityFrameworkCore;

namespace ChessTournamentManager.Shared.Models
{
    public class ChessDbContext : DbContext
    {
        public ChessDbContext(DbContextOptions<ChessDbContext> options) : base(options) { }

        public DbSet<Club> Clubs { get; set; }
        public DbSet<Player> Players { get; set; }

        // NEU:
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<PlayerTournament> PlayerTournaments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Club>().HasKey(e => e.ClubId);
            modelBuilder.Entity<Player>().HasKey(e => e.PlayerId);
            modelBuilder.Entity<Tournament>().HasKey(e => e.TournamentId);
            modelBuilder.Entity<PlayerTournament>().HasKey(e => e.PlayerTournamentId);
        }
    }
}