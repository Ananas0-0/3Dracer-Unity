using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LobbyLevelSelector : MonoBehaviour
{
    public GameObject[] pages; // levels(1), levels(2)
    public Button nextButton;
    public Button previousButton;

    private int pageIndex = 0;

    void Start()
    {
        UpdatePages();
    }

    public void NextPage()
    {
        if (pageIndex < pages.Length - 1)
        {
            pageIndex++;
            UpdatePages();
        }
    }

    public void PreviousPage()
    {
        if (pageIndex > 0)
        {
            pageIndex--;
            UpdatePages();
        }
    }

    void UpdatePages()
    {
        for (int i = 0; i < pages.Length; i++)
            pages[i].SetActive(i == pageIndex);
    }
}