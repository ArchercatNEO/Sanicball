using System;

public static class Constants
{
    #region Timer Values
    public static readonly float StartRaceTimeout = 3;

    #endregion Timer Values

    #region Online Constants

    public static readonly Guid guid = Guid.NewGuid();
    public static readonly string APP_ID = "Sanicball";

    #endregion Online Constants

    //? Scene Names, this way we can change where every scripts directs to safely
    #region Scene Names

    public static readonly string introName = "Intro";
    public static readonly string menuName = "Menu";
    public static readonly string lobbyName = "Lobby";
    public static readonly string greenHillName = "Track_GreenHillZone";
    public static readonly string desertName = "Track_Desert";
    public static readonly string volcanoName = "Track_FlameCore";
    public static readonly string rainbowName = "Track_RainbowRoad";
    public static readonly string mountainName = "Track_SnowyMountain";

    #endregion Scene Names
}