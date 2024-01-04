using System;
using System.Linq;
using Sanicball.Data;
using SanicballCore;

public static class Globals
{
    //? This data is saved to a json file, we use get/set to ensure the files are always updated
    #region JSON data
/*     private static readonly LazySaveFile<MatchSettings> matchSettings = new("MatchSettings.json");
    public static MatchSettings MatchSettings
    {
        get => matchSettings.File;
        set => matchSettings.File = value;
    } */

    #endregion

    //? Player/Match State
    #region State


    /// <summary>
    /// Current settings for this match. On remote clients, this is only used for showing settings on the UI.
    /// </summary>
    public static MatchSettings settings;

    public static Scene scene = Scene.Intro;

    #endregion State

    //? Built-in Functions to add/remove from clients/players/settings
    #region State Initializer Functions

    public static void FromDefault()
    {
        Client.Reset();
        settings = MatchSettings.CreateDefault();
    }

    #endregion State Initializer Functions
}

public enum Scene
{
    Intro,
    Menu,
    Lobby,
    Race
}