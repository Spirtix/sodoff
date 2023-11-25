using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using sodoff.Model;
using sodoff.Schema;
using System.Text.RegularExpressions;

namespace sodoff.Services;
public class GameDataService {
    private readonly DBContext ctx;

    public GameDataService(DBContext ctx) {
        this.ctx = ctx;
    }

    public bool SaveGameData(Viking viking, int gameId, bool isMultiplayer, int difficulty, int gameLevel, string xmlDocumentData, bool win, bool loss) {
        Model.GameData? gameData = viking.GameData.FirstOrDefault(x => x.GameId == gameId && x.IsMultiplayer == isMultiplayer && x.Difficulty == difficulty && x.GameLevel == gameLevel && x.Win == win && x.Loss == loss);
        if (gameData == null) {
            gameData = new Model.GameData {
                GameId = gameId,
                IsMultiplayer = isMultiplayer,
                Difficulty = difficulty,
                GameLevel = gameLevel,
                Win = win,
                Loss = loss,
                GameDataPairs = new List<GameDataPair>()
            };
            viking.GameData.Add(gameData);

        }
        gameData.DatePlayed = DateTime.Now;
        SavePairs(gameData, xmlDocumentData);
        ctx.SaveChanges();
        return true;
    }

    public GameDataSummary GetGameData(Viking viking, [FromForm] int gameId, bool isMultiplayer, int difficulty, int gameLevel, string key, int count, bool AscendingOrder, bool buddyFilter, DateTime? startDate = null, DateTime? endDate = null) {
        // TODO: Buddy filter
        List<GameDataResponse> selectedData;
        IQueryable<Model.GameData> query = ctx.GameData.Where(x => x.GameId == gameId && x.IsMultiplayer == false && x.Difficulty == difficulty && x.GameLevel == gameLevel);

        if (startDate != null && endDate != null)
            query = query.Where(x => x.DatePlayed >= startDate && x.DatePlayed <= endDate.Value.AddMinutes(2));

        selectedData = query.SelectMany(e => e.GameDataPairs)
            .Where(x => x.Name == key)
            .Select(e => new GameDataResponse(e.GameData.Viking.Name, e.GameData.Viking.Uid, e.GameData.DatePlayed, e.GameData.Win, e.GameData.Loss, e.Value))
            .Take(count)
            .ToList();

        if (AscendingOrder)
            selectedData.Sort((a, b) => a.Value.CompareTo(b.Value));
        else
            selectedData.Sort((a, b) => b.Value.CompareTo(a.Value));

        return GetSummaryFromResponse(viking, isMultiplayer, difficulty, gameLevel, key, selectedData);
    }

    private GameDataSummary GetSummaryFromResponse(Viking viking, bool isMultiplayer, int difficulty, int gameLevel, string key, List<GameDataResponse> selectedData) {
        GameDataSummary gameData = new();
        gameData.IsMultiplayer = isMultiplayer;
        gameData.Difficulty = difficulty;
        gameData.GameLevel = gameLevel;
        gameData.Key = key;
        gameData.UserPosition = -1;
        gameData.GameDataList = new Schema.GameData[selectedData.Count];
        for (int i = 0; i < selectedData.Count; i++) {
            Schema.GameData data = new();
            data.RankID = i + 1;
            data.IsMember = true;
            data.UserName = selectedData[i].Name;
            data.Value = selectedData[i].Value;
            data.DatePlayed = selectedData[i].DatePlayed;
            data.Win = selectedData[i].Win ? 1 : 0;
            data.Loss = selectedData[i].Loss ? 1 : 0;
            data.UserID = selectedData[i].Uid;
            gameData.GameDataList[i] = data;
            if (data.UserName == viking.Name && gameData.UserPosition == -1)
                gameData.UserPosition = i;
        }
        if (gameData.UserPosition == -1)
            gameData.UserPosition = selectedData.Count;
        return gameData;
    }

    private void SavePairs(Model.GameData gameData, string xmlDocumentData) {
        foreach (var pair in GetGameDataPairs(xmlDocumentData)) {
            GameDataPair? dbPair = gameData.GameDataPairs.FirstOrDefault(x => x.Name == pair.Name);
            if (dbPair == null)
                gameData.GameDataPairs.Add(pair);
            else
                dbPair.Value = pair.Value;
        }
    }

    private ICollection<GameDataPair> GetGameDataPairs(string xmlDocumentData) {
        List<GameDataPair> pairs = new();
        foreach (Match match in Regex.Matches(xmlDocumentData, @"<(\w+)>(.*?)<\/\1>")) {
            pairs.Add(new GameDataPair {
                Name = match.Groups[1].Value,
                Value = int.Parse(match.Groups[2].Value)
            });
        }
        return pairs;
    }

    struct GameDataResponse {
        public GameDataResponse(string Name, Guid Uid, DateTime DatePlayed, bool Win, bool Loss, int Value) {
            this.Name = Name;
            this.Uid = Uid;
            this.DatePlayed = DatePlayed;
            this.Win = Win;
            this.Loss = Loss;
            this.Value = Value;
        }
        public string Name;
        public Guid Uid;
        public DateTime DatePlayed;
        public bool Win;
        public bool Loss;
        public int Value;
    }
}
