using UnityEngine;

public class CarPreviewSlot : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject[] carPrefabs;

    private GameObject currentCar;

    public void ShowCar(int carIndex)
    {
        ClearSlot();

        currentCar = Instantiate(
            carPrefabs[carIndex],
            spawnPoint.position,
            spawnPoint.rotation,
            spawnPoint
        );

        Rigidbody rb = currentCar.GetComponent<Rigidbody>();
        if (rb != null)
            Destroy(rb);

        foreach (var comp in currentCar.GetComponents<MonoBehaviour>())
        {
            Destroy(comp);
        }
    }

    public void ClearSlot()
    {
        if (currentCar != null)
            Destroy(currentCar);
    }
}