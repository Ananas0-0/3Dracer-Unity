using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class FinishLine : MonoBehaviour
{
    private bool hasPlayerPassed = false;
    private Dictionary<GameObject, float> carPassingTimes = new Dictionary<GameObject, float>();
    private float playerPassingTime = 0f;
    //private MoneyManager moneyManager;

    public TMP_Text placeWinText, placeLoseText, moneyText; // Ссылка на TextMeshPro для отображения места игрока
    public GameObject winPanel, losePanel;
    public int moneyWin;

    private void Start() {
        //moneyManager = GetComponent<MoneyManager>();
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !hasPlayerPassed)
        {
            // Обработка прохождения финишной линии игроком
            Debug.Log("Player has passed the finish line!");
            hasPlayerPassed = true;
            playerPassingTime = Time.time;
            CalculatePlace();
            TurnOffCarSounds();
        }
        else if (other.CompareTag("AI") && !carPassingTimes.ContainsKey(other.gameObject))
        {
            // Обработка прохождения финишной линии ботом
            carPassingTimes.Add(other.gameObject, Time.time);
        }
    }

    void CalculatePlace() {
        int botCarsPassed = 0;

        // Считаем количество ботов, которые пересекли финишную линию
        foreach (var time in carPassingTimes.Values)
        {
            if (time < playerPassingTime)
            {
                botCarsPassed++;
            }
        }

        // Место игрока равно количеству пройденных ботов плюс один
        int place = botCarsPassed + 1;

        if (place > 1) {
            Time.timeScale = 0.0f;
            losePanel.SetActive(true);
            placeLoseText.text = "Your place: " + place.ToString();
        }
        else {
            UnlockLevel();
            Time.timeScale = 0.0f;
            winPanel.SetActive(true);
            MoneyManager.instance.AddMoney(moneyWin);
            moneyText.text = "+ " + moneyWin.ToString();
            // Выводим место игрока на экран
            placeWinText.text = "Your place: " + place.ToString();
        }
    }

    void TurnOffCarSounds() {
        // Получаем все коллайдеры объектов на слое "Car"
        Collider[] carColliders = Physics.OverlapSphere(transform.position, 100f, LayerMask.GetMask("Car"));

        // Для каждого коллайдера на слое "Car" находим все аудиоисточники и выключаем их
        foreach (var collider in carColliders)
        {
            AudioSource[] carAudioSources = collider.GetComponentsInChildren<AudioSource>();
            foreach (var audioSource in carAudioSources)
            {
                audioSource.Stop();
            }
        }
    }

    public void UnlockLevel() {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        if (currentLevel >= PlayerPrefs.GetInt("levels")) {
            PlayerPrefs.SetInt("levels", currentLevel + 1);
        }
    }
}
