using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LobbyLevelSelector : MonoBehaviour
{
    public GameObject[] levelPanels;
    public Button nextButton;
    public Button previousButton;

    void Start()
    {
        UpdatePanels(LobbyState.Instance.selectedMapIndex);
        UpdateButtons(LobbyState.Instance.selectedMapIndex);

        bool isHost = NetworkServer.active;
        nextButton.interactable = isHost;
        previousButton.interactable = isHost;
    }

    public void Next()
    {
        if (!NetworkServer.active) return;

        int index = LobbyState.Instance.selectedMapIndex;

        if (index < levelPanels.Length - 1)
        {
            LobbyState.Instance.SetMap(index + 1);
        }
    }

    public void Previous()
    {
        if (!NetworkServer.active) return;

        int index = LobbyState.Instance.selectedMapIndex;

        if (index > 0)
        {
            LobbyState.Instance.SetMap(index - 1);
        }
    }

    public void UpdatePanels(int index)
    {
        for (int i = 0; i < levelPanels.Length; i++)
        {
            levelPanels[i].SetActive(i == index);
        }
    }

    public void UpdateButtons(int index)
    {
        bool isHost = NetworkServer.active;

        nextButton.interactable = isHost && index < levelPanels.Length - 1;
        previousButton.interactable = isHost && index > 0;
    }
}