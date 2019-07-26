using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameController : MonoBehaviour {
    //kinect left => 2-1
    //kinect Right => 1-2
    //kinect Wheel => 3
    public enum VideoState {
        NULL,
        Standby,
        LEFT,
        RIGHT,
        Wheel,
    }

    public RawImage idleImage;
    public RawImage StandbyImage;
    public VideoPlayer movie;

    public List<VideoClip> standbyVideoList = new List<VideoClip> ();
    public List<VideoClip> RlVideoList = new List<VideoClip> ();
    public List<VideoClip> RWVideoList = new List<VideoClip> ();
    public List<VideoClip> LRVideoList = new List<VideoClip> ();
    public List<VideoClip> LWVideoList = new List<VideoClip> ();
    public List<VideoClip> WRVideoList = new List<VideoClip> ();
    public List<VideoClip> WLVideoList = new List<VideoClip> ();

    private AvatarGestureListener gestureListener;
    private bool IsAniming = false;
    private int currentState = -1;
    private bool isUserLost = true;
    private bool IsWheeling = false;
    private VideoState videoState = VideoState.NULL;

    void Start () {
        InitPage ();
        RefreshPage ();
        IsAniming = false;
        IsWheeling = false;
    }

    void Update () {
        if (IsAniming)
            return;

        if (Input.GetKeyDown (KeyCode.LeftArrow)) {
            ChangeLeft ();
        } else if (Input.GetKeyDown (KeyCode.RightArrow)) {
            ChangeRight ();
        } else if (Input.GetKeyDown (KeyCode.UpArrow)) {
            ChangeWheel ();
        } else if (Input.GetKeyDown (KeyCode.DownArrow)) {
            BeginImage ();
        }

    }

    private void InitPage () {
        gestureListener = AvatarGestureListener.Instance;
        gestureListener.delegateUserDetected = ExectueUserDetected;
        gestureListener.delegateUserLost = ExectueUserLost;
        gestureListener.delegateGestureCompleted = ExectueGestureComplate;
        gestureListener.delegateGestureCancel = ExectueGestureCancel;
        movie.loopPointReached += OnLoopPointReached;
    }

    private void RefreshPage () {
        IsWheeling = false;
        isUserLost = true;
        if (videoState == VideoState.NULL) {
            idleImage.enabled = true;
            idleImage.DOFade (1, 0);
        } else {
            IsAniming = true;
            StandbyImage.enabled = true;
            StandbyImage.DOFade (1, 0);
            idleImage.enabled = true;
            idleImage.DOFade (0, 0);
            idleImage.DOFade (1, 1.0f).onComplete = () => {
                IsAniming = false;
                videoState = VideoState.NULL;
            };
        }
    }

    private void ExectueUserDetected () {
        BeginImage ();
    }
    private void ExectueUserLost () {
        isUserLost = true;
        if (!IsAniming) {
            RefreshPage ();
        }
    }
    private void ExectueGestureComplate (KinectGestures.Gestures gestures) {
        if (IsAniming)
            return;
        if (gestures == KinectGestures.Gestures.SwipeLeft) {
            ChangeLeft ();
        } else if (gestures == KinectGestures.Gestures.SwipeRight) {
            ChangeRight ();
        } else if (gestures == KinectGestures.Gestures.Wheel && !IsWheeling) {
            ChangeWheel ();
        }
    }
    private void ExectueGestureCancel (KinectGestures.Gestures gestures) {
        if (gestures == KinectGestures.Gestures.Wheel && IsWheeling) {
            BeginImage ();
        }
        Debug.Log ("Gesture Cancel " + gestures.ToString ());
    }

    private void CheckUserLostState () {
        if (isUserLost) {
            RefreshPage ();
        }
    }

    private void BeginImage () {
        isUserLost = false;
        IsAniming = true;
        if (videoState == VideoState.NULL) {
            StandbyImage.DOFade (1, 0);
            StandbyImage.enabled = true;
            idleImage.DOFade (0, 1).onComplete = () => {
                IsAniming = false;
            };
        } else {
            StandbyImage.DOFade (0, 0);
            StandbyImage.enabled = true;
            StandbyImage.DOFade (1, 1).onComplete = () => {
                IsAniming = false;
            };
        }
        videoState = VideoState.Standby;
    }

    private void ChangeLeft () {
        Debug.LogWarning ("Left");
        if (videoState == VideoState.LEFT) {
            return;
        }
        Debug.LogWarning ("Left Anim");
        IsWheeling = false;
        if (videoState == VideoState.Standby) {
            PlayVideo (standbyVideoList[0]);
            StartCoroutine (closeStandBy ());
        } else if (videoState == VideoState.RIGHT) {
            PlayVideo (RlVideoList[0]);
        } else if (videoState == VideoState.Wheel) {
            PlayVideo (WLVideoList[0]);
        }
        videoState = VideoState.LEFT;
    }
    private void ChangeRight () {
        Debug.LogWarning ("Right");
        if (videoState == VideoState.RIGHT) {
            return;
        }
        Debug.LogWarning ("Right Anim");
        IsWheeling = false;
        if (videoState == VideoState.Standby) {
            PlayVideo  (standbyVideoList[1]);
            StartCoroutine (closeStandBy ());
        } else if (videoState == VideoState.LEFT) {
            PlayVideo (LRVideoList[0]);
        } else if (videoState == VideoState.Wheel) {
            PlayVideo (WRVideoList[0]);
        }
        videoState = VideoState.RIGHT;
    }
    private void ChangeWheel () {
        Debug.LogWarning ("Wheel");
        IsWheeling = true;
        if (videoState == VideoState.Wheel) {
            return;
        }
        Debug.LogWarning ("Wheel Anim");
        if (videoState == VideoState.Standby) {
            PlayVideo (standbyVideoList[2]);
            StartCoroutine (closeStandBy ());
        } else if (videoState == VideoState.LEFT) {
            PlayVideo (LWVideoList[0]);
        } else if (videoState == VideoState.RIGHT) {
            PlayVideo (RWVideoList[0]);
        }
        videoState = VideoState.Wheel;
    }

    private void PlayVideo (VideoClip video) {
        movie.clip = video;
        movie.Play ();
    }

    IEnumerator closeStandBy () {
        yield return new WaitForSeconds (0.5f);
        StandbyImage.enabled = false;
    }

    void OnLoopPointReached (VideoPlayer source) {
        IsAniming = false;
        Debug.LogWarning ("Video Finish~~~");
    }
}