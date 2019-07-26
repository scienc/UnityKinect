using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public class MovieController : MonoBehaviour {
    public VideoPlayer movie;
    public VideoClip blackVideo;
    public VideoClip greyVideo;
    public RawImage rawImage;

    private int currentIndex = -1;
    private bool IsPlayAnim = false;
    void Start () {
        // if (this.gameObject.GetComponent<Renderer> () != null)
        //     this.gameObject.GetComponent<Renderer> ().material.mainTexture = movie;
        // movie.Play ();
        // movie.loop = true;
        movie.loopPointReached += OnLoopPointReached;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update () {
        if (Input.GetKeyDown (KeyCode.LeftArrow)) {
            ChangeVideo ();
        }
    }

    private void ChangeVideo () {
        if (IsPlayAnim)
            return;
        IsPlayAnim = true;
        if (currentIndex == -1) {
            movie.clip = blackVideo;
            movie.time = 0;
            movie.Prepare ();
            rawImage.DOFade (0, 1.0f).onComplete = () => {
                currentIndex = 1;
                IsPlayAnim = false;
                movie.Play ();
                rawImage.enabled = false;
            };
        } else if (currentIndex == 1) {
            movie.clip = greyVideo;
            currentIndex = 2;
            movie.Play ();
        } else if (currentIndex == 2) {
            movie.clip = blackVideo;
            currentIndex = 1;
            movie.Play ();
        }
    }

    void OnLoopPointReached (VideoPlayer source) {
        IsPlayAnim = false;
        Debug.LogWarning ("Video Finish~~~");
    }
}