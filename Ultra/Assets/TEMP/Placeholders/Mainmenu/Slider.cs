using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Slider : MonoBehaviour {

    //--------------------------LIST-------------------------------//

        //--Public--//

    public float minValueX;
    public float maxValueX;

    public AudioMixer aMixer;

    public string mixerParameter;

        //--Private--//

    private bool inTrigger = false;

    private Vector3 minPos;
    private Vector3 maxPos;


    private float currentVal;
    private float updateVal;
    private float currentY;
    private float currentZ;
    private float percentVolume;
    private float currentMixerVol;
    private float minAudio = -80.0f;
    private float maxAudio = 20.0f;
    private float incrementX = 1.0f;
    private float currentAudio;
    private float startPos;


    //-------------------------START - UPDATE--------------------------------//

    // Use this for initialization
    void Start ()
    {
        aMixer.GetFloat(mixerParameter, out currentMixerVol);
        currentAudio = currentMixerVol;
        //percentVolume = PerectageCalc(minAudio, maxAudio, currentAudio);
        currentY = transform.position.y;
        currentZ = transform.position.z;
        //currentVal = FindValue(percentVolume, maxValueX, minValueX);
        //transform.position = new Vector3(currentVal, currentY, currentZ);
        //Debug.Log(this.gameObject.transform.position.x + " : " + currentMixerVol + " -- " + "current val: " + currentVal);



        minPos = new Vector3(minValueX, currentY, currentZ);
        maxPos = new Vector3(maxValueX, currentY, currentZ);
        float dist = Vector3.Distance(minPos, maxPos);
        Debug.Log(dist);


    }
	
	// Update is called once per frame
	void Update ()
    {
        Activated();
    }


    //------------------------ON-TRIGGER--------------------------//

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "player")
        {
            inTrigger = true;
        }
        
                                                                                //how to acces a specific collider that collides with this trigger?
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            inTrigger = false;
        }
     
    }


    // -------------------FUNCTIONS---------------------------------//


    private void Activated()
    {

        if (inTrigger == true)
        {
            if (Input.GetButtonDown("P1_YButton") )
            {
                //Increase
                if (this.gameObject.transform.position.x < maxValueX)
                {

                    Debug.Log("maxvaluex: " + maxValueX);
                    this.transform.position += Vector3.right * incrementX;
                    ChangeVolume(true);
                }
            }
            else if(Input.GetButtonDown("P1_XButton"))
            {
                //Decrease
                if (this.gameObject.transform.position.x > minValueX)
                {
                    Debug.Log("minvaluex: " + minValueX);
                    this.transform.position += Vector3.left * incrementX;
                    ChangeVolume(false);
                }
            }

            
        }

    } 

    private void ChangeVolume(bool add)
    {
        currentAudio = Mathf.Round(currentAudio * 10f) / 10f;                               //Rounds to a whole

        float incrementer = ((minValueX - maxValueX) / (minAudio - maxAudio)) * 100;        // calculates incrementation steps 

        if (add == true)
        {
            //Increase
            currentAudio = currentAudio + incrementer;

            if (currentAudio > maxAudio)
            {
                currentAudio = maxAudio;
            }

        }
        else if (add == false)
        {
            //Decrease
            currentAudio = currentAudio - incrementer;

            if (currentAudio < minAudio)
            {
                currentAudio = minAudio;
            }
        }

        currentMixerVol = currentAudio;
        aMixer.SetFloat(mixerParameter, currentMixerVol);
        Debug.Log("currentMixerVol: " + currentMixerVol + " -- " + "currentAudio: " + currentAudio + " -- " + "current pos x: " + this.gameObject.transform.position.x);
    }

    private float PerectageCalc(float min, float max, float currentPos)
    {
        float y = (((max - min) - (max - currentPos)) / (max - min));

        return currentPos;
    }

    private float FindValue(float currentPercent, float maxValue, float minValue)
    {

        float valX = ((maxValue * currentPercent) - (minValue * currentPercent) + (100 * minValue)) / 100;
        //float valX = (currentPercent / 100) * maxValue;
        Debug.Log("valx: " + valX);
        return valX; 
    }

}


