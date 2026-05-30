using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarHandler : MonoBehaviour
{

    [SerializeField] private GameObject flashFront;
    [SerializeField] private GameObject flashRear;
    private bool isLighting = false;
    CarController carController;

    private void Awake() {
        carController = GetComponent<CarController>();
    }
    private void Start() {
        
    }
    public void Update() {
        Vector3 inputVector = Vector3.zero;

        inputVector.z = Input.GetAxis("Vertical");
        inputVector.x = Input.GetAxis("Horizontal");

        carController.SetInputVector(inputVector);
    }

    private void FlashLightEnable() {
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
