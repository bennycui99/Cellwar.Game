using UnityEngine;
using System.Collections;

public class PixelArtCamera : MonoBehaviour {

    public float pixelBaseSize = 64f;
    public float scale = 2f;
    Camera myCamera;

    void Start () {
        myCamera = GetComponent<Camera>();
        myCamera.orthographic = true;
    }
	
	void Update () {
        myCamera.orthographicSize = Screen.height / pixelBaseSize / scale;
    }
}
