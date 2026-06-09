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

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Refresh();
    }

    void OnEnable()
    {
        Refresh();
    }

    public void CreateHost() { NetworkManager.singleton.StartHost(); }

    // ===============================
    // ===== PLAYER REFRESH ==========
    // ===============================

    public void Refresh()
    {
        var players = new List<LobbyPlayer>(FindObjectsOfType<LobbyPlayer>());

        // Стабильный порядок
        players.Sort((a, b) => a.netId.CompareTo(b.netId));

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < players.Count)
            {
                slots[i].root.SetActive(true);

                slots[i].nicknameText.text = players[i].nickname;

                int avatar = players[i].avatarIndex;
                if (avatar >= 0 && avatar < avatarSprites.Length)
                    slots[i].avatarImage.sprite = avatarSprites[avatar];

                // Корона у хоста
                slots[i].crownIcon.SetActive(players[i].isServer);
            }
            else
            {
                slots[i].root.SetActive(false);
            }
        }

        // Кнопка старта только у хоста
        startButton.gameObject.SetActive(NetworkServer.active);
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
            NetworkManager.singleton.StopHost();
        else
            NetworkManager.singleton.StopClient();
    }
}