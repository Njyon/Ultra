using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour
{
    public List<TextManager> textManger;

    private void Start()
    {
        //PlayerOne
        textManger[0].header.color = PlayerInfoManager.playerOne.color;
        textManger[0].score.text = "Score: " + PlayerDataManager.playerOne.Score.ToString();
        textManger[0].maxCombo.text = "MaxCombo: " + PlayerDataManager.playerOne.HighestCombo.ToString();
        textManger[0].dodges.text = "Dodges: " + PlayerDataManager.playerOne.AmountOfDodges.ToString();
        textManger[0].maxMultiplier.text = "MaxMultiplier: " + PlayerDataManager.playerOne.HighestMultiplier.ToString();
        textManger[0].bounces.text = "Bounces: " + PlayerDataManager.playerOne.Bounces.ToString();

        //PlayerTwo
        textManger[1].header.color = PlayerInfoManager.playerTwo.color;
        textManger[1].score.text = "Score: " + PlayerDataManager.playerTwo.Score.ToString();
        textManger[1].maxCombo.text = "MaxCombo: " + PlayerDataManager.playerTwo.HighestCombo.ToString();
        textManger[1].dodges.text = "Dodges: " + PlayerDataManager.playerTwo.AmountOfDodges.ToString();
        textManger[1].maxMultiplier.text = "MaxMultiplier: " + PlayerDataManager.playerTwo.HighestMultiplier.ToString();
        textManger[1].bounces.text = "Bounces: " + PlayerDataManager.playerTwo.Bounces.ToString();

        if(PlayerDataManager.playerOne.Score > PlayerDataManager.playerTwo.Score)
        {
            textManger[1].crown.SetActive(false);
        }
        else
        {
            textManger[0].crown.SetActive(false);
        }

        Invoke("EnableInput", 5);
    }

    void EnableInput()
    {
        InputManager.p1_OnKeyReleased += GoBackToMainMenu;
        InputManager.p2_OnKeyReleased += GoBackToMainMenu;
    }

    void RemoveInput()
    {
        InputManager.p1_OnKeyReleased -= GoBackToMainMenu;
        InputManager.p2_OnKeyReleased -= GoBackToMainMenu;
    }

    void GoBackToMainMenu(KeyCode noNeed)
    {
        RemoveInput();
        StartCoroutine(LoadNewScene());
    }

    IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(0.1f);
        AsyncOperation async = SceneManager.LoadSceneAsync(1);
        while (!async.isDone)
        {
            yield return null;
        }
    }
}


[System.Serializable]
public class TextManager
{
    public Text header;
    public Text score;
    public Text maxCombo;
    public Text dodges;
    public Text maxMultiplier;
    public Text bounces;
    public GameObject crown;
}