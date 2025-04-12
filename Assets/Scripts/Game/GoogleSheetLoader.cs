using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroStats.Game
{
    public class GoogleSheetLoader
    {
        private const string spreadsheetId = "1-baBp1opIxKzMVJr-3Yhb_6cUQGr1aJOiT428hNaZZE"; // замените на свой ID

        [Serializable]
        public class CardDatabase
        {
            public Card[] cards = default!;
            public CardDescriptor[] cardDescriptors = default!;
        }

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

        public async UniTask<CardDatabase> Start()
        {
            Card[]? cards = null;
            CardDescriptor[]? cardDescriptors = null;

            // Загрузка листа Cards
            await FetchSheetData("Cards", resp => { cards = ParseCards(resp); });

            // Загрузка листа CardDescriptors
            await FetchSheetData("CardDescriptors", resp => { cardDescriptors = ParseCardDescriptors(resp); });

            // Формируем итоговую базу
            return new CardDatabase
            {
                cards = cards ?? throw new InvalidOperationException("data cards not loaded"),
                cardDescriptors = cardDescriptors ??
                                  throw new InvalidOperationException("data cardDescriptors not loaded"),
            };
        }

        private async UniTask FetchSheetData(string sheetName, Action<GvizResponse> onSuccess)
        {
            string url =
                $"https://docs.google.com/spreadsheets/d/{spreadsheetId}/gviz/tq?tqx=out:json&sheet={sheetName}";
            UnityWebRequest www = UnityWebRequest.Get(url);
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

        private Card[] ParseCards(GvizResponse response)
        {
            var list = new List<Card>();
            foreach (var row in response.table.rows)
            {
                if (row.c.Length < 9)
                    throw new InvalidOperationException("not enough columns. expected 9, got " + row.c.Length);
                var card = new Card
                {
                    Id = ParseInt(row.c[0].v),
                    IconPath = row.c[1].v ?? string.Empty,
                    Name = row.c[2].v ?? string.Empty,
                    ResultDescription = row.c[3].v ?? string.Empty,
                    ResultResourcesPath = row.c[4].v,
                    Stat1Delta = ParseInt(row.c[5].v),
                    Stat2Delta = ParseInt(row.c[6].v),
                    Stat3Delta = ParseInt(row.c[7].v),
                    Stat4Delta = ParseInt(row.c[8].v),
                };
                list.Add(card);
            }

            return list.ToArray();
        }

        private CardDescriptor[] ParseCardDescriptors(GvizResponse response)
        {
            var list = new List<CardDescriptor>();
            foreach (var row in response.table.rows)
            {
                if (row.c.Length < 6)
                    throw new InvalidOperationException("not enough columns. expected 6, got " + row.c.Length);

                var cd = new CardDescriptor
                {
                    CardId = ParseInt(row.c[0].v),
                    Weight = ParseInt(row.c[1].v),
                    NotApplicableStages = row.c[2].v?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(ParseInt).Cast<GameStage>().ToArray() ?? Array.Empty<GameStage>(),
                    StatNumber = ParseInt(row.c[3].v),
                    MinStatForUse = ParseInt(row.c[4].v),
                    MaxStatForUse = ParseInt(row.c[5].v),
                };
                list.Add(cd);
            }

            return list.ToArray();
        }

        private static int ParseInt(string? stringNumber)
        {
            return (int)double.Parse(stringNumber ?? "0", NumberStyles.Any, NumberFormatInfo.InvariantInfo);
        }
    }
}