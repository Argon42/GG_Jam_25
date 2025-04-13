using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZeroStats.Game.Data.Remote
{
    [Serializable]
    public class ParametersData
    {
        [SerializeField] private string[] keys;
        [SerializeField] private string[] values;

        private Dictionary<string, string>? _cached;

        public Dictionary<string, string> Parameters =>
            _cached ??= keys.Zip(values, (k, v) => (k, v)).ToDictionary(t => t.k, t => t.v);

        public ParametersData(Dictionary<string, string> parameters)
        {
            keys = parameters.Keys.ToArray();
            values = parameters.Values.ToArray();
        }

        public int GetInt(string name) =>
            GoogleSheetLoader.ParseInt(Parameters[name]);

        public float GetFloat(string name) =>
            (float)GoogleSheetLoader.ParseDouble(Parameters[name]);
    }
}