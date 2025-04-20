using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static string savePath = Application.persistentDataPath + "/playerSave.json";

    public static void SavePlayerData(float posX, float posY, int health, int coin)
    {
        PlayerData data = new PlayerData(posX, posY, health, coin);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }

    public static PlayerData LoadPlayerData()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No save file found");
            return null;
        }

        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<PlayerData>(json);
    }

    public static bool SaveExists()
    {
        return File.Exists(savePath);
    }

    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
    }
}