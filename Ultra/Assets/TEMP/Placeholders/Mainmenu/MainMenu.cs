using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


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

    //______Slider_________//

    public AudioMixer aMixer;

    GameObject sfxSlider;
    GameObject musicSlider;
    GameObject masterSlider;

    float incrementX = 1.0f;
    float currentMixerVol;
    float minAudio = -80.0f;
    float maxAudio = 20.0f;
    float currentAudio;

    [HideInInspector] public Resolution[] resolutions;


    //----------------START UPDATE------------------------//

    private void Awake()
    {

    }


    void Start()
    {

        #region Audio Slider

        sfxSlider = GameObject.Find("SfxSlider");
        musicSlider = GameObject.Find("MusicSlider");
        masterSlider = GameObject.Find("MasterSlider");

        SetDefaultSliderPos("sfx", sfxSlider);
        SetDefaultSliderPos("music", musicSlider);
        SetDefaultSliderPos("master", masterSlider);

        #endregion

        #region Resolution
        GetResolution();
        SetResolution();
        #endregion

    }

    //----------------Functions-----------------------//



    #region Resolution
    public void GetResolution()
    {
        Resolution currentScreen = Screen.currentResolution;
        string s_width = currentScreen.width.ToString();
        string s_height = currentScreen.height.ToString();
        textObj = GameObject.Find("ResolutionText");
        text = textObj.GetComponent<Text>();
        text.text = s_width + " x " + s_height;
        resolutions = Screen.resolutions;
    }

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
    #endregion

    #region Audio Sliders

    public void VolumeIncrementBase()
    {

    }

    public void VolumeIncrementSpecial()
    {

    }

    public void ChangeVolume(bool add, float incrementX, string mixerParameter)
    {
        //incrementX = (maxAudio - minAudio) / 10;

        if (add == true)
        {
            currentAudio = currentAudio + incrementX;

            if (currentAudio > maxAudio)
            {
                currentAudio = maxAudio;
            }
        }
        else if (add == false)
        {
            currentAudio = currentAudio - incrementX;

            if (currentAudio < minAudio)
            {
                currentAudio = minAudio;
            }
        }

        currentMixerVol = currentAudio;
        aMixer.SetFloat(mixerParameter, currentMixerVol);
        Debug.Log("currentslidervol: " + +currentMixerVol);
    }



    private void SetDefaultSliderPos(string mixerParameter, GameObject currentSlider)
    {
        aMixer.GetFloat(mixerParameter, out currentMixerVol);
        currentAudio = currentMixerVol;

        float currentX = currentSlider.transform.position.x;
        float currentY = currentSlider.transform.position.y;
        float currentZ = currentSlider.transform.position.z;

        float minValueX = currentX - 5;

        Vector3 minPos = new Vector3(minValueX, currentY, currentZ);
        Vector3 currPos = new Vector3(currentX, currentY, currentZ);
        float dist = Vector3.Distance(minPos, currPos);

        Vector3 minPosAudio = new Vector3(minAudio, currentY, currentZ);
        Vector3 currPosAudio = new Vector3(currentAudio, currentY, currentZ);
        float distAudio = Vector3.Distance(minPosAudio, currPosAudio);

        float newCurrentX = (distAudio / 10);

        while (dist != newCurrentX)
        {
            if (dist < newCurrentX)
            {
                currentX += 1.0f;
                dist += 1.0f;
            }
            else if (dist > newCurrentX)
            {
                currentX -= 1.0f;
                dist -= 1.0f;
            }
        }

        currentSlider.transform.position = new Vector3(currentX, currentY, currentZ);


    }

    #endregion

}
