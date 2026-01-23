using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using ChessTournamentManager.Shared.Models;

namespace ChessTournamentManager.Client.Services
{
    public class DatabaseManager
    {
        private readonly IDbContextFactory<ChessDbContext> _dbFactory;
        private readonly IJSRuntime _js;
        private const string DbFileName = "chess.db";

        public DatabaseManager(IDbContextFactory<ChessDbContext> dbFactory, IJSRuntime js)
        {
            _dbFactory = dbFactory;
            _js = js;
        }

        public async Task DownloadDatabase()
        {
            if (File.Exists(DbFileName))
            {
                var bytes = await File.ReadAllBytesAsync(DbFileName);
                await _js.InvokeVoidAsync("downloadFileFromStream", "Schuelerliga_Datenbank.db", bytes);
            }
        }

        public async Task ImportDatabase(Stream uploadStream)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            await db.Database.CloseConnectionAsync();

            using (var fileStream = File.Create(DbFileName))
            {
                await uploadStream.CopyToAsync(fileStream);
            }

            await db.Database.EnsureCreatedAsync();
        }
    }
}