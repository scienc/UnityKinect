using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour {
    private void Awake () {
        for (int i = 0; i < Display.displays.Length; i++) {
            Display.displays[i].Activate ();
            Screen.SetResolution (Screen.width, Screen.height, true);
        }
        Debug.Log ("Display.displays " + Display.displays.Length);
    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            Application.Quit ();
        } else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
            Debug.Log ("LeftArrow");
        } else if (Input.GetKeyDown (KeyCode.RightArrow)) {
            Debug.Log ("RightArrow");
        }
    }
}