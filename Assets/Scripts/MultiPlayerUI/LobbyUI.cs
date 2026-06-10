using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using System.Collections.Generic;

public class LobbyUI : MonoBehaviour
{
    public static LobbyUI Instance;

    [System.Serializable]
    public class PlayerSlot
    {
        public GameObject root;
        public Image avatarImage;
        public TMP_Text nicknameText;
        public GameObject crownIcon;
    }

    [Header("Players")]
    public PlayerSlot[] slots;
    public Sprite[] avatarSprites;

    [Header("Map UI")]
    public Image mapPreviewImage;
    public Sprite[] mapSprites;
    public GameObject mapSelectPanel;
    public Button startButton;

    [Header("Level Selector")]
    public LobbyLevelSelector levelSelector;

    [Header("IP")]
    [SerializeField] private GameObject ipPanel;
    [SerializeField] private TMP_Text ipText;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
         // IP только для хоста
        if (NetworkServer.active)
        {
            ipPanel.SetActive(true);
            ipText.text = "IP: " + GetLocalIPAddress();
        }
        else { ipPanel.SetActive(false); }
        Debug.Log("Server active: " + NetworkServer.active);

        Refresh();
    }

    void OnEnable()
    {
        Refresh();
    }

    string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                return ip.ToString();
        }
        return "Not found";
    }

    public void CreateHost() { NetworkManager.singleton.StartHost(); }

    // ===============================
    // ===== PLAYER REFRESH ==========
    // ===============================

    public void Refresh()
    {
        if (slots == null || slots.Length == 0)
            return;

        var players = new List<LobbyPlayer>(FindObjectsOfType<LobbyPlayer>());

        players.Sort((a, b) => a.netId.CompareTo(b.netId));

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                continue;

            if (i < players.Count)
            {
                if (slots[i].root != null)
                slots[i].root.SetActive(true);

                if (slots[i].nicknameText != null)
                    slots[i].nicknameText.text = players[i].nickname;

                if (slots[i].avatarImage != null &&
                    players[i].avatarIndex >= 0 &&
                    players[i].avatarIndex < avatarSprites.Length)
                {
                    slots[i].avatarImage.sprite =
                        avatarSprites[players[i].avatarIndex];
                }

                if (slots[i].crownIcon != null)
                    slots[i].crownIcon.SetActive(players[i].isServer);
            }
            else
            {
                if (slots[i].root != null)
                    slots[i].root.SetActive(false);
            }
        }
    }

    // ===============================
    // ===== MAP SELECTION ===========
    // ===============================

    public void SelectMap(int index)
    {
        if (!NetworkServer.active) return;

        LobbyState.Instance.SetMap(index);

        if (index >= 0 && index < mapSprites.Length)
            mapPreviewImage.sprite = mapSprites[index];

        mapSelectPanel.SetActive(false);
    }

    public void UpdateMapPreview(int index)
    {
        if (levelSelector != null)
        {
            levelSelector.UpdatePanels(index);
            levelSelector.UpdateButtons(index);
        }

        if (index >= 0 && index < mapSprites.Length)
            mapPreviewImage.sprite = mapSprites[index];
    }

    // ===============================
    // ===== GAME CONTROL ============
    // ===============================

    public void StartGame()
    {
        if (!NetworkServer.active) return;

        ((NetworkRaceManager)NetworkManager.singleton).StartGame();
    }

    public void LeaveLobby()
    {
        if (NetworkServer.active)
        {
            // Сначала уведомляем клиентов
            if (LobbyState.Instance != null)
                LobbyState.Instance.RpcHostLeft();

            // Потом останавливаем сервер
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }

        LobbyMenuController.Instance?.ShowMainMenu();
    }
}