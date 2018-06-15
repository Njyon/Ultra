using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager : MonoBehaviour
{
    public static PlayerInfo playerOne = new PlayerInfo()
    {
        playerID = "PlayerOne",
        character = Characters.None,
        color = Color.white
    };
    public static PlayerInfo playerTwo = new PlayerInfo()
    {
        playerID = "PlayerTwo",
        character = Characters.None,
        color = Color.white
    };

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
