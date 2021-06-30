using System.IO;
using UnityEngine;

/// <summary>
/// The Save Manager is responsible for Saving and Loading the Save Data, which currently only holds high scores
/// When attempting to save, we check for a save file and create one if need be
/// When attempting to load, if there is no save file, we save by creating a blank save file
/// </summary>
public static class SaveManager
{
    [Header("Directory Information")]
    public static string directory = "Saves";
    public static string fileName = "save.sav";

    /// <summary>
    /// Save is responsible for taking Save Data and converting it to JSON
    /// TODO: Add some encryption
    /// </summary>
    /// <param name="saveData"></param>
    public static void Save(SaveData saveData)
    {
        string saveDirectory = GetFullPathDirectory();

        if (!Directory.Exists(saveDirectory))
            Directory.CreateDirectory(saveDirectory);

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(saveDirectory + fileName, json);
    }

    /// <summary>
    /// Load returns a JSON Save Data object or creates a blank one if none exist
    /// </summary>
    /// <returns></returns>
    public static SaveData Load()
    {
        string fullDirectoryPath = GetFullPathDirectory() + fileName;
        SaveData saveData = new SaveData();

        if(File.Exists(fullDirectoryPath))
        {
            string json = File.ReadAllText(fullDirectoryPath);
            saveData = JsonUtility.FromJson<SaveData>(json);
        }
        else { Debug.Log($"No file directory found for: {fullDirectoryPath} -- creating one..."); }
        Save(saveData);

        return saveData;
    }

    /// <summary>
    /// Helper method to save string space for the directory names
    /// </summary>
    /// <returns></returns>
    private static string GetFullPathDirectory()
    {
        return Application.persistentDataPath + "/" + directory + "/";
    }
}