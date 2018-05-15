using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class MainMenu : MonoBehaviour {

    //Slider
    public AudioMixer aMixer;

    GameObject sfxSlider;
    GameObject musicSlider;
    GameObject masterSlider;

    //float incrementX = 1.0f;
    float currentMixerVol;
    float minAudio = -80.0f;
    float maxAudio = 20.0f;
    float currentAudio;
    string mixerParameter;

    //Resolution
    [HideInInspector] public Resolution[] resolutions;
    private Resolution currentRes;
    private GameObject textObj;
    private Text text;
    private int index = 0;



    ////////////////////////////////////////////////////////
    //////////////          Start          ////////////////
    //////////////////////////////////////////////////////

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

    ////////////////////////////////////////////////////////
    ////////////          Functions          //////////////
    //////////////////////////////////////////////////////


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

    public void VolumeIncrementBase(GameObject currentSlider)
    {
        ChangeVolume(10.0f, currentSlider);
    }

    public void VolumeIncrementSpecial(GameObject currentSlider)
    {
        ChangeVolume(20.0f, currentSlider);
    }

    public void ChangeVolume(float incrementX, GameObject currentSlider)
    {
        Slider slider = currentSlider.gameObject.GetComponent<Slider>();
        aMixer.GetFloat(slider.mixerParameter, out currentMixerVol);
        currentAudio = currentMixerVol;
        currentAudio = Mathf.Round(currentAudio * 10f) / 10f;
        Debug.Log(currentAudio + " : " + currentMixerVol);
        if (slider.increased == true)
        {
            
            currentAudio = currentAudio + incrementX;

            if (currentAudio > maxAudio)
            {
                currentAudio = maxAudio;
            }
        }
        else if (slider.increase == false)
        {
            currentAudio = currentAudio - incrementX;

            if (currentAudio < minAudio)
            {
                currentAudio = minAudio;
            }
        }
        Debug.Log(currentAudio + " : " + mixerParameter);
        currentMixerVol = currentAudio;
        aMixer.SetFloat(slider.mixerParameter, currentMixerVol);
    }



    private void SetDefaultSliderPos(string mixerParameter, GameObject currentSlider)
    {
        if (currentSlider == null)
            return;

        aMixer.GetFloat(mixerParameter, out currentMixerVol);
        currentAudio = currentMixerVol;

        float currentX = currentSlider.transform.position.x;
        float minValueX = currentSlider.transform.position.x - 5;

        float distSlider = DistanceCalc(minValueX, currentX, currentSlider);
        float distAudio = DistanceCalc(minAudio, currentAudio, currentSlider);

        float newCurrentX = (distAudio / 10);

        if (distSlider != newCurrentX)
        {
            if (distSlider < newCurrentX)
            {
                currentX += newCurrentX - distSlider;
            }
            else if (distSlider > newCurrentX)
            {
                currentX -= distSlider - newCurrentX;
            }
        }
        currentSlider.transform.position = new Vector3(currentX, currentSlider.transform.position.y, currentSlider.transform.position.z);
    }


    float DistanceCalc(float min, float current, GameObject currentObj)
    {
        Vector3 minPosSlider = new Vector3(min, 0, 0);
        Vector3 currPosSlider = new Vector3(current, 0, 0);
        float distance = Vector3.Distance(minPosSlider, currPosSlider);


        return distance;
    }
    #endregion

}
