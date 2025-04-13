using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using ZeroStats.Game.Data.Enums;
using ZeroStats.Game.Data.Remote;

namespace ZeroStats.Game.Data
{
    public class GoogleSheetLoader
    {
        private readonly Func<string, string> _urlGet;

        public GoogleSheetLoader()
        {
            const string spreadsheetId = "1-baBp1opIxKzMVJr-3Yhb_6cUQGr1aJOiT428hNaZZE";
            _urlGet = url => $"https://docs.google.com/spreadsheets/d/{spreadsheetId}/gviz/tq?tqx=out:json&sheet={url}";
        }

        public GoogleSheetLoader(Func<string, string> urlGet)
        {
            _urlGet = urlGet;
        }

        public static int ParseInt(string? stringNumber) => (int)ParseDouble(stringNumber);

        public static double ParseDouble(string? stringNumber) =>
            double.Parse(string.IsNullOrEmpty(stringNumber) ? "0" : stringNumber, NumberStyles.Any,
                NumberFormatInfo.InvariantInfo);

        public async UniTask<CardDatabase> LoadAllData() => new()
        {
            cards = await FetchData("Cards", ParseCard),
            cardDescriptors = await FetchData("CardDescriptors", ParseDescription),
            colors = await FetchData("Colors", ParseColor),
            parameters = new ParametersData(
                (await FetchData("Parameters", ParseParameters)).ToDictionary(tuple => tuple.Item1,
                    tuple => tuple.Item2)
            ),
            engResults = await FetchData("Results", EngGameResults.Parse),
        };

        private static Card ParseCard(string?[] cells) => new()
        {
            Id = ParseInt(cells[0]),
            IconPath = cells[1] ?? string.Empty,
            Name = cells[2] ?? string.Empty,
            ResultDescription = cells[3] ?? string.Empty,
            ResultResourcesPath = cells[4],
            Stat1Delta = ParseInt(cells[5]),
            Stat2Delta = ParseInt(cells[6]),
            Stat3Delta = ParseInt(cells[7]),
            Stat4Delta = ParseInt(cells[8]),
            Group = ParseInt(cells[9]),
            AddCardIdInPool = ParseInt(cells[10]),
            StageDelayForAddToPool = ParseInt(cells[11]),
        };

        private static CardDescriptor ParseDescription(string?[] cells) => new()
        {
            CardId = ParseInt(cells[0]),
            Weight = ParseInt(cells[1]),
            NotApplicableStages = cells[2]?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(ParseInt).Cast<GameStage>().ToArray() ?? Array.Empty<GameStage>(),
            StatNumber = ParseInt(cells[3]),
            MinStatForUse = ParseInt(cells[4]),
            MaxStatForUse = ParseInt(cells[5]),
        };

        private (string, string) ParseParameters(string?[] arg) => (arg[0] ?? string.Empty, arg[1] ?? string.Empty);


        private ColorData ParseColor(string?[] arg) => new()
        {
            name = arg[0] ?? string.Empty,
            color = ColorUtility.TryParseHtmlString(arg[1], out var color) ? color : Color.white,
        };

        private async UniTask<T[]> FetchData<T>(string sheetName, Func<string?[], T> parseItem)
        {
            T[] result = default!;
            await FetchSheetData(sheetName, resp => { result = ParseGeneric(resp, parseItem).ToArray(); });
            return result ?? throw new InvalidOperationException("data not loaded");
        }

        private async UniTask FetchSheetData(string sheetName, Action<GvizResponse> onSuccess)
        {
            var www = UnityWebRequest.Get(_urlGet(sheetName));
            await www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Ошибка запроса листа {sheetName}: {www.error}");
            }
            else
            {
                string result = www.downloadHandler.text;
                // Удаляем обертку вида: google.visualization.Query.setResponse({...});
                int start = result.IndexOf('(');
                int end = result.LastIndexOf(')');
                if (start >= 0 && end > start)
                {
                    string jsonText = result.Substring(start + 1, end - start - 1);
                    GvizResponse resp = JsonUtility.FromJson<GvizResponse>(jsonText);
                    onSuccess.Invoke(resp);
                }
                else
                {
                    Debug.LogError("Не удалось распарсить ответ листа " + sheetName);
                }
            }
        }

        private IEnumerable<T> ParseGeneric<T>(GvizResponse response, Func<string?[], T> parseItem) =>
            response.table.rows
                .Select(row => row.c)
                .Select(cells =>
                {
                    try
                    {
                        return parseItem(cells.Select(cell => cell.v).ToArray());
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e + "\n " + string.Join("|", cells.Select(c => c.v)));
                        throw;
                    }
                });

        // Вспомогательные классы для парсинга JSON от Google Sheets (формат gviz)
        [Serializable]
        public class GvizResponse
        {
            public GvizTable table = default!;
        }

        [Serializable]
        public class GvizTable
        {
            public GvizColumn[] cols = default!;
            public GvizRow[] rows = default!;
        }

        [Serializable]
        public class GvizColumn
        {
            public string label = default!;
        }

        [Serializable]
        public class GvizRow
        {
            public GvizCell[] c = default!;
        }

        [Serializable]
        public class GvizCell
        {
            public string? v;
        }
    }
}