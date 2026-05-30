using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] AllCar;


    private void Start()
    {
        for (int i = 0; i < AllCar.Length; i++)
        {
            AllCar[i].SetActive(false);
        }
        AllCar[PlayerPrefs.GetInt("CurrentCar",0)].gameObject.SetActive(true);
    }
}
