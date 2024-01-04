using UnityEngine;

public static class TextureFactory
{
    public static Texture2D NotSega => Resources.Load<Texture2D>("Art/User Interface/Menu+Intro/Intro_Notsega");
    public static Texture2D SanicTeam => Resources.Load<Texture2D>("Art/User Interface/Menu+Intro/Intro_Sanicteam");
    public static Texture2D Sanicball => Resources.Load<Texture2D>("Art/User Interface/Menu+Intro/GameTitle");

    public static Texture2D WhiteBox = null;
}
