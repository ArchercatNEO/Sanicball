using System;
using System.Linq;
using Sanicball.Data;
using SanicballCore;

public static class Globals
{
    //? This data is saved to a json file, we use get/set to ensure the files are always updated
    #region JSON data
    private static MatchSettings matchSettings = Save.LoadFile<MatchSettings>("MatchSettings.json");
    public static MatchSettings MatchSettings
    {
        get { return matchSettings; }
        set { Save.SaveFile("MatchSettings.json", value); matchSettings = value; }
    }

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

    //? Esport mode thingys for... no clue
    #region ESports

    public static bool ESportsReady()
    {
        if (!GameSettings.Instance.eSportsReady) return false;

        return !Client.clients[Constants.guid].players.Values.Any(
            player => player.charId != 13
        );
    }

    #endregion ESports
}

public enum Scene
{
    Intro,
    Menu,
    Lobby,
    Race
}