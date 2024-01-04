using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

//TODO, rewrite matchSettings to use this (globals)
public class LazySaveFile<T> where T: class, new()
{
    private readonly string relativePath;
    private T? file = null;
    public T File
    {
        get
        {
            if (file is not null) { return file; }
            
            //Make sure file exists
            string fullPath = $"{Application.persistentDataPath}/{relativePath}";
            if (!System.IO.File.Exists(fullPath))
            {
                Debug.LogError($"{fullPath} has not been loaded - file not found.");
                file = new T();
                return file;
            }

            //Load file contents and make sure it isn't empty
            string dataString = new StreamReader(fullPath).ReadToEnd();
            if (dataString.Length == 0)
            {
                Debug.LogError($"Failed to load {fullPath}: file is empty.");
                file = new T();
                return file;
            }

            //Deserialize from JSON into a data object
            try
            {
                file = JsonConvert.DeserializeObject<T>(dataString);
                Debug.Log($"{fullPath} loaded successfully.");
                return file;
            }
            catch (JsonException ex)
            {
                Debug.LogError($"Failed to parse {fullPath}! JSON converter info: {ex.Message}");
                file = new T();
                return file;
            }
        }
        set => file = value;
    }

    public LazySaveFile(string RelativePath)
    {
        relativePath = RelativePath;
        AppDomain.CurrentDomain.ProcessExit += (object sender, EventArgs e) =>
            {
                string fullPath = $"{Application.persistentDataPath}/{RelativePath}";
                string data = JsonConvert.SerializeObject(File);
                new StreamWriter(fullPath).Write(data);
                Debug.Log($"{fullPath} saved successfully.");
            };
    }
}