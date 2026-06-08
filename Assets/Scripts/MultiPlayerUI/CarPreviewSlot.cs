using UnityEngine;

public class CarPreviewSlot : MonoBehaviour
{
    [Header("Preview Settings")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject[] carPrefabs;
    [SerializeField] private float previewScale = 2f;

    private GameObject currentCar;

    public void ShowCar(int carIndex)
    {
        ClearSlot();

        if (carIndex < 0 || carIndex >= carPrefabs.Length)
            return;

        currentCar = Instantiate(
            carPrefabs[carIndex],
            spawnPoint.position,
            spawnPoint.rotation,
            spawnPoint
        );

        // ВАЖНО: сначала обнуляем локальный scale
        currentCar.transform.localScale = Vector3.one;

        // Потом задаём нужный размер
        currentCar.transform.localScale = Vector3.one * previewScale;

        // убираем физику
        Rigidbody rb = currentCar.GetComponent<Rigidbody>();
        if (rb != null)
            Destroy(rb);

        // убираем все скрипты
        foreach (var comp in currentCar.GetComponents<MonoBehaviour>())
        {
            Destroy(comp);
        }

        // убираем Network компоненты если есть
        var netIdentity = currentCar.GetComponent<Mirror.NetworkIdentity>();
        if (netIdentity != null)
            Destroy(netIdentity);
    }

    public void ClearSlot()
    {
        if (currentCar != null)
            Destroy(currentCar);
    }
}