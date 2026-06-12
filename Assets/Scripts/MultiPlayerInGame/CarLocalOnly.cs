using Mirror;
using UnityEngine;
using Cinemachine;

public class CarLocalOnly : NetworkBehaviour
{
    [Header("Root with local-only objects (VCam, HUD, minimap etc.)")]
    [SerializeField] private GameObject localOnlyRoot;

    [Header("Optional: vcam inside LocalOnly (auto-find if null)")]
    [SerializeField] private CinemachineVirtualCamera vcam;

    private void Awake()
    {
        if (localOnlyRoot == null)
            localOnlyRoot = transform.Find("LocalOnly")?.gameObject;

        if (vcam == null && localOnlyRoot != null)
            vcam = localOnlyRoot.GetComponentInChildren<CinemachineVirtualCamera>(true);
    }

    public override void OnStartClient() => Apply();
    public override void OnStartAuthority() => Apply();
    public override void OnStopAuthority() => Apply();

    private void Apply()
    {
        bool local = isOwned; // в твоей версии Mirror

        if (localOnlyRoot != null)
            localOnlyRoot.SetActive(local);

        // На всякий случай: если vcam есть, у локального поднимем приоритет
        if (vcam != null)
            vcam.Priority = local ? 50 : 0;
    }
}