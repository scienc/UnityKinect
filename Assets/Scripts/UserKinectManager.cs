using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserKinectManager : MonoBehaviour
{

    public RawImage kinectImg;

    private KinectManager manager;
    // Use this for initialization
    void Start()
    {
        manager = KinectManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (manager != null)
        {
            //设备准备好了  可以读取了
            if (kinectImg.texture == null)
            {
                Texture2D kinectPic = KinectManager.Instance.GetUsersClrTex();  //从设备获取彩色数据
                                                                                // Texture2D kinectPic = KinectManager.Instance.GetUsersLblTex();  //获取深度数据量
                kinectImg.texture = kinectPic;  //把彩色数据给控件显示
            }

            if (KinectManager.Instance.IsUserDetected())
            {
                //检测到玩家
                long userId = KinectManager.Instance.GetPrimaryUserID();  //获取用户id

                Vector3 userPos = KinectManager.Instance.GetUserPosition(userId);  //获取用户离Kinect的距离信息
                print("x = " + userPos.x + " y = " + userPos.y + " z = " + userPos.z);


                int jointType = (int)KinectInterop.JointType.HandLeft;
                if (KinectManager.Instance.IsJointTracked(userId, jointType))
                {
                    //关节点被追踪到
                    Vector3 leftHandPos = KinectManager.Instance.GetJointKinectPosition(userId, jointType);
                    //Vector3 leftHandPos = KinectManager.Instance.GetJointPosition(userId, jointType);  //y轴输出不一样 
                    // print("x = " + leftHandPos.x + " y = " + leftHandPos.y + " z = " + leftHandPos.z);

                    KinectInterop.HandState leftHandState = KinectManager.Instance.GetLeftHandState(userId); //获取左手姿势
                    if (leftHandState == KinectInterop.HandState.Closed)
                    {
                        print("左手握拳");
                    }
                    else if (leftHandState == KinectInterop.HandState.Open)
                    {
                        print("左手张开");
                    }
                    else if (leftHandState == KinectInterop.HandState.Lasso)
                    {
                        print("yes手势");
                    }
                }
            }
        }
    }
}