using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem
{
    public static void SavePlayer(GameManager gameManager)
    {
        if (gameManager == null)
        {
            Debug.LogError("SaveSystem: GameManager reference is null.");
            return;
        }
        string path = Path.Combine(Application.persistentDataPath, "player.save");
        try
        {
            PlayerData data = new PlayerData(gameManager);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
            }
            Debug.Log("Saved");
        }
        catch (Exception exception)
        {
            Debug.LogError("SaveSystem: Failed to save player data. " + exception);
        }
    }

    public static PlayerData LoadPlayer()
    {
        string path = Path.Combine(Application.persistentDataPath, "player.save");
        if (!File.Exists(path))
        {
            Debug.Log("Saved file not found in " + path);
            return null;
        }
        try
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                if (data == null)
                    Debug.LogError("SaveSystem: Save file does not contain valid PlayerData.");
                else
                    Debug.Log("Loaded");
                return data;
            }
        }
        catch (Exception exception)
        {
            Debug.LogError("SaveSystem: Failed to load player data. " + exception);
            return null;
        }
    }
}