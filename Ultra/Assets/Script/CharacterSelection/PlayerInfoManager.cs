using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager : MonoBehaviour
{
    public static PlayerInfo playerOne = new PlayerInfo()
    {
        playerID = "PlayerOne",
        character = Characters.None,
        color = Color.white,
        charColor = CharColors.None
    };
    public static PlayerInfo playerTwo = new PlayerInfo()
    {
        playerID = "PlayerTwo",
        character = Characters.None,
        color = Color.white,
        charColor = CharColors.None
    };

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
