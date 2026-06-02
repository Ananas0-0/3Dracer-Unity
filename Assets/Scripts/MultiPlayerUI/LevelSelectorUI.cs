using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorUI : MonoBehaviour
{
    [Header("Pages")]
    [SerializeField] private GameObject[] levelPages; // 0 = page1, 1 = page2
    private int currentPage = 0;

    [Header("Selection")]
    [SerializeField] private GameObject selectLevelsPanel; // окно выбора
    [SerializeField] private Image previewImage; // большая картинка
    [SerializeField] private Sprite[] levelSprites; // 5 спрайтов
    [SerializeField] private string[] sceneNames; // 5 имен сцен

    private string selectedScene;

    private void Start()
    {
        UpdatePages();
        selectedScene = sceneNames[0];
        previewImage.sprite = levelSprites[0];
    }

    // ---------- PAGE SWITCH ----------

    public void NextPage()
    {
        currentPage++;
        if (currentPage >= levelPages.Length)
            currentPage = 0;

        UpdatePages();
    }

    public void PrevPage()
    {
        currentPage--;
        if (currentPage < 0)
            currentPage = levelPages.Length - 1;

        UpdatePages();
    }

    private void UpdatePages()
    {
        for (int i = 0; i < levelPages.Length; i++)
        {
            levelPages[i].SetActive(i == currentPage);
        }
    }

    // ---------- LEVEL SELECT ----------

    public void SelectLevel(int index)
    {
        selectedScene = sceneNames[index];
        previewImage.sprite = levelSprites[index];

        selectLevelsPanel.SetActive(false); // закрываем окно выбора
    }

    public string GetSelectedScene() { return selectedScene; }

    public void OpenSelectPanel() { selectLevelsPanel.SetActive(true); }
}