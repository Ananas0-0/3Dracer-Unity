using UnityEngine;

public class LobbyMenuController : MonoBehaviour
{
    public static LobbyMenuController Instance;

    [SerializeField] private GameObject joinPanel;
    [SerializeField] private GameObject hostPanel;
    [SerializeField] private GameObject onlineChoicePanel;

    void Awake()
    {
        Instance = this;
    }

    public void ShowLobby()
    {
        joinPanel.SetActive(false);
        hostPanel.SetActive(true);
        onlineChoicePanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        joinPanel.SetActive(false);
        hostPanel.SetActive(false);
        onlineChoicePanel.SetActive(true);
    }
}