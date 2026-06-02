using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRaceManager : NetworkManager
{
    [Header("Player Car Prefabs (order = shop CarIndex)")]
    public GameObject[] playerCarPrefabs;

    [Header("Menu Spawn Points (4 slots)")]
    [SerializeField] private Transform[] menuSpawnPoints;

    private int spawnIndex = 0;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        int carIndex = PlayerPrefs.GetInt("CurrentCar", 0);

        if (carIndex < 0 || carIndex >= playerCarPrefabs.Length)
            carIndex = 0;

        GameObject prefab = playerCarPrefabs[carIndex];

        Vector3 pos;
        Quaternion rot;

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            Transform start = menuSpawnPoints[spawnIndex % menuSpawnPoints.Length];
            spawnIndex++;

            pos = start.position;
            rot = start.rotation;
        }
        else
        {
            Transform startPos = GetStartPosition();
            pos = startPos != null ? startPos.position : Vector3.zero;
            rot = startPos != null ? startPos.rotation : Quaternion.identity;
        }

        GameObject player = Instantiate(prefab, pos, rot);
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            Transform body = player.transform.Find("Body/Body");

            if (body != null)
                body.localScale *= 3f;
        }

        DisableLocalObjects(player);

        NetworkServer.AddPlayerForConnection(conn, player);
    }

    void DisableLocalObjects(GameObject player)
    {
        if (SceneManager.GetActiveScene().name != "Menu")
            return;

        Transform canvas = player.transform.Find("Canvas");
        if (canvas) canvas.gameObject.SetActive(false);

        Transform vcam = player.transform.Find("Virtual Camera");
        if (vcam) vcam.gameObject.SetActive(false);

        Transform listener = player.transform.Find("Listener");
        if (listener) listener.gameObject.SetActive(false);

        Transform miniMap = player.transform.Find("MiniMapCamera");
        if (miniMap) miniMap.gameObject.SetActive(false);
    }
}