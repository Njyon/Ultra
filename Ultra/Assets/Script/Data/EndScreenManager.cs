using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour
{
    public List<TextManager> textManger;

    private void Start()
    {
        //PlayerOne
        textManger[0].score.text = "Score: " + PlayerDataManager.playerOne.Score.ToString();
        textManger[0].maxCombo.text = "MaxCombo: " + PlayerDataManager.playerOne.HighestCombo.ToString();
        textManger[0].dodges.text = "Dodges: " + PlayerDataManager.playerOne.AmountOfDodges.ToString();
        textManger[0].maxMultiplier.text = "MaxMultiplier: " + PlayerDataManager.playerOne.HighestMultiplier.ToString();
        textManger[0].bounces.text = "Boucnes: " + PlayerDataManager.playerOne.Bounces.ToString();

        //PlayerTwo
        textManger[1].score.text = "Score: " + PlayerDataManager.playerTwo.Score.ToString();
        textManger[1].maxCombo.text = "MaxCombo: " + PlayerDataManager.playerTwo.HighestCombo.ToString();
        textManger[1].dodges.text = "Dodges: " + PlayerDataManager.playerTwo.AmountOfDodges.ToString();
        textManger[1].maxMultiplier.text = "MaxMultiplier: " + PlayerDataManager.playerTwo.HighestMultiplier.ToString();
        textManger[1].bounces.text = "Boucnes: " + PlayerDataManager.playerTwo.Bounces.ToString();
    }
}

[System.Serializable]
public class TextManager
{
    public Text score;
    public Text maxCombo;
    public Text dodges;
    public Text maxMultiplier;
    public Text bounces;
}