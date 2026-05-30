using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class WinPanel : MonoBehaviour
{
    public GameObject winPanel;
    public AudioSource winAudio;
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            UnlockLevel();
            Time.timeScale = 0.0f;
            winPanel.SetActive(true);
            // SceneManager.LoadScene(0);
            winAudio.Play();
        }
    }

    public void UnlockLevel() {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        if (currentLevel >= PlayerPrefs.GetInt("levels")) {
            PlayerPrefs.SetInt("levels", currentLevel + 1);
        }
    }
    
}




// Time.timeScale = 0.0f;
//winPanel.SetActive(true);
//winAudio.Play();