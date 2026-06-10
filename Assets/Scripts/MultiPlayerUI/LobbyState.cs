using Mirror;
using UnityEngine;

public class LobbyState : NetworkBehaviour
{
    public static LobbyState Instance;

    [SyncVar(hook = nameof(OnMapChanged))]
    public int selectedMapIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    void OnMapChanged(int oldValue, int newValue)
    {
        LobbyUI.Instance?.UpdateMapPreview(newValue);
    }

    [Server]
    public void SetMap(int index)
    {
        selectedMapIndex = index;
    }

    [ClientRpc]
    public void RpcHostLeft()
    {
        LobbyMenuController.Instance?.ShowMainMenu();
    }
}