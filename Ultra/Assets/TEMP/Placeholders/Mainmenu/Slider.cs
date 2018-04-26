using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Slider : MonoBehaviour {

    //--------------------------LIST-------------------------------//
    #region Variables
    //--Public--//

    public float minValueX;
    public float maxValueX;

    public string mixerParameter;

    public AudioMixer aMixer;

        //--Private--//

    private bool inTrigger = false;

    private float currentMixerVol;
    private float minAudio = -80.0f;
    private float maxAudio = 20.0f;
    private float incrementX = 1.0f;
    private float currentAudio;

    #endregion

    //-------------------------START - UPDATE--------------------------------//

    // Use this for initialization
    void Start ()
    {
        SetPos();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Activated();
    }


    //------------------------ON-TRIGGER--------------------------//

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
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
                    this.transform.position += Vector3.right * incrementX;
                    ChangeVolume(true);
                }
            }
            else if(Input.GetButtonDown("P1_XButton"))
            {
                //Decrease
                if (this.gameObject.transform.position.x > minValueX)
                {
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
    }

    private void SetPos()
    {
        aMixer.GetFloat(mixerParameter, out currentMixerVol);
        currentAudio = currentMixerVol;

        float currentX = this.transform.position.x;

        //Don't know why i need this....but i do
        float currentY = transform.position.y;
        float currentZ = transform.position.z;


        //get distance of start point of slider and current position
        Vector3 minPos = new Vector3(minValueX, currentY, currentZ);
        Vector3 currPos = new Vector3(currentX, currentY, currentZ);
        float dist = Vector3.Distance(minPos, currPos);

        //get distance of start point of volume and current volume
        Vector3 minPosAudio = new Vector3(minAudio, currentY, currentZ);
        Vector3 currPosAudio = new Vector3(currentAudio, currentY, currentZ);
        float distAudio = Vector3.Distance(minPosAudio, currPosAudio);



        float newCurrentX = (distAudio / 10); 

        while(dist != newCurrentX)
        {
            if(dist < newCurrentX)
            {
                currentX += 1.0f;
                dist += 1.0f;
            }
            else if(dist > newCurrentX)
            {
                currentX -= 1.0f;
                dist -= 1.0f;
            }
        }

        transform.position = new Vector3(currentX, currentY, currentZ);
    }
}


