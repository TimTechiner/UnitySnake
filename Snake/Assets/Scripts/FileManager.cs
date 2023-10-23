using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileManager
{
    private const string DIRECTORY = "SavedData";
    private const string PATH = DIRECTORY + "/BestScore.json";
    public static SaveData Load()
    {
        if (!Directory.Exists(DIRECTORY))
        {
            Directory.CreateDirectory(DIRECTORY);
        }

        if (!File.Exists(PATH))
        {
            using (var fs = File.Create(PATH)) { }
            var defaultSaveData = new SaveData();
            Save(defaultSaveData);
        }

        var json = File.ReadAllText(PATH);

        return JsonUtility.FromJson<SaveData>(json);
    }

    public static void Save(SaveData saveData)
    {
        var json = JsonUtility.ToJson(saveData);

        if (!Directory.Exists(DIRECTORY))
        {
            Directory.CreateDirectory(DIRECTORY);
        }

        if (!File.Exists(PATH))
        {
            using (var fs = File.Create(PATH)) { }
        }

        File.WriteAllText(PATH, json);
    }
}
