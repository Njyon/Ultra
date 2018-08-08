using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventsStartTimer : MonoBehaviour {

    public void One()
    {
        Fabric.EventManager.Instance.PostEvent("Announcer1");
    }
    
    public void Two()
    {
        Fabric.EventManager.Instance.PostEvent("Announcer2");
    }
    
    public void Three()
    {
        Fabric.EventManager.Instance.PostEvent("Announcer3");
    }
    
    public void Fight()
    {
        Fabric.EventManager.Instance.PostEvent("AnnouncerFight");
    }
}
