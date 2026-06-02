using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSettings : MonoBehaviour
{
    public GameObject pausePanel, winPanel, levelPanel, onlinePanel, hostPanel, joinPanel, settingsPanel;

    private void Update() {
        if (Input.GetKey(KeyCode.Escape)) { PauseButtonPressed(); }
        if (Input.GetKey(KeyCode.R)) { RestarButtonPressed(); }
    }

    public void PauseButtonPressed() {
        TurnOffCarSounds();
        pausePanel.SetActive(true);
        Time.timeScale = 0.0f;
        
    }
    public void ContinueButtonPressed() {
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void RestarButtonPressed() {
        SceneTransition.SwitchToScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }
    public void QuitButtonPressed() {
        SceneTransition.SwitchToScene(0);
        Time.timeScale = 1.0f;
    }
    public void NextButtonPressed() {
        Time.timeScale = 1.0f;
        winPanel.SetActive(false);
        SceneTransition.SwitchToScene((SceneManager.GetActiveScene().buildIndex) + 1);
    }
    public void ExitButtonPressed() {
        Application.Quit();
        Debug.Log("Application Was Closed.");
    }

    public void StartButtonPressed(bool activeBtn) { levelPanel.SetActive(activeBtn); }
    public void OnlineButtonPressed(bool activeBtn) { onlinePanel.SetActive(activeBtn); }
    public void OpenHostPanel(bool activeBtn) { hostPanel.SetActive(activeBtn); }
    public void OpenJoinPanel(bool activeBtn) { joinPanel.SetActive(activeBtn); }


    public void ClosePanelButtonPressed(bool activeBtnSet) { settingsPanel.SetActive(activeBtnSet); }

    public void ResetButtonPressed() {
        // Удаляем все данные, сохраненные в PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Перезагружаем текущую сцену, чтобы применить изменения
        SceneTransition.SwitchToScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    void TurnOffCarSounds()
    {
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
}
