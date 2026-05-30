using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int levelUnlock;
    public Button[] buttons;
    // Start is called before the first frame update
    private void Start()
    {
        levelUnlock = PlayerPrefs.GetInt("levels", 1);

        for (int i = 0; i < buttons.Length; i++) { buttons[i].interactable = false; }
        for (int i = 0; i < levelUnlock; i++) { buttons[i].interactable = true; }
    }

    public void LoadLevel(int levelIndex) {
        SceneTransition.SwitchToScene(levelIndex);
    }
    public void resetBtnPressed() {
        PlayerPrefs.DeleteKey("levels");
        levelUnlock = PlayerPrefs.GetInt("levels", 1);
        PlayerPrefs.Save();
    }
}
