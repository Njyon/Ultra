using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UnloadTest : MonoBehaviour
{
    private void Start()
    {
        SceneManager.UnloadSceneAsync("TEST");
    }

}