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
    [Header("HighestPoint")]
    public Animator anim;
    public Text winnerText;

    GameObject playerOne;
    GameObject playerTwo;

    private void Start()
    {
        //PlayerOne
        textManger[0].score.text = "Score: 0"; //+ PlayerDataManager.playerOne.Score.ToString();
        //textManger[0].maxCombo.text = "MaxCombo: " + PlayerDataManager.playerOne.HighestCombo.ToString();
        //textManger[0].dodges.text = "Dodges: " + PlayerDataManager.playerOne.AmountOfDodges.ToString();
        //textManger[0].maxMultiplier.text = "MaxMultiplier: " + PlayerDataManager.playerOne.HighestMultiplier.ToString();
        //textManger[0].bounces.text = "Bounces: " + PlayerDataManager.playerOne.Bounces.ToString();

        //PlayerTwo
        textManger[1].score.text = "Score: 0"; // + PlayerDataManager.playerTwo.Score.ToString();
        //textManger[1].maxCombo.text = "MaxCombo: " + PlayerDataManager.playerTwo.HighestCombo.ToString();
        //textManger[1].dodges.text = "Dodges: " + PlayerDataManager.playerTwo.AmountOfDodges.ToString();
        //textManger[1].maxMultiplier.text = "MaxMultiplier: " + PlayerDataManager.playerTwo.HighestMultiplier.ToString();
        //textManger[1].bounces.text = "Bounces: " + PlayerDataManager.playerTwo.Bounces.ToString();
        

        Invoke("EnableInput", 1f);
        Init();
        StartCoroutine(CountPoints(PlayerDataManager.playerOne.Score, PlayerDataManager.playerTwo.Score));
    }

    void Init()
    {
        //Spawn PlayerOne and Color his Character
        playerOne = Instantiate(character, playerSpawns[0].transform.position, Quaternion.identity);
        playerOne.GetComponent<MyCharacter>().clothRenderer.materials[0].SetColor("_EmissionColor", PlayerInfoManager.playerOne.color);
        playerOne.GetComponent<MyCharacter>().clothRenderer.materials[1].SetColor("_EmissionColor", PlayerInfoManager.playerOne.color);

        //Spawn PlayerTwo and Color his Character
        playerTwo = Instantiate(character, playerSpawns[1].transform.position, Quaternion.Euler(0,180,0));
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
                return (maxHight / p1_Score * p2_Score);
            case false:
                return (maxHight / p2_Score * p1_Score);
        }
        return 0;
    }

    float GetDodgeHight()
    {
        return 0f;
    }

    int GetWinnerScore(int p1_Score, int p2_Score)
    {
        if (p1_Score > p2_Score)
            return p1_Score;
        else
            return p2_Score;
    }   
    int GetDodgeScore(int Dodges)
    {
        return Dodges * 1000;
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
    IEnumerator RisePlateau(float p1_scoreToAdd, float p2_scoreToAdd)
    {
        float time = 2f;
        float divideTime = time;

        float p1_HightToAdd =  p1_scoreToAdd / scorePerHight;
        float p2_HightToAdd =  p2_scoreToAdd / scorePerHight;

        float p1_OldScore = p1_CurrentsScore;
        float p2_OldScore = p2_CurrentsScore;

        Vector3 p1_PlateauPos = Plateaus[0].transform.position;
        Vector3 p2_PlateauPos = Plateaus[1].transform.position;

        Vector3 p1_PlateauEndPos = new Vector3(Plateaus[0].transform.position.x, Plateaus[0].transform.position.y + p1_HightToAdd, Plateaus[0].transform.position.z);
        Vector3 p2_PlateauENdPos = new Vector3(Plateaus[1].transform.position.x, Plateaus[1].transform.position.y + p2_HightToAdd, Plateaus[1].transform.position.z);

        while (time > 0)
        {
            // Count Score Up
            p1_CurrentsScore = Mathf.Lerp(p1_scoreToAdd + p1_OldScore, p1_OldScore, time / divideTime);
            p2_CurrentsScore = Mathf.Lerp(p2_scoreToAdd + p2_OldScore, p2_OldScore, time / divideTime);
            // Convert To Int to Display
            int p1_IntScore = (int)p1_CurrentsScore;
            int p2_IntScore = (int)p2_CurrentsScore;
            // Display new Score
            textManger[0].score.text = "Score: " + p1_IntScore.ToString();
            textManger[1].score.text = "Score: " + p2_IntScore.ToString();

            //Lerp Plateau to new Position
            Plateaus[0].transform.position = Vector3.Lerp(p1_PlateauEndPos, p1_PlateauPos, time / divideTime);
            Plateaus[1].transform.position = Vector3.Lerp(p2_PlateauENdPos, p2_PlateauPos, time / divideTime);
            
            time -= Time.deltaTime;

            yield return null;
        }

        // Count Score Up
        p1_CurrentsScore = p1_scoreToAdd + p1_OldScore;
        p2_CurrentsScore = p2_scoreToAdd + p2_OldScore;

        int p1_intScore = (int)p1_CurrentsScore;
        int p2_intScore = (int)p2_CurrentsScore;

        // Display new Score
        textManger[0].score.text = "Score: " + p1_intScore.ToString();
        textManger[1].score.text = "Score: " + p2_intScore.ToString();

        yield return null;
    }
    float p1_CurrentsScore = 0;
    float p2_CurrentsScore = 0;
    float scorePerHight;
    IEnumerator CountPoints(int p1_Score, int p2_Score)
    {
        yield return new WaitForSeconds(1.5f);
        bool playerOneWon = PlayerOneWins(p1_Score, p2_Score);

        int winnerScore = GetWinnerScore(p1_Score, p2_Score);
        scorePerHight = winnerScore / maxScoreHight.position.y;

        Debug.Log(maxScoreHight.position.y);

        int p1_Dodge_Score = GetDodgeScore(PlayerDataManager.playerOne.AmountOfDodges);
        int p2_Dodge_Score = GetDodgeScore(PlayerDataManager.playerTwo.AmountOfDodges);

        int p1_RestScore = p1_Score - p1_Dodge_Score;
        int p2_RestScore = p2_Score - p2_Dodge_Score;

        StartCoroutine(RisePlateau(p1_Dodge_Score, p2_Dodge_Score));
        yield return new WaitForSeconds(3f);

        StartCoroutine(RisePlateau(p1_RestScore * 0.35f, p2_RestScore * 0.35f));
        yield return new WaitForSeconds(3f);

        StartCoroutine(RisePlateau(p1_RestScore * 0.25f, p2_RestScore * 0.25f));
        yield return new WaitForSeconds(3f);

        StartCoroutine(RisePlateau(p1_RestScore * 0.50f, p2_RestScore * 0.50f));

        textManger[0].score.text = "Score: " + PlayerDataManager.playerOne.Score.ToString();
        textManger[1].score.text = "Score: " + PlayerDataManager.playerTwo.Score.ToString();

        if(playerOneWon)
        {
            playerOne.GetComponent<MyCharacter>().animator.SetBool("Won", true);
            playerTwo.GetComponent<MyCharacter>().animator.SetBool("Lose", true);
        }
        else
        {
            playerOne.GetComponent<MyCharacter>().animator.SetBool("Lose", true);
            playerTwo.GetComponent<MyCharacter>().animator.SetBool("Won", true);
        }

        yield return null;
    }

    IEnumerator TEST()
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

        while (!done)
        {
            time += Time.deltaTime / speed;
            // Count Score Up
            p1_CurrentsScore = Mathf.Lerp(0, p1_Score, time);
            p2_CurrentsScore = Mathf.Lerp(0, p2_Score, time);

            int p1_IntScore = (int)p1_CurrentsScore;
            int p2_IntScore = (int)p2_CurrentsScore;

            // Display new Score
            textManger[0].score.text = "Score: " + p1_IntScore.ToString();
            textManger[1].score.text = "Score: " + p2_IntScore.ToString();

            if (playerOneWon)
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
            if (p1_CurrentsScore == p1_Score && p2_CurrentsScore == p2_Score)
            {
                done = true;
            }
            yield return null;
        }

        if (playerOneWon)
        {
            winnerText.text = "Player One Wins";
        }
        else
        {
            winnerText.text = "Player Two Wins";
        }

        Invoke("DisplayWinner", 0.5f);
        yield return null;
    }

    void DisplayWinner()
    {
        anim.SetBool("Play", true);
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