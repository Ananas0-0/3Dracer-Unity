using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class RaceGameManager : MonoBehaviour
{
    [Header("SinglePlayer")]
    public GameObject[] botPrefabs;
    public Transform[] botSpawnPoints;

    [Header("Multiplayer")]
    public Transform[] networkSpawnPoints;

    private List<Transform> availableSpawnPoints;

    void Start()
    {
        bool isMultiplayer = NetworkClient.active;

        if (isMultiplayer)
            SetupMultiplayer();
        else
            SetupSinglePlayer();
    }

    // ===============================
    // ===== SINGLE PLAYER ===========
    // ===============================

    void SetupSinglePlayer()
    {
        // Машина игрока уже активируется через CarSpawner

        // Спавним 3 бота
        for (int i = 0; i < botPrefabs.Length && i < botSpawnPoints.Length; i++)
        {
            Instantiate(botPrefabs[i], botSpawnPoints[i].position, botSpawnPoints[i].rotation);
        }
    }

    // ===============================
    // ===== MULTIPLAYER =============
    // ===============================

    void SetupMultiplayer()
    {
        if (!NetworkServer.active)
            return;

        availableSpawnPoints = new List<Transform>(networkSpawnPoints);
    }

    public Transform GetRandomSpawnPoint()
    {
        if (availableSpawnPoints == null || availableSpawnPoints.Count == 0)
            return null;

        int index = Random.Range(0, availableSpawnPoints.Count);

        Transform point = availableSpawnPoints[index];
        availableSpawnPoints.RemoveAt(index);

        return point;
    }
}