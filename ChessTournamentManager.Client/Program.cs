using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// Eigene Namespaces
using ChessTournamentManager.Client;
using ChessTournamentManager.Client.Services;
using ChessTournamentManager.Shared.Models;

namespace ChessTournamentManager.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // 1. Standard HttpClient für Blazor
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // 2. UNSERE EIGENEN SERVICES REGISTRIEREN
            builder.Services.AddScoped<ExcelImportService>();
            builder.Services.AddScoped<DatabaseManager>();

            // NEU: Der Auswertungs-Service für die Endtabelle!
            builder.Services.AddScoped<SchuelerligaService>();

            // 3. DATENBANK (SQLite) REGISTRIEREN
            builder.Services.AddDbContextFactory<ChessDbContext>(options =>
            {
                options.UseSqlite("Filename=chess.db");
            });

            await builder.Build().RunAsync();
        }
    }
}