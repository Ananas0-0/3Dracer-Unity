using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRaceManager : NetworkManager
{
    [Header("Player Car Prefabs")]
    public GameObject[] playerCarPrefabs;

    [Header("Map Settings")]
    public string[] mapSceneNames;

    // ===============================
    // ===== START GAME ==============
    // ===============================

    [Server]
    public void StartGame()
    {
        if (LobbyState.Instance == null)
            return;

        int index = LobbyState.Instance.selectedMapIndex;

        if (index < 0 || index >= mapSceneNames.Length)
            return;

        ServerChangeScene(mapSceneNames[index]);
    }

    // ===============================
    // ===== PLAYER SPAWN ============
    // ===============================

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            base.OnServerAddPlayer(conn);
            return;
        }

        int carIndex = PlayerPrefs.GetInt("CurrentCar", 0);

        if (carIndex < 0 || carIndex >= playerCarPrefabs.Length)
            carIndex = 0;

        GameObject prefab = playerCarPrefabs[carIndex];

        RaceGameManager gameManager = FindObjectOfType<RaceGameManager>();

        Transform spawnPoint = gameManager.GetRandomSpawnPoint();

        Vector3 pos = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        Quaternion rot = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;

        GameObject player = Instantiate(prefab, pos, rot);

        NetworkServer.AddPlayerForConnection(conn, player);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        // Переключаем UI у клиента
        LobbyMenuController.Instance?.ShowLobby();
    }

    public override void OnStartHost()
    {
        base.OnStartHost();

        LobbyUI.Instance?.OnHostStarted();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);

        StartCoroutine(DelayedRefresh());
    }

    private System.Collections.IEnumerator DelayedRefresh()
    {
        yield return null;
        if (LobbyUI.Instance != null);
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        LobbyMenuController.Instance?.ShowMainMenu();
    }
    
}