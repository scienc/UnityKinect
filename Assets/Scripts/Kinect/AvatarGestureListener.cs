using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AvatarGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    private static AvatarGestureListener instance = null;
    public static AvatarGestureListener Instance
    {
        get
        {
            return instance;
        }
    }

    private bool progressDisplayed;
    private float progressGestureTime;

    public System.Action delegateUserDetected;
    public System.Action delegateUserLost;
    public System.Action<KinectGestures.Gestures> delegateGestureCompleted;
    public System.Action<KinectGestures.Gestures> delegateGestureCancel;

    public void UserDetected(long userId, int userIndex)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return;

        // detect these user specific gestures
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeLeft);
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeRight);
        manager.DetectGesture(userId, KinectGestures.Gestures.Wheel);

        if (delegateUserDetected != null)
            delegateUserDetected();
    }
    public void UserLost(long userId, int userIndex)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return;

        if (delegateUserLost != null)
            delegateUserLost();
    }
    public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  float progress, KinectInterop.JointType joint, Vector3 screenPos)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return;
        if (gesture == KinectGestures.Gestures.Wheel && progress > 0.5f)
        {
            if (delegateGestureCompleted != null)
            {
                delegateGestureCompleted(gesture);
            }
        }
    }
    public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectInterop.JointType joint, Vector3 screenPos)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return false;
        if (delegateGestureCompleted != null)
            delegateGestureCompleted(gesture);
        return true;
    }

    public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectInterop.JointType joint)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return false;
        if (delegateGestureCancel != null)
            delegateGestureCancel(gesture);
        return true;
    }


    void Awake()
    {
        instance = this;
    }
}
