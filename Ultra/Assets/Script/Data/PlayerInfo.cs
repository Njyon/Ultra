using UnityEngine;

public enum CharColors
{
    Orange,
    Blue,
    Pink,
    Red,
    Yellow,
    Green,
    None
}

public class PlayerInfo
{
    public string playerID;
    public Characters character;
    public CharColors charColor;
    [ColorUsageAttribute(true, true)] public Color color;
}
