using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceNetworkActions : NetworkBehaviour
{
    [Command]
    public void CmdRestartRace()
    {
        // ✅ Гарантируем нормальное время
        Time.timeScale = 1f;

        NetworkManager.singleton.ServerChangeScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    [Command]
    public void CmdSetPaused(bool value)
    {
        if (RaceState.Instance != null)
            RaceState.Instance.SetPaused(value);
    }
}