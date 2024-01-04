using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class StaticUpdater
{
    private static readonly GameObject gameObject;
    private static List<Action> toRemove = new();
    
    static StaticUpdater()
    {
        gameObject = new("Static Updater");
        Object.DontDestroyOnLoad(gameObject);
    }

    public static void PermanentScript<T>() where T : MonoBehaviour
    {
        gameObject.AddComponent<T>();
    }

    public static void TemporaryScript<T>() where T : MonoBehaviour
    {
        gameObject.AddComponent<T>();
    }
}