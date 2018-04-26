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

    private int index = 0;

    GameObject buttonOn;
    GameObject buttonHit;

    [HideInInspector] public Resolution[] resolutions;


    //----------------START UPDATE------------------------//

    private void Awake()
    {

    }


    void Start()
    {

        Resolution currentScreen = Screen.currentResolution;
        string s_width = currentScreen.width.ToString();
        string s_height = currentScreen.height.ToString();
        textObj = GameObject.Find("ResolutionText");
        text = textObj.GetComponent<Text>();
        text.text = s_width + " x " + s_height;
        resolutions = Screen.resolutions;

        SetResolution();
    }

    //----------------Functions-----------------------//

    public void SetResolution() 
    {
        for(int i = 0; i < resolutions.Length; i++)
        {
            if(resolutions[i].height == currentRes.height && resolutions[i].width == currentRes.width)
            {
                index = i;
            }
        }
        
        text.text = (resolutions[index].width.ToString() + "x" + resolutions[index].height.ToString());

    }

    public void ResIndexIncrease()
    {
        if (index + 1 >= resolutions.Length)
            return;

        index++;
        text.text = (resolutions[index].width.ToString() + "x" + resolutions[index].height.ToString());
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, true);
        Screen.fullScreen = true;
    }

    public void ResIndexDecrease()
    {
        if (index - 1 < 0)
            return;

        index--;
        text.text = (resolutions[index].width.ToString() + "x" + resolutions[index].height.ToString());
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, true);
        Screen.fullScreen = true;
    }

}
