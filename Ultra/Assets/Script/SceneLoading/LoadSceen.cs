using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceen : MonoBehaviour
{
    
	void Start ()
    {
        StartCoroutine(LoadSceenDelayed());
	}

    IEnumerator LoadSceenDelayed()
    {
        yield return new WaitForSeconds(2f);
        AsyncOperation async = SceneManager.LoadSceneAsync(2);
        while (!async.isDone)
        {
            yield return null;
        }
        yield return null;
    }

}
