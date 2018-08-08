using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoScript : MonoBehaviour
{
    public RawImage image;

    public VideoClip videoToPlay;

    private VideoPlayer videoPlayer;
    private VideoSource videoSource;

    private AudioSource audioSource;
    public bool loop;

    public int SceneToLoad;

    public bool loadScene;
    public bool debug = false;

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        StartCoroutine(PlayVideo());
        InputManager.p1_OnKeyPressed += GetInput;
        InputManager.p2_OnKeyPressed += GetInput;
    }

    void GetInput(KeyCode noNeed)
    {
        if (loadScene)
            StartCoroutine(LoadNewScene());
    }
    

    public void IStart()
    {
        StartCoroutine(PlayVideo());
    }

    IEnumerator PlayVideo()
    {

        //Add VideoPlayer to the GameObject
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        //Add AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();

        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        audioSource.Pause();

        //We want to play from video clip not from url

        videoPlayer.source = VideoSource.VideoClip;

        // Vide clip from Url
        //videoPlayer.source = VideoSource.Url;
        //videoPlayer.url = "http://www.quirksmode.org/html5/videos/big_buck_bunny.mp4";


        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //Set video To Play then prepare Audio to prevent Buffering
        videoPlayer.clip = videoToPlay;
        videoPlayer.Prepare();
        videoPlayer.isLooping = loop;

        //Wait until video is prepared
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        Debug.Log("Done Preparing Video");

        //Assign the Texture from Video to RawImage to be displayed
        image.texture = videoPlayer.texture;

        //Play Video
        videoPlayer.Play();

        //Play Sound
        audioSource.Play();

        Debug.Log("Playing Video");
        while (videoPlayer.isPlaying && debug == true)
        {
            Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
            yield return null;
        }

        if(loadScene)
        {
            Debug.Log("Done Playing Video");
            StartCoroutine(LoadNewScene());
        }
    }


    IEnumerator LoadNewScene()
    {
        InputManager.p1_OnKeyPressed -= GetInput;
        InputManager.p2_OnKeyPressed -= GetInput;

        yield return new WaitForSeconds(0.1f);
        AsyncOperation async = SceneManager.LoadSceneAsync(SceneToLoad);
        while (!async.isDone)
        {
            yield return null;
        }
    }
}