using Code.Game.Configs.DataModels;
using UnityEngine;

namespace Code.Game.Systems.Services
{
    public class SaveLoadService
    {
        private const string SaveKey = "game_save";

        public void Save(GameSaveData data)
        {
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        public GameSaveData? Load()
        {
            if (!PlayerPrefs.HasKey(SaveKey))
            {
                return null;
            }

            var json = PlayerPrefs.GetString(SaveKey);
            return JsonUtility.FromJson<GameSaveData>(json);
        }

    }
}