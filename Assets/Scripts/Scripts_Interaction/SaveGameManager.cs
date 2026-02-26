using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// This is a "container" for what you want to save
[System.Serializable]
public class SaveData
{
    public float timeOfDay; // If you want to save the exact minute

    public DropResourceManager dropResourceManager;
}

public class SaveGameManager : MonoBehaviour
{
    private string savePath;

    private void Awake()
    {
        // Sets the path to: C:/Users/[User]/AppData/LocalLow/[Company]/[Game]/save.json
        savePath = Application.persistentDataPath + "/save.json";
    }

    public void SaveGame(float currentTime, DropResourceManager resourceManagerReference)
    {
        SaveData data = new SaveData();
        data.timeOfDay = currentTime;
        data.dropResourceManager = resourceManagerReference;

        // Convert the "data" object into a string of text (JSON)
        string json = JsonUtility.ToJson(data);

        // Write that text to the file
        File.WriteAllText(savePath, json);

        Debug.Log("Game Auto-Saved at Day: ");
    }

    public void ResetGameData()
    {
        string path = Application.persistentDataPath + "/save.json";

        // 1. Delete the physical save file
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted.");
        }
    }
}


