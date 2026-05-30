using UnityEngine;
using System.Collections.Generic;

public class AISpawner : MonoBehaviour
{
    public List<GameObject> prefabsToSpawn = new List<GameObject>();
    public List<Transform> spawnPoints = new List<Transform>();
    public bool spawnOnce = true;

    private HashSet<int> usedIndexes = new HashSet<int>();

    void Start()
    {
        if (spawnOnce && prefabsToSpawn.Count > 0 && spawnPoints.Count > 0)
        {
            SpawnUniqueObject();
        }
    }

    void SpawnUniqueObject()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points specified!");
            return;
        }

        for (int i = 0; i < prefabsToSpawn.Count; i++)
        {
            int randomIndex = GetUniqueRandomIndex();
            if (randomIndex == -1)
            {
                Debug.LogWarning("All spawn points have been used!");
                return;
            }

            GameObject prefab = prefabsToSpawn[i];
            Transform spawnPoint = spawnPoints[randomIndex];

            // Создаем новый экземпляр выбранного префаба на позиции указанного spawnPoint
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    int GetUniqueRandomIndex()
    {
        if (usedIndexes.Count >= spawnPoints.Count)
        {
            return -1;
        }

        int randomIndex = Random.Range(0, spawnPoints.Count);
        while (usedIndexes.Contains(randomIndex))
        {
            randomIndex = (randomIndex + 1) % spawnPoints.Count;
        }

        usedIndexes.Add(randomIndex);
        return randomIndex;
    }
}
