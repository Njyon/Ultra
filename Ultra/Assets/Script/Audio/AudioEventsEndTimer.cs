using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventsEndTimer : MonoBehaviour {

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
	
	public void Four()
	{
		Fabric.EventManager.Instance.PostEvent("Announcer4");
	}
	
	public void Five()
	{
		Fabric.EventManager.Instance.PostEvent("Announcer5");
	}
    
	public void Game()
	{
		Fabric.EventManager.Instance.PostEvent("AnnouncerGame");
	}
}
