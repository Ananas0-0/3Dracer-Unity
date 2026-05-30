using UnityEngine;
using UnityEngine.UI;

public class LevelPanelSwitcher : MonoBehaviour
{
    public GameObject[] levelPanels; // Массив для хранения панелей уровней
    public Button nextButton; // Кнопка для переключения на следующий уровень
    public Button previousButton; // Кнопка для переключения на предыдущий уровень

    private int currentIndex = 0; // Индекс текущей активной панели
    private const string LevelPanelKey = "levelPanel"; // Ключ для сохранения текущего уровня в PlayerPrefs

    void Start()
    {
        // Получить сохраненный индекс из PlayerPrefs
        currentIndex = PlayerPrefs.GetInt(LevelPanelKey, 0);
        // Убедиться, что индекс находится в допустимых пределах
        currentIndex = Mathf.Clamp(currentIndex, 0, levelPanels.Length - 1);
        // Показать только текущую панель
        UpdatePanels();
        // Обновить состояние кнопок
        UpdateButtons();
    }

    // Метод для переключения на следующую панель
    public void SwitchToNext()
    {
        if (currentIndex < levelPanels.Length - 1)
        {
            currentIndex++;
            SaveCurrentIndex();
            UpdatePanels();
            UpdateButtons();
        }
    }

    // Метод для переключения на предыдущую панель
    public void SwitchToPrevious()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            SaveCurrentIndex();
            UpdatePanels();
            UpdateButtons();
        }
    }

    // Метод для обновления видимости панелей
    private void UpdatePanels()
    {
        for (int i = 0; i < levelPanels.Length; i++)
        {
            levelPanels[i].SetActive(i == currentIndex);
        }
    }

    // Метод для обновления состояния кнопок
    private void UpdateButtons()
    {
        nextButton.interactable = currentIndex < levelPanels.Length - 1;
        previousButton.interactable = currentIndex > 0;
    }

    // Метод для сохранения текущего индекса в PlayerPrefs
    private void SaveCurrentIndex()
    {
        PlayerPrefs.SetInt(LevelPanelKey, currentIndex);
        PlayerPrefs.Save();
    }
}
