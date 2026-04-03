using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// This is a "container" for what you want to save
[System.Serializable]
public class SaveData
{
    public float timeOfDay; // If you want to save the exact minute

    public ResourceData resourceDataSaved;
    // public GameManager gameManager;

    [SerializeField] public List<GameObject> _towerObjects;

    public Spawner spawner1,spawner2,spawner3;
    public int waveNumber1 = 1, waveNumber2 = 1, waveNumber3 = 1;
}

public class SaveGameManager : MonoBehaviour
{
    private string savePath;

    private void Awake()
    {
        // Sets the path to: C:/Users/[User]/AppData/LocalLow/[Company]/[Game]/save.json
        //savePath = Application.persistentDataPath + "/save.json";
    }


    private void Start()
    {
        //// Every time the scene starts/restarts, this runs automatically
        //if (File.Exists(savePath))
        //{
        //    Debug.Log("Save found, Loading Game");
        //    LoadGame();
        //}
        //else
        //{
        //    Debug.Log("No save found, starting fresh New Game.");
        //}
    }

    
    public void OnRestartButtonClick()
    {
        Time.timeScale = 1f; // Essential!
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void LoadGame()
    {
        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // GameManager.Instance = data.gameManager; // make sure this works
        GameManager.Instance.DropManager.resourceData = data.resourceDataSaved;
        

        Debug.Log("Scene successfully rebuilt from Daybreak save.");
    }


    public void SaveGame(float currentTime, DropResourceManager resourceManager)
    {
        SaveData save = new SaveData();
        save.timeOfDay = currentTime;

        // This copies the entire set of numbers at once!
        save.resourceDataSaved = resourceManager.resourceData;
        
        save._towerObjects = GameManager.Instance.towersObjects;

        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Saved resources and time!");
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


