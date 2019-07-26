using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {
    public Transform root;
    public List<RawImage> showImage;
    public RawImage idleImage;
    public RawImage waveImage;
    public float showTime;
    private AvatarGestureListener gestureListener;

    private bool IsAniming = false;
    private int currentImageIndex = -1;

    private bool isUserLost = true;

    private RawImage currentImage = null;

    private int maxIndex = 6;

    private bool IsWheeling = false;

    void Start () {
        InitPage ();
        RefreshPage ();
        IsAniming = false;
        IsWheeling = false;
    }

    private void InitPage () {
        gestureListener = AvatarGestureListener.Instance;
        gestureListener.delegateUserDetected = ExectueUserDetected;
        gestureListener.delegateUserLost = ExectueUserLost;
        gestureListener.delegateGestureCompleted = ExectueGestureComplate;
        gestureListener.delegateGestureCancel = ExectueGestureCancel;
    }
    private void RefreshPage () {
        IsWheeling = false;
        isUserLost = true;
        if (currentImage != null) {
            IsAniming = true;
            idleImage.DOFade (1, 0);
            idleImage.rectTransform.SetSiblingIndex (maxIndex - 1);
            currentImage.DOFade (0, 1.0f).onComplete = () => {
                IsAniming = false;
                idleImage.rectTransform.SetAsLastSibling ();
                currentImage = idleImage;
            };
        } else {
            idleImage.DOFade (1, 0);
            idleImage.rectTransform.SetAsLastSibling ();
        }
        currentImage = idleImage;
        currentImageIndex = -1;
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
        currentImageIndex = 0;
        IsWheeling = false;
        showImage[currentImageIndex].rectTransform.SetSiblingIndex (maxIndex - 1);
        showImage[currentImageIndex].DOFade (1, 0);
        currentImage.DOFade (0, 1.0f).onComplete = () => {
            IsAniming = false;
            currentImage.rectTransform.SetAsFirstSibling ();
            showImage[currentImageIndex].rectTransform.SetAsLastSibling ();
            currentImage = showImage[currentImageIndex];
            CheckUserLostState ();
        };
    }

    private void ChangeLeft () {
        Debug.LogWarning ("Left");
        IsWheeling = false;
        IsAniming = true;
        currentImageIndex -= 1;
        if (currentImageIndex < 0) {
            currentImageIndex = showImage.Count - 1;
        }
        showImage[currentImageIndex].rectTransform.SetSiblingIndex (maxIndex - 1);
        showImage[currentImageIndex].DOFade (1, 0);
        currentImage.DOFade (0, 1.0f).onComplete = () => {
            IsAniming = false;
            currentImage.rectTransform.SetAsFirstSibling ();
            showImage[currentImageIndex].rectTransform.SetAsLastSibling ();
            currentImage = showImage[currentImageIndex];
            CheckUserLostState ();
        };
    }
    private void ChangeRight () {
        Debug.LogWarning ("Right");
        IsWheeling = false;
        IsAniming = true;
        currentImageIndex += 1;
        if (currentImageIndex >= showImage.Count) {
            currentImageIndex = 0;
        }
        showImage[currentImageIndex].rectTransform.SetSiblingIndex (maxIndex - 1);
        showImage[currentImageIndex].DOFade (1, 0);
        currentImage.DOFade (0, 1.0f).onComplete = () => {
            IsAniming = false;
            currentImage.rectTransform.SetAsFirstSibling ();
            showImage[currentImageIndex].rectTransform.SetAsLastSibling ();
            currentImage = showImage[currentImageIndex];
            CheckUserLostState ();
        };
    }
    private void ChangeWheel () {
        Debug.LogWarning ("Wheel");
        IsWheeling = true;
        IsAniming = true;
        waveImage.rectTransform.SetSiblingIndex (maxIndex - 1);
        waveImage.DOFade (1, 0);
        currentImage.DOFade (0, 1.0f).onComplete = () => {
            IsAniming = false;
            currentImage.rectTransform.SetAsFirstSibling ();
            waveImage.rectTransform.SetAsLastSibling ();
            currentImage = waveImage;
            CheckUserLostState ();
        };
    }
}