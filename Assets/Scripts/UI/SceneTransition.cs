using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneTransition : MonoBehaviour
{
    public TMP_Text LoadingPercentage;
    public Image LoadingProgressBar;
    
    private static SceneTransition instance;
    private static bool shouldPlayOpeningAnimation = false; 
    
    private Animator componentAnimator;
    private AsyncOperation loadingSceneOperation;

    private static bool isSceneSwitching = false;

    public static void SwitchToScene(int sceneName)
    {
        if (isSceneSwitching)
            return;

        isSceneSwitching = true;
        
        instance.componentAnimator.SetTrigger("sceneClosing");

        instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        
        // Чтобы сцена не начала переключаться пока играет анимация closing:
        instance.loadingSceneOperation.allowSceneActivation = false;
        
        instance.LoadingProgressBar.fillAmount = 0;
    }
    
    private void Start()
    {
        instance = this;
        
        componentAnimator = GetComponent<Animator>();
        
        if (shouldPlayOpeningAnimation) 
        {
            componentAnimator.SetTrigger("sceneOpening");
            instance.LoadingProgressBar.fillAmount = 1;
            
            // Чтобы если следующий переход будет обычным SceneManager.LoadScene, не проигрывать анимацию opening:
            shouldPlayOpeningAnimation = false; 
        }

        isSceneSwitching = false;
    }

    private void Update()
    {
        if (loadingSceneOperation != null)
        {
            // Calculate progress considering the range [0, 0.9] for loading progress
            float progress = Mathf.Clamp01(loadingSceneOperation.progress / 0.9f); // Normalize progress to be between 0 and 1
            LoadingPercentage.text = Mathf.RoundToInt(progress * 100) + "%";
            
            // Просто присвоить прогресс:
            LoadingProgressBar.fillAmount = progress; 
            
            // Присвоить прогресс с быстрой анимацией, чтобы ощущалось плавнее:
            //LoadingProgressBar.fillAmount = Mathf.Lerp(LoadingProgressBar.fillAmount, progress, Time.deltaTime * 5);
        }
    }

    public void OnAnimationOver()
    {
        // Чтобы при открытии сцены, куда мы переключаемся, проигралась анимация opening:
        shouldPlayOpeningAnimation = true;
        
        loadingSceneOperation.allowSceneActivation = true;

        // Убедиться, что прогресс-бар заполнен полностью при активации сцены
        LoadingProgressBar.fillAmount = 1.0f;
    }
}
