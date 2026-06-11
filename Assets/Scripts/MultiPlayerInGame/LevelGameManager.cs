using UnityEngine;
using System.Collections.Generic;
using Mirror;

public class LevelGameManager : MonoBehaviour
{
    [Header("Blocks")]
    [SerializeField] private GameObject spBlock;
    [SerializeField] private GameObject mpBlock;

    [Header("MP Spawn Points")]
    [SerializeField] private Transform[] networkSpawnPoints;

    private int nextSpawnIndex = 0;

    private void Start()
    {
        if (GameSession.CurrentMode == GameMode.MultiPlayer)
        {
            spBlock.SetActive(false);
            mpBlock.SetActive(true);
        }
        else
        {
            spBlock.SetActive(true);
            mpBlock.SetActive(false);
        }
        Debug.Log("Scene loaded on: " + (NetworkServer.active ? "Host" : "Client"));
    }

    public Transform GetNextSpawnPoint()
    {
        if (networkSpawnPoints == null || networkSpawnPoints.Length == 0)
            return null;

        if (nextSpawnIndex >= networkSpawnPoints.Length)
            return null;

        Transform point = networkSpawnPoints[nextSpawnIndex];
        nextSpawnIndex++;

        return point;
    }
}