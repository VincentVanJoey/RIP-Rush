using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace RIPRUSH.Entities {

    public class SaveData {

        // ideally more stuff will go in here later idk
        // TODO - save settings preferences as well as the stuff besiides high score written already
        // TODO - maybe add multi=profile support for extra polis at the end???

        public float HighScore { get; set; } = 0f;
        public string PlayerName { get; set; } = ""; // will be ideally set when I actually do more save file stuff so they have a distinct profile
        public Dictionary<string, int> Upgrades { get; set; } = new Dictionary<string, int>(); // I'd be able to track tiered upgrades e.g. {"Health Up": 2, "Jump": 1}
        public int TotalCoins { get; set; } = 0; // the eventual "currency" amount I'd want the player to accumulate for upgrades but not coins probably 
    }
    public static class SaveFileManager {

        private static readonly string _path = Path.Combine(AppContext.BaseDirectory, "savefile.json");

        private static readonly JsonSerializerOptions _jsonOptions = new() {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static SaveData Data { get; private set; } = new();


        // Can use these versus having to do specific direct data access every time 
        // CIS 400 epic callback right here!
        public static T Get<T>(Func<SaveData, T> selector) => selector(Data);
        public static void Set(Action<SaveData> setter) {
            setter(Data);
            Save(); // shold auto-save on set for stuff (yippie)
        } 

        public static void Initialize() {
            if (!File.Exists(_path)) { // if no file, then make it
                Data = new SaveData();
                Save(); // create a new file
                return;
            }

            try {
                string json = File.ReadAllText(_path);
                var loaded = JsonSerializer.Deserialize<SaveData>(json, _jsonOptions);
                if (loaded != null) {
                    Data = loaded;
                }
            }
            catch {
                // defaults if error
                Data = new SaveData();
            }
        }

        public static void Save() {
            try {
                string json = JsonSerializer.Serialize(Data, _jsonOptions);
                File.WriteAllText(_path, json);
            }
            catch (Exception ex) {
                Console.WriteLine($"Failed to save data: {ex.Message}"); // mainly debug for now, but use log file(?) or gumui pop-up later
            }
        }


        public static void Reset() {
            Data = new SaveData();
            Save();
        }

        public static void SetHighScore(float newScore) {
            Set(d =>
            {
                if (newScore > d.HighScore)
                    d.HighScore = newScore;
            });
        }
    }

}
