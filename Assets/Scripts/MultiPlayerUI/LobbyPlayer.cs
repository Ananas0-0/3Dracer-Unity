using Mirror;
using UnityEngine;

public class LobbyPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnNicknameChanged))]
    public string nickname;

    [SyncVar(hook = nameof(OnAvatarChanged))]
    public int avatarIndex;

    [SyncVar]
    public int selectedCarIndex;   // ✅ добавили

    public override void OnStartLocalPlayer()
    {
        string nick = PlayerPrefs.GetString("PlayerNickname", "Player");
        int carIndex = PlayerPrefs.GetInt("CurrentCar", 0);

        CmdSetup(nick, 0, carIndex);
    }

    [Command]
    void CmdSetup(string nick, int avatar, int carIndex)
    {
        nickname = nick;
        avatarIndex = avatar;
        selectedCarIndex = carIndex;  // ✅ передаём серверу

        // ✅ сохраняем в connection
        connectionToClient.authenticationData = carIndex;
    }

    public override void OnStartClient()
    {
        LobbyUI.Instance?.Refresh();
    }

    public override void OnStopClient()
    {
        LobbyUI.Instance?.Refresh();
    }

    void OnNicknameChanged(string oldValue, string newValue)
    {
        LobbyUI.Instance?.Refresh();
    }

    void OnAvatarChanged(int oldValue, int newValue)
    {
        LobbyUI.Instance?.Refresh();
    }
}