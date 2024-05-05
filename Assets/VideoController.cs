using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    public List<VideoClip> Clips;
    public int clipIndex = 0;
    public Image ArrowRight;
    public Image ArrowLeft;

    public Sprite ArrowEnabled;
    public Sprite ArrowDisabled;
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(WaitToPlayVideo());
    }

    void OnDisable()
    {
        videoPlayer.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator WaitToPlayVideo()
    {
        yield return new WaitForSeconds(1.5f);
        videoPlayer.Play();
    }

    public void SetClipIndex(bool isRight)
    {
        videoPlayer.Stop();
        if (clipIndex == 0 && isRight)
        {
            clipIndex = 1;
            ArrowRight.sprite = ArrowDisabled;
            ArrowLeft.sprite = ArrowEnabled;
        }
        else if (clipIndex == 1 && !isRight)
        {
            clipIndex = 0;
            ArrowRight.sprite = ArrowEnabled;
            ArrowLeft.sprite = ArrowDisabled;
        }
        videoPlayer.clip = Clips[clipIndex];
        videoPlayer.Play();
    }

    public void ChangeVideoState()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
        else if (videoPlayer.isPaused)
        {
            videoPlayer.Play();
        }
    }
}
