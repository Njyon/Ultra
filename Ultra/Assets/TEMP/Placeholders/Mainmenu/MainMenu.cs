using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour {

    //----------------------LIST-----------------------//

    private Buttons buttonsScript;

    private GameObject[] buttons;
    private GameObject textObj;
    private Text text;
    private Resolution currentRes;

    public bool inTrigger;

    private Color resetCol;
    private Material origMat;

    private Color resetCol1;
    private Material origMat1;

    private Animator anim;


    public GameObject buttonOn1;
    public GameObject buttonOn2;
    public GameObject buttonHit1;
    public GameObject buttonHit2;

    //----------------START UPDATE------------------------//

    private void Awake()
    {

    


        origMat = buttonOn1.GetComponent<Renderer>().material;
        resetCol = origMat.color;

        origMat1 = buttonHit1.GetComponent<Renderer>().material;
        resetCol1 = origMat1.color;


    }


    void Start()
    {

        SetResolution();

        anim = buttonHit1.GetComponent<Animator>();

    }


    void Update () {

        HitButton(buttonHit1);
        OnButton(buttonOn1);
        HitButton(buttonHit2);
        OnButton(buttonOn2);

    }


    public void SetResolution()  //called in "Buttons script
    {

        #region Sets text to Current Screen

        Resolution currentScreen = Screen.currentResolution;
        string s_width = currentScreen.width.ToString();
        string s_height = currentScreen.height.ToString();
        textObj = GameObject.Find("ResolutionText");
        text = textObj.GetComponent<Text>();
        text.text = s_width + " x " + s_height;

        #endregion

        #region Makes a list of all resolutions

        List<Resolution> resolutionsList = new List<Resolution>();
        List<string> resString = new List<string>();

         //------------Adds current resolution to list------------//
        resolutionsList.Add(currentScreen);
        resString.Add(s_width + "x" + s_height);

        //---------Adds all available resolutions to list----------//
        Resolution[] resolutions = Screen.resolutions;
        foreach (Resolution res in resolutions)
        {
            resolutionsList.Add(res);
            resString.Add(res.width.ToString() + "x" + res.height.ToString());
        }

        #endregion


        //int i = 0;

        string newRes = resString[0];
        Debug.Log("newRes: " + newRes + "  reslist1: " + resolutionsList[0]);

        //Text text = textObj.GetComponent<Text>();
        //text.text = (resolutions[1].width.ToString() + "x" + resolutions[1].height.ToString());

    }






    //----------------Buttons-----------------------//

    private void ChangeCol(Color newColor, GameObject obj)
    {
        Material rend = obj.GetComponent<Renderer>().material;
        rend.color = newColor;
    }

    private void HitButton(GameObject currentButton)
    {
        if (inTrigger == true && currentButton)
        {
            if (Input.GetButtonDown("P1_XButton"))
            {
                anim.SetBool("onButtonPressed", true);
                anim.Play("anim_buttonWiggle");
                ChangeCol(Color.green, currentButton);

            }
            else if (Input.GetButtonUp("P1_XButton"))
            {
                anim.SetBool("onButtonPressed", false);
                ChangeCol(resetCol1, currentButton);
            }
        }
    }

    private void OnButton(GameObject currentButton)
    {
        if (inTrigger == true && currentButton)
        {
            ChangeCol(Color.red, currentButton);
        }
        else
        {
            ChangeCol(resetCol, currentButton);
        }
    }




}
