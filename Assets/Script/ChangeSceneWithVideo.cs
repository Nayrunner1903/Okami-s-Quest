
//ChangeSceneWithVideo

using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using GamingIsLove.Makinom.Schematics;
using GamingIsLove.Makinom;


public class ChangeSceneWithVideo : MonoBehaviour
{
    public VideoClip cutsceneVideo;
    public string nextSceneName;
    public RenderTexture targetTexture;

    private VideoPlayer videoPlayer;

    public GameObject sceneChanger; 

    void Start()
    {
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        videoPlayer.clip = cutsceneVideo;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = targetTexture;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            sceneChanger.SetActive(true);
            //LoadNextScene();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        sceneChanger.SetActive(true);
        //LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}

