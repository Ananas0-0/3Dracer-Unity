using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CarSelector : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();
    private int currentIndex = 0;

    public Button nextButton;
    public Button prevButton;

    void Start()
    {
        nextButton.onClick.AddListener(ShowNextObject);
        prevButton.onClick.AddListener(ShowPreviousObject);

        // используем тот же ключ что и магазин
        currentIndex = PlayerPrefs.GetInt("CurrentCar", 0);

        ShowCurrentObject();
    }

    public void ShowCurrentObject()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(i == currentIndex);
        }

        nextButton.interactable = currentIndex < objects.Count - 1;
        prevButton.interactable = currentIndex > 0;
    }

    public void ShowNextObject()
    {
        if (currentIndex < objects.Count - 1)
        {
            currentIndex++;
            SaveCurrentIndex();
            ShowCurrentObject();
        }
    }

    public void ShowPreviousObject()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            SaveCurrentIndex();
            ShowCurrentObject();
        }
    }

    void SaveCurrentIndex()
    {
        PlayerPrefs.SetInt("CurrentCar", currentIndex);
        PlayerPrefs.Save();
    }
}