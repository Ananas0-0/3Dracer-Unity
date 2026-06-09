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
        // В Menu создаём LobbyPlayer
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            base.OnServerAddPlayer(conn);
            return;
        }

        // В игровой сцене создаём машину
        int carIndex = PlayerPrefs.GetInt("CurrentCar", 0);

        if (carIndex < 0 || carIndex >= playerCarPrefabs.Length)
            carIndex = 0;

        GameObject prefab = playerCarPrefabs[carIndex];

        Transform startPos = GetStartPosition();

        Vector3 pos = startPos != null ? startPos.position : Vector3.zero;
        Quaternion rot = startPos != null ? startPos.rotation : Quaternion.identity;

        GameObject player = Instantiate(prefab, pos, rot);

        NetworkServer.AddPlayerForConnection(conn, player);
    }
}