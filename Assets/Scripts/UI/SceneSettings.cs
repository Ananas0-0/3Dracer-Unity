using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class SceneSettings : MonoBehaviour
{
    public GameObject pausePanel, winPanel, levelPanel, onlinePanel, hostPanel, joinPanel, settingsPanel;

    [Header("In-Game Buttons (optional)")]
    [SerializeField] private GameObject restartButton; // кнопку рестарта скрываем у клиента в MP

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) PauseButtonPressed();
        if (Input.GetKeyDown(KeyCode.R)) RestarButtonPressed();
    }

    // ===============================
    // ===== GAME MODE BUTTONS =======
    // ===============================

    public void StartSinglePlayer()
    {
        GameSession.CurrentMode = GameMode.SinglePlayer;

        // если случайно клиент всё ещё активен
        if (NetworkClient.active)
        NetworkManager.singleton.StopClient();

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
        // SP: обычная загрузка
        if (GameSession.CurrentMode == GameMode.SinglePlayer)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(buildIndex);
            return;
        }

        // MP: сцену меняет только хост
        if (!NetworkServer.active)
            return;

        Time.timeScale = 1f;
        string sceneName = SceneUtility.GetScenePathByBuildIndex(buildIndex);
        sceneName = System.IO.Path.GetFileNameWithoutExtension(sceneName);

        NetworkManager.singleton.ServerChangeScene(sceneName);
    }

    // ===============================
    // ===== UI CONTROLS =============
    // ===============================

    void RequestPause(bool value)
    {
        if (!NetworkClient.active || NetworkClient.localPlayer == null) return;
        var actions = NetworkClient.localPlayer.GetComponentInChildren<RaceNetworkActions>(true);
        if (actions != null) actions.CmdSetPaused(value);
    }

    
    public void PauseButtonPressed()
    {
        TurnOffCarSounds();
        pausePanel.SetActive(true);

        if (GameSession.CurrentMode == GameMode.SinglePlayer) Time.timeScale = 0f;
        else RequestPause(true);
    }

    public void ContinueButtonPressed()
    {
        pausePanel.SetActive(false);

        if (GameSession.CurrentMode == GameMode.SinglePlayer) Time.timeScale = 1f;
        else RequestPause(false);
    }

    public void RestarButtonPressed()
    {
        if (GameSession.CurrentMode == GameMode.SinglePlayer)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        // MP: любой игрок может инициировать рестарт, но выполняет сервер
        if (!NetworkClient.active || NetworkClient.localPlayer == null)
            return;

        var actions = NetworkClient.localPlayer.GetComponent<RaceNetworkActions>();
        if (actions != null)
            actions.CmdRestartRace();
    }

    public void QuitButtonPressed()
    {
        if (GameSession.CurrentMode == GameMode.SinglePlayer)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
            return;
        }

        var nm = NetworkManager.singleton;

        if (nm != null)
        {
            if (nm.mode == NetworkManagerMode.Host)
                nm.StopHost();
            else if (nm.mode == NetworkManagerMode.ClientOnly)
                nm.StopClient();
        }

        // ✅ Сбрасываем режим
        GameSession.CurrentMode = GameMode.SinglePlayer;

        Time.timeScale = 1f;

        SceneManager.LoadScene(0); // вернуться в меню
    }
    
    public void NextButtonPressed()
    {
        // В MP лучше не использовать, иначе клиент сам загрузит сцену.
        if (GameSession.CurrentMode == GameMode.MultiPlayer)
            return;

        Time.timeScale = 1f;
        winPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
        Debug.Log("Application Was Closed.");
    }

    // ===== Menu panels =====
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

    // ===============================
    // ===== Helpers ================
    // ===============================

    private void SetLocalCarInputEnabled(bool enabled)
    {
        if (!NetworkClient.active) return;
        if (NetworkClient.localPlayer == null) return;

        var input = NetworkClient.localPlayer.GetComponentInChildren<PlayerCarHandler>(true);
        if (input != null) input.enabled = enabled;
    }

    void TurnOffCarSounds()
    {
        Collider[] carColliders = Physics.OverlapSphere(transform.position, 100f, LayerMask.GetMask("Car"));
        foreach (var collider in carColliders)
        {
            AudioSource[] carAudioSources = collider.GetComponentsInChildren<AudioSource>();
            foreach (var audioSource in carAudioSources)
                audioSource.Stop();
        }
    }
}