using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashController : MonoBehaviour
{
    public GameObject flashFront;
    public GameObject flashRear;
    private bool isLighting = false;

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            isLighting = !isLighting;
            flashFront.SetActive(isLighting);
        }
        if ((Input.GetKeyDown(KeyCode.Space)) || (Input.GetKeyDown(KeyCode.S))) {
            flashRear.SetActive(true);
        }
        if ((Input.GetKeyUp(KeyCode.Space)) || (Input.GetKeyUp(KeyCode.S))) {
            flashRear.SetActive(false);
        }
    }
}
