using Microsoft.JSInterop;
using Microsoft.Data.Sqlite; // NEU: Wird gebraucht, um die Datei-Sperre zu lösen!

namespace ChessTournamentManager.Client.Services
{
    public class DatabaseManager
    {
        private readonly IJSRuntime _jsRuntime;
        private const string DbFilename = "chess.db";

        public DatabaseManager(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task ImportDatabase(Stream fileStream)
        {
            // 1. ZWINGEND: Löst die Datenbank-Sperre im Browser!
            SqliteConnection.ClearAllPools();

            // 2. Neue Daten in den Arbeitsspeicher laden
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            // 3. Alte Datei löschen und neu schreiben
            if (File.Exists(DbFilename)) File.Delete(DbFilename);
            await File.WriteAllBytesAsync(DbFilename, fileBytes);
        }

        public async Task DownloadDatabase()
        {
            // ZWINGEND: Sperre lösen, damit wir die Datei überhaupt lesen dürfen
            SqliteConnection.ClearAllPools();

            if (!File.Exists(DbFilename)) return;
            var fileBytes = await File.ReadAllBytesAsync(DbFilename);
            var base64 = Convert.ToBase64String(fileBytes);
            await _jsRuntime.InvokeVoidAsync("window.downloadBinaryFile", DbFilename, base64);
        }

        public async Task<string> GetDatabaseAsBase64Async()
        {
            // ZWINGEND: Sperre lösen
            SqliteConnection.ClearAllPools();

            if (!File.Exists(DbFilename)) return string.Empty;
            var fileBytes = await File.ReadAllBytesAsync(DbFilename);
            return Convert.ToBase64String(fileBytes);
        }
    }
}