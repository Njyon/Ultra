using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour
{
    [Header("Character To Spawn")]
    public GameObject character;
    [Header("PlayerSpawm")]
    public List<GameObject> playerSpawns;
    [Header("Plateaus")]
    public List<GameObject> Plateaus;
    [Header("Text")]
    public List<TextManager> textManger;
    [Header("HighestPoint")]
    public Transform maxScoreHight;

    GameObject playerOne;
    GameObject playerTwo;

    private void Start()
    {
        //PlayerOne
        textManger[0].header.color = PlayerInfoManager.playerOne.color;
        textManger[0].score.text = "Score: 0"; //+ PlayerDataManager.playerOne.Score.ToString();
        textManger[0].maxCombo.text = "MaxCombo: " + PlayerDataManager.playerOne.HighestCombo.ToString();
        textManger[0].dodges.text = "Dodges: " + PlayerDataManager.playerOne.AmountOfDodges.ToString();
        textManger[0].maxMultiplier.text = "MaxMultiplier: " + PlayerDataManager.playerOne.HighestMultiplier.ToString();
        textManger[0].bounces.text = "Bounces: " + PlayerDataManager.playerOne.Bounces.ToString();

        //PlayerTwo
        textManger[1].header.color = PlayerInfoManager.playerTwo.color;
        textManger[1].score.text = "Score: 0"; // + PlayerDataManager.playerTwo.Score.ToString();
        textManger[1].maxCombo.text = "MaxCombo: " + PlayerDataManager.playerTwo.HighestCombo.ToString();
        textManger[1].dodges.text = "Dodges: " + PlayerDataManager.playerTwo.AmountOfDodges.ToString();
        textManger[1].maxMultiplier.text = "MaxMultiplier: " + PlayerDataManager.playerTwo.HighestMultiplier.ToString();
        textManger[1].bounces.text = "Bounces: " + PlayerDataManager.playerTwo.Bounces.ToString();

        // Only production prototyp
        if(PlayerDataManager.playerOne.Score > PlayerDataManager.playerTwo.Score)
        {
            textManger[1].crown.SetActive(false);
        }
        else
        {
            textManger[0].crown.SetActive(false);
        }

        Invoke("EnableInput", 1);
        Init();
        StartCoroutine(CountPoints());
    }

    void Init()
    {
        //Spawn PlayerOne and Color his Character
        playerOne = Instantiate(character, playerSpawns[0].transform.position, Quaternion.identity);
        playerOne.GetComponent<MyCharacter>().clothRenderer.materials[0].SetColor("_EmissionColor", PlayerInfoManager.playerOne.color);
        playerOne.GetComponent<MyCharacter>().clothRenderer.materials[1].SetColor("_EmissionColor", PlayerInfoManager.playerOne.color);

        //Spawn PlayerTwo and Color his Character
        playerTwo = Instantiate(character, playerSpawns[1].transform.position, Quaternion.identity);
        playerTwo.GetComponent<MyCharacter>().clothRenderer.materials[0].SetColor("_EmissionColor", PlayerInfoManager.playerTwo.color);
        playerTwo.GetComponent<MyCharacter>().clothRenderer.materials[1].SetColor("_EmissionColor", PlayerInfoManager.playerTwo.color);
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
    bool PlayerOneWins(int p1_Score, int p2_Score)
    {
        if (p1_Score > p2_Score)
            return true;
        else if (p2_Score > p1_Score)
            return false;
        else
            return true;
    }
    float GetLoserHight(bool playerOneWon, float maxHight, int p1_Score, int p2_Score)
    {
        switch (playerOneWon)
        {
            case true:
                return (maxHight / p1_Score * p2_Score) * 0.2f;
            case false:
                return (maxHight / p2_Score * p1_Score) * 0.2f;
        }
        return 0;
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
    IEnumerator CountPoints()
    {
        int p1_Score = PlayerDataManager.playerOne.Score;
        int p2_Score = PlayerDataManager.playerTwo.Score;
        bool done = false;
        bool playerOneWon = PlayerOneWins(p1_Score, p2_Score);
        float p1_CurrentsScore = 0;
        float p2_CurrentsScore = 0;
        float winnerYPos = maxScoreHight.position.y;
        float loserYPos = GetLoserHight(playerOneWon, winnerYPos, p1_Score, p2_Score);

        Vector3 p1_PlateauStartPos = Plateaus[0].transform.position;
        Vector3 p2_PlateauStartPos = Plateaus[1].transform.position;

        //Vector3 p1_PlateauEndPos = new Vector3(p1_PlateauStartPos.x, p1_PlateauStartPos.y += p1_Score / 100);
        //Vector3 p2_PlateauEndPos = new Vector3(p2_PlateauStartPos.x, p2_PlateauStartPos.y += p1_Score / 100);

        float time = 0;
        float speed = 5f;
        

        yield return new WaitForSeconds(1f);
        
        while(!done)
        {
            time += Time.deltaTime / speed;
            // Count Score Up
            p1_CurrentsScore = Mathf.Lerp(0, p1_Score, time);
            p2_CurrentsScore = Mathf.Lerp(0, p2_Score, time);

            int p1_IntScore = (int)p1_CurrentsScore;
            int p2_IntScore = (int)p2_CurrentsScore;

            // Display new Score
            textManger[0].score.text = p1_IntScore.ToString();
            textManger[1].score.text = p2_IntScore.ToString();
            if(playerOneWon)
            {
                Plateaus[0].transform.position = Vector3.Lerp(p1_PlateauStartPos, new Vector3(p1_PlateauStartPos.x, winnerYPos, p1_PlateauStartPos.z), time);
                Plateaus[1].transform.position = Vector3.Lerp(p2_PlateauStartPos, new Vector3(p2_PlateauStartPos.x, loserYPos, p2_PlateauStartPos.z), time);
            }
            else
            {
                Plateaus[0].transform.position = Vector3.Lerp(p1_PlateauStartPos, new Vector3(p1_PlateauStartPos.x, loserYPos, p1_PlateauStartPos.z), time);
                Plateaus[1].transform.position = Vector3.Lerp(p2_PlateauStartPos, new Vector3(p2_PlateauStartPos.x, winnerYPos, p2_PlateauStartPos.z), time);
            }
            
            //playerOne.transform.position = new Vector3(playerOne.transform.position.x, playerOne.transform.position.y + p1_CurrentsScore / 1000, playerOne.transform.position.z);
            //playerTwo.transform.position = new Vector3(playerTwo.transform.position.x, playerTwo.transform.position.y + p2_CurrentsScore / 1000, playerTwo.transform.position.z);

            if (p1_CurrentsScore >= p1_Score)
            {
                p1_CurrentsScore = p1_Score;
                textManger[0].score.text = p1_Score.ToString();
            }
            if (p2_CurrentsScore >= p2_Score)
            {
                p2_CurrentsScore = p2_Score;
                textManger[1].score.text = p2_Score.ToString();
            }
            if(p1_CurrentsScore == p1_Score && p2_CurrentsScore == p2_Score)
            {
                done = true;
            }
            yield return null;
        }

        yield return null;
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