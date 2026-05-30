using System.Collections;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TMP_Text timerText;
    private bool countdownFinished = false;
    private float startTime;

    void Start()
    {
        timerText = GetComponent<TMP_Text>();
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        // Отображаем обратный отсчёт
        for (int i = 5; i >= 0; i--)
        {
            // Изменяем цвет текста на красный
            timerText.color = Color.red;
            timerText.text = "00:0" + i;
            yield return new WaitForSeconds(1f);
        }
        countdownFinished = true;
        StartCoroutine(StartTimer());

        // Запускаем метод, который включает скрипт CarHandler-ы через 0 секунд после завершения отсчета
        Invoke("EnableCarHandler", 0f);
        // Включаем isKinematic для машин
        ToggleCarKinematic(false);
    }

    IEnumerator StartTimer()
    {
        // Изменяем цвет текста на белый
        timerText.color = Color.white;
        float startTime = Time.time;
        
        while (!countdownFinished)
        {
            float elapsedTime = Time.time - startTime;
            string minutes = Mathf.Floor(elapsedTime / 60).ToString("00");
            string seconds = (elapsedTime % 60).ToString("00");
            timerText.text = minutes + ":" + seconds;
            yield return null;
        }
        // Обновляем startTime для обычного отсчёта времени
        startTime = Time.time;
        
        while (true)
        {
            float elapsedTime = Time.time - startTime;
            string minutes = Mathf.Floor(elapsedTime / 60).ToString("00");
            string seconds = (elapsedTime % 60).ToString("00");
            timerText.text = minutes + ":" + seconds;
            yield return null;
        }
    }

    void EnableCarHandler()
    {
        // Включаем скрипт AICarHandler у всех машин
        AICarHandler[] aiCarHandlers = FindObjectsOfType<AICarHandler>();
        foreach (AICarHandler aiCarHandler in aiCarHandlers)
        {
            aiCarHandler.enabled = true;
        }

        // Включаем скрипт AICarHandler у всех машин
        PlayerCarHandler[] playerCarHandlers = FindObjectsOfType<PlayerCarHandler>();
        foreach (PlayerCarHandler playerCarHandler in playerCarHandlers)
        {
            playerCarHandler.enabled = true;
        }
    }

    void ToggleCarKinematic(bool isKinematic)
    {
        // Включаем/выключаем isKinematic для машин
        AICarHandler[] aiCarHandlers = FindObjectsOfType<AICarHandler>();
        foreach (AICarHandler aiCarHandler in aiCarHandlers)
        {
            Rigidbody aiRigidbody = aiCarHandler.GetComponent<Rigidbody>();
            if (aiRigidbody != null)
            {
                aiRigidbody.isKinematic = isKinematic;
            }
        }
        // Включаем/выключаем isKinematic для машин
        PlayerCarHandler[] playerCarHandlers = FindObjectsOfType<PlayerCarHandler>();
        foreach (PlayerCarHandler playerCarHandler in playerCarHandlers)
        {
            Rigidbody playerRigidbody = playerCarHandler.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                playerRigidbody.isKinematic = isKinematic;
            }
        }
    }

    void Update()
    {
        if (!countdownFinished)
        {
            // Если отсчет не завершен, замораживаем позицию машин
            ToggleCarKinematic(true);
        }
    }
}
