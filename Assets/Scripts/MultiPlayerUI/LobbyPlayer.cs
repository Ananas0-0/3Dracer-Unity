using Mirror;
using UnityEngine;

public class LobbyPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnNicknameChanged))]
    public string nickname;

    [SyncVar(hook = nameof(OnAvatarChanged))]
    public int avatarIndex;

    public override void OnStartLocalPlayer()
    {
        string nick = PlayerPrefs.GetString("PlayerNickname", "Player");
        CmdSetup(nick, 0);
    }

    public override void OnStartClient()
    {
        LobbyUI.Instance?.Refresh();
    }

    public override void OnStopClient()
    {
        LobbyUI.Instance?.Refresh();
    }

    [Command]
    void CmdSetup(string nick, int avatar)
    {
        nickname = nick;
        avatarIndex = avatar;
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