using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CarSelector : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();
    private int currentIndex = 0;

    public Button nextButton;
    public Button prevButton;

    private const string PlayerPrefsKey = "SelectedCarIndex";

    void Start()
    {
        // Назначаем обработчики событий для кнопок
        nextButton.onClick.AddListener(ShowNextObject);
        prevButton.onClick.AddListener(ShowPreviousObject);

        // Загружаем сохраненный индекс, если есть
        if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            currentIndex = PlayerPrefs.GetInt(PlayerPrefsKey);
        }

        // Показываем текущий объект
        ShowCurrentObject();
    }

    public void ShowCurrentObject()
    {
        // Скрываем все объекты, кроме текущего
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(i == currentIndex);
        }

        // Отключаем кнопки, если мы находимся на границах списка
        nextButton.interactable = currentIndex < objects.Count - 1;
        prevButton.interactable = currentIndex > 0;
    }

    public void ShowNextObject()
    {
        // Увеличиваем индекс и показываем следующий объект, если возможно
        if (currentIndex < objects.Count - 1)
        {
            currentIndex++;
            SaveCurrentIndex();
            ShowCurrentObject();
        }
    }

    public void ShowPreviousObject()
    {
        // Уменьшаем индекс и показываем предыдущий объект, если возможно
        if (currentIndex > 0)
        {
            currentIndex--;
            SaveCurrentIndex();
            ShowCurrentObject();
        }
    }

    void SaveCurrentIndex()
    {
        // Сохраняем текущий индекс в PlayerPrefs
        PlayerPrefs.SetInt(PlayerPrefsKey, currentIndex);
        PlayerPrefs.Save();
    }
}
