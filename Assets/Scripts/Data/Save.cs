using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public static class Save
{
    static readonly string path = Application.persistentDataPath;
    public static T LoadFile<T>(string filename) where T : new()
    {
        //Make sure file exists
        string fullPath = $"{path}/{filename}";
        if (!File.Exists(fullPath))
        {
            Debug.LogError($"{filename} has not been loaded - file not found.");
            return new T();
        }

        //Load file contents and make sure it isn't empty
        string dataString = new StreamReader(fullPath).ReadToEnd();
        if (dataString.Length == 0)
        {
            Debug.LogError($"Failed to load {filename}: file is empty.");
            return new T();
        }

        //Deserialize from JSON into a data object
        try
        {
            Debug.Log($"{filename} loaded successfully.");
            return JsonConvert.DeserializeObject<T>(dataString);
        }
        catch (JsonException ex)
        {
            Debug.LogError($"Failed to parse {filename}! JSON converter info: {ex.Message}");
            return new T();
        }
    }

    public static void SaveFile(string filename, object objToSave)
    {
        string fullPath = $"{path}/{filename}";
        string data = JsonConvert.SerializeObject(objToSave);

        new StreamWriter(fullPath).Write(data);
        Debug.Log(filename + " saved successfully.");
    }
}