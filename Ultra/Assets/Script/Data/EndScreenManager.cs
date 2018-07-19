﻿using System.Collections;
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
        bool done = false;
        int p1_Score = 5000;//PlayerDataManager.playerOne.Score;
        int p2_Score = 3776;//PlayerDataManager.playerTwo.Score;
        float p1_CurrentsScore = 0;
        float p2_CurrentsScore = 0;

        Vector3 p1_PlateauStartPos = Plateaus[0].transform.position;
        Vector3 p2_PlateauStartPos = Plateaus[1].transform.position;

        //Vector3 p1_PlateauEndPos = new Vector3(p1_PlateauStartPos.x, p1_PlateauStartPos.y += p1_Score / 100);
        //Vector3 p2_PlateauEndPos = new Vector3(p2_PlateauStartPos.x, p2_PlateauStartPos.y += p1_Score / 100);

        float speed = 1000f;

        yield return new WaitForSeconds(1f);
        
        while(!done)
        {
            Debug.Log(p1_CurrentsScore);
            // Count Score Up
            p1_CurrentsScore += Time.deltaTime * speed;
            p2_CurrentsScore += Time.deltaTime * speed;

            int p1_IntScore = (int)p1_CurrentsScore;
            int p2_IntScore = (int)p2_CurrentsScore;

            // Display new Score
            textManger[0].score.text = p1_IntScore.ToString();
            textManger[1].score.text = p2_IntScore.ToString();

            Plateaus[0].transform.position = new Vector3(p1_PlateauStartPos.x, p1_PlateauStartPos.y + p1_CurrentsScore / 1000, p1_PlateauStartPos.z);
            Plateaus[1].transform.position = new Vector3(p2_PlateauStartPos.x, p2_PlateauStartPos.y + p2_CurrentsScore / 1000, p2_PlateauStartPos.z);

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
                Plateaus[0].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                Plateaus[1].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
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