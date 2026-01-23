using System.IO;
using ExcelDataReader;
using ChessTournamentManager.Shared.Models;

namespace ChessTournamentManager.Client.Services
{
    public class ExcelImportService
    {
        public List<Player> ReadExcelStream(Stream fileStream)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var players = new List<Player>();

            using (var reader = ExcelReaderFactory.CreateReader(fileStream))
            {
                int nameCol = -1, eloCol = -1, clubCol = -1, countryCol = -1, rankCol = -1;
                bool headerFound = false;

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var val = reader.GetValue(i)?.ToString()?.ToLower() ?? "";
                        if (val.Contains("name"))
                        {
                            headerFound = true;
                            nameCol = i;

                            for (int j = 0; j < reader.FieldCount; j++)
                            {
                                var headerVal = reader.GetValue(j)?.ToString()?.ToLower() ?? "";
                                if (headerVal.Contains("elo") || headerVal == "rg") eloCol = j;
                                if (headerVal == "land" || headerVal == "fed") countryCol = j;
                                if (headerVal.Contains("verein") || headerVal.Contains("club") || headerVal.Contains("ort")) clubCol = j;

                                // NEU: Wir suchen die Rang-Spalte ("Rg." oder "Rang")
                                if (headerVal.StartsWith("rg") || headerVal.StartsWith("rk") || headerVal.StartsWith("rang")) rankCol = j;
                            }
                            break;
                        }
                    }
                    if (headerFound) break;
                }

                while (reader.Read())
                {
                    if (nameCol == -1) break;

                    string name = reader.GetValue(nameCol)?.ToString() ?? "";
                    if (string.IsNullOrWhiteSpace(name) || name.Contains("Name")) continue;

                    string eloStr = eloCol != -1 ? (reader.GetValue(eloCol)?.ToString() ?? "") : "";
                    int? elo = int.TryParse(eloStr, out int parsedElo) ? parsedElo : null;

                    string country = countryCol != -1 ? (reader.GetValue(countryCol)?.ToString() ?? "") : "";
                    string clubName = clubCol != -1 ? (reader.GetValue(clubCol)?.ToString() ?? "") : "";

                    // NEU: Den Rang auslesen
                    string rankStr = rankCol != -1 ? (reader.GetValue(rankCol)?.ToString() ?? "") : "";
                    // Wenn kein Rang da ist, nehmen wir einfach die aktuelle Position in der Liste (1, 2, 3...)
                    int rank = int.TryParse(rankStr, out int parsedRank) ? parsedRank : players.Count + 1;

                    string firstname = "";
                    string lastname = "";

                    if (name.Contains(','))
                    {
                        var parts = name.Split(',');
                        lastname = parts[0].Trim();
                        firstname = parts.Length > 1 ? parts[1].Trim() : "";
                    }
                    else
                    {
                        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        lastname = parts.Length > 0 ? parts[0] : "";
                        firstname = parts.Length > 1 ? parts[1] : "";
                    }

                    players.Add(new Player
                    {
                        Firstname = firstname,
                        Lastname = lastname,
                        Elo = elo,
                        Country = country,
                        TournamentRank = rank, // HIER SPEICHERN WIR DEN RANG
                        Club = string.IsNullOrWhiteSpace(clubName) ? null : new Club { Name = clubName }
                    });
                }
            }
            return players;
        }
    }
}