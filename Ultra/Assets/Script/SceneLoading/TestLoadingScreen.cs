using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TestLoadingScreen : MonoBehaviour
{
    private void Start()
    {
        Invoke("Load", 1);
    }

    void Load()
    {
        LoadingScreenManager.LoadScene(5);
    }
}