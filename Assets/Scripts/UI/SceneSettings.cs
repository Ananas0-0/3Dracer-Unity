using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class SceneSettings : MonoBehaviour
{
    public GameObject pausePanel, winPanel, levelPanel, onlinePanel, hostPanel, joinPanel, settingsPanel;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) PauseButtonPressed();
        if (Input.GetKey(KeyCode.R)) RestarButtonPressed();
    }

    // ===============================
    // ===== GAME MODE BUTTONS =======
    // ===============================

    public void StartSinglePlayer()
    {
        GameSession.CurrentMode = GameMode.SinglePlayer;
        levelPanel.SetActive(true);
    }

    public void StartMultiPlayer()
    {
        GameSession.CurrentMode = GameMode.MultiPlayer;
        onlinePanel.SetActive(true);
    }

    // ===============================
    // ===== LEVEL LOADING ===========
    // ===============================

    public void LoadLevel(int buildIndex)
    {
        Time.timeScale = 1f;

        if (GameSession.CurrentMode == GameMode.SinglePlayer)
        {
            SceneManager.LoadScene(buildIndex);
        }
        else
        {
            if (NetworkServer.active)
            {
                NetworkManager.singleton.ServerChangeScene(
                    SceneManager.GetSceneByBuildIndex(buildIndex).name
                );
            }
        }
    }

    // ===============================
    // ===== UI CONTROLS =============
    // ===============================

    public void PauseButtonPressed()
    {
        TurnOffCarSounds();
        pausePanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void ContinueButtonPressed()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void RestarButtonPressed()
    {
        Time.timeScale = 1.0f;

        if (GameSession.CurrentMode == GameMode.SinglePlayer)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else
            NetworkManager.singleton.ServerChangeScene(SceneManager.GetActiveScene().name);
    }

    public void QuitButtonPressed()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void NextButtonPressed()
    {
        Time.timeScale = 1.0f;
        winPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
        Debug.Log("Application Was Closed.");
    }

    public void StartButtonPressed(bool activeBtn) => levelPanel.SetActive(activeBtn);
    public void OnlineButtonPressed(bool activeBtn) => onlinePanel.SetActive(activeBtn);
    public void OpenHostPanel(bool activeBtn) => hostPanel.SetActive(activeBtn);
    public void OpenJoinPanel(bool activeBtn) => joinPanel.SetActive(activeBtn);
    public void ClosePanelButtonPressed(bool activeBtnSet) => settingsPanel.SetActive(activeBtnSet);

    public void ResetButtonPressed()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void TurnOffCarSounds()
    {
        Collider[] carColliders = Physics.OverlapSphere(
            transform.position,
            100f,
            LayerMask.GetMask("Car")
        );

        foreach (var collider in carColliders)
        {
            AudioSource[] carAudioSources =
                collider.GetComponentsInChildren<AudioSource>();

            foreach (var audioSource in carAudioSources)
                audioSource.Stop();
        }
    }
}