using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSkip : MonoBehaviour
{
    public float timeSpeed = 0.0f;
    private int num = 1;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.L))
            Time.timeScale = 1.0f;
        if (Input.GetKey(KeyCode.K))
            Time.timeScale = 0.0f;
        if (Input.GetKey(KeyCode.J))
            Time.timeScale = timeSpeed;
        if (Input.GetKey(KeyCode.P))
            ScreenCapture.CaptureScreenshot("picture" + num + ".png");
            num++;
    }
}
