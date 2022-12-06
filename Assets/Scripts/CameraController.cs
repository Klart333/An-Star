using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    public void UpdateSize(int x, int y)
    {
        if (x > y * 2.5f)
        {
            cam.orthographicSize = x / 4.0f + 1;
        }
        else
        {
            cam.orthographicSize = y / 2.0f + 1;
        }
        cam.transform.position = new Vector3(0, y / 30.0f, -10);
    }
}
