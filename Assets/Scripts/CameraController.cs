using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Application.targetFrameRate = 60;
        Resolution res;
        res = Screen.currentResolution;

        Screen.fullScreen = false;

        Screen.SetResolution(res.width, res.height, true);

	}

}
