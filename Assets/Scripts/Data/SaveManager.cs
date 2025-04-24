using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static readonly string SavePath = Application.persistentDataPath + "/save.json";
    private static HashSet<string> collectedCoins = new HashSet<string>();

    public static void SavePlayerData(float x, float y, float health, int coin)
    {
        var data = new PlayerData(x, y, health, coin, new List<string>(collectedCoins).ToArray());
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SavePath, json);
    }

    public static PlayerData LoadPlayerData()
    {
        if (!File.Exists(SavePath)) return null;

        string json = File.ReadAllText(SavePath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);

        collectedCoins = new HashSet<string>(data.collectedCoinIDs ?? new string[0]);

        return data;
    }

    public static void DeleteSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
        }
        collectedCoins.Clear();
    }

    public static bool SaveExists()
    {
        return File.Exists(SavePath);
    }

    public static void MarkCoinCollected(string coinID)
    {
        collectedCoins.Add(coinID);
    }

    public static bool IsCoinCollected(string coinID)
    {
        return collectedCoins.Contains(coinID);
    }
}