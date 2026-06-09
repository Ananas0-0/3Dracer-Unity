using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Graphics")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Toggle vsyncToggle;
    public Toggle fullscreenToggle;

    [Header("Nickname")]
    public TMP_InputField nicknameInput;

    private Resolution[] resolutions;

    void Start()
    {
        SetupResolutions();
        LoadSettings();
    }

    void SetupResolutions()
    {
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        resolutions = Screen.resolutions;

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option =
                resolutions[i].width + "x" +
                resolutions[i].height + " " +
                resolutions[i].refreshRate + "Hz";

            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetFullscreen(bool isFullscreen) { Screen.fullScreen = isFullscreen; }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex) { QualitySettings.SetQualityLevel(qualityIndex); }

    public void SetVSync(bool isVSyncOn) { QualitySettings.vSyncCount = isVSyncOn ? 1 : 0; }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", System.Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetInt("VSyncPreference", System.Convert.ToInt32(vsyncToggle.isOn));

        string nickname = nicknameInput.text.Trim();

        if (string.IsNullOrEmpty(nickname))
            nickname = "Player";

        if (nickname.Length > 16)
            nickname = nickname.Substring(0, 16);

        PlayerPrefs.SetString("PlayerNickname", nickname);

        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference", 3);
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference", resolutionDropdown.value);
        Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference", 1));
        vsyncToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("VSyncPreference", 1));

        SetVSync(vsyncToggle.isOn);

        if (PlayerPrefs.HasKey("PlayerNickname"))
            nicknameInput.text = PlayerPrefs.GetString("PlayerNickname");
        else
            nicknameInput.text = "Player";
    }
}
