using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager : MonoBehaviour
{
    public PlayerInfo playerOne = new PlayerInfo()
    {
        playerID = "PlayerOne",
        character = Characters.None
    };
    public PlayerInfo playerTwo = new PlayerInfo()
    {
        playerID = "PlayerTwo",
        character = Characters.None
    };

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
