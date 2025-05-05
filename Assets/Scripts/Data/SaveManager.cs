using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static readonly string SavePath = Application.persistentDataPath + "/save.json";
    
    private static HashSet<string> collectedCoins = new HashSet<string>();
    private static HashSet<string> collectedHealthPickups = new HashSet<string>();

    public static bool IsHealthCollected(string healthID) => collectedHealthPickups.Contains(healthID);
    public static bool IsCoinCollected(string coinID) => collectedCoins.Contains(coinID);
    public static bool SaveExists() => File.Exists(SavePath);
    public static void MarkHealthCollected(string healthID) => collectedHealthPickups.Add(healthID);
    public static void MarkCoinCollected(string coinID) => collectedCoins.Add(coinID);

    public static void SavePlayerData(float x, float y, float health, int coin)
    {
        var data = new PlayerData(
            x, y, health, coin,
            new List<string>(collectedCoins).ToArray(),
            new List<string>(collectedHealthPickups).ToArray()
        );

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SavePath, json);
    }

    public static PlayerData LoadPlayerData()
    {
        if (!File.Exists(SavePath)) return null;

        string json = File.ReadAllText(SavePath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);

        collectedCoins = new HashSet<string>(data.collectedCoinIDs ?? new string[0]);
        collectedHealthPickups = new HashSet<string>(data.collectedHealthPickupIDs ?? new string[0]);

        return data;
    }

    public static void DeleteSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
        }

        collectedCoins.Clear();
        collectedHealthPickups.Clear();
    }

    public static void ResetCollectedItems()
    {
        collectedCoins.Clear();
        collectedHealthPickups.Clear();
    }

}