using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDataManager
{
    /// <summary>
    /// Reset All Values back to 0
    /// </summary>
    public static void ResetValues()
    {
        // Reset PlayerOne
        playerOne.Score = 0;
        playerOne.HighestCombo = 0;
        playerOne.AmountOfDodges = 0;
        playerOne.HighestMultiplier = 0;
        playerOne.Bounces = 0;

        // Reset PlayerTwo
        playerTwo.Score = 0;
        playerTwo.HighestCombo = 0;
        playerTwo.AmountOfDodges = 0;
        playerTwo.HighestMultiplier = 0;
        playerTwo.Bounces = 0;
    }

    public static PlayerData playerOne = new PlayerData()
    {
        Score = 0,
        HighestCombo = 0,
        AmountOfDodges = 0,
        HighestMultiplier = 0,
        Bounces = 0,
    };
    public static PlayerData playerTwo = new PlayerData()
    {
        Score = 0,
        HighestCombo = 0,
        AmountOfDodges = 0,
        HighestMultiplier = 0,
        Bounces = 0
    };
}

public struct PlayerData
{
    public int Score { get; set; }
    public int HighestCombo { get; set; }
    public int AmountOfDodges { get; set; }
    public int HighestMultiplier { get; set; }
    public int Bounces { get; set; }
}