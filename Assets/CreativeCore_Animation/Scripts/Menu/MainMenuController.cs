using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;

    [Header("Settings Sliders")]
    public Slider volumeSlider;
    public Slider playerSpeedSlider;
    public Slider enemySpeedSlider;

    [Header("Slider Labels")]
    public TextMeshProUGUI volumeLabel;
    public TextMeshProUGUI playerSpeedLabel;
    public TextMeshProUGUI enemySpeedLabel;

    [Header("BGM")]
    public AudioSource bgmSource;

    [Header("Click")]
    public AudioSource clickSound;

    void Start()
    {
        // Load saved settings or use defaults
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.75f);
        playerSpeedSlider.value = PlayerPrefs.GetFloat("PlayerSpeed", 2f);
        enemySpeedSlider.value = PlayerPrefs.GetFloat("EnemySpeed", 3.5f);

        // Set slider ranges
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;
        playerSpeedSlider.minValue = 1f;
        playerSpeedSlider.maxValue = 10f;
        enemySpeedSlider.minValue = 1f;
        enemySpeedSlider.maxValue = 10f;

        // Apply saved volume to BGM
        if (bgmSource != null)
            bgmSource.volume = volumeSlider.value;

        UpdateLabels();
        ShowMain();
    }

    // Button functions
    public void OnStartClicked()
    {
        clickSound.Play();
        SaveSettings();
        if (Time.timeScale == 0) Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }

    public void OnSettingsClicked()
    {
        clickSound.Play();
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnBackClicked()
    {
        clickSound.Play();
        SaveSettings();
        ShowMain();
    }

    public void OnExitClicked()
    {
        clickSound.Play();
        Application.Quit();
        Debug.Log("Game Exited");
    }

    // Slider callbacks
    public void OnVolumeChanged()
    {
        if (bgmSource != null)
            bgmSource.volume = volumeSlider.value;
        UpdateLabels();
    }

    public void OnPlayerSpeedChanged()
    {
        UpdateLabels();
    }

    public void OnEnemySpeedChanged()
    {
        UpdateLabels();
    }

    void ShowMain()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    void UpdateLabels()
    {
        if (volumeLabel != null)
            volumeLabel.text = "Volume: " + Mathf.Round(volumeSlider.value * 100) + "%";
        if (playerSpeedLabel != null)
            playerSpeedLabel.text = "Player Speed: " + playerSpeedSlider.value.ToString("F1");
        if (enemySpeedLabel != null)
            enemySpeedLabel.text = "Enemy Speed: " + enemySpeedSlider.value.ToString("F1");
    }

    void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetFloat("PlayerSpeed", playerSpeedSlider.value);
        PlayerPrefs.SetFloat("EnemySpeed", enemySpeedSlider.value);
        PlayerPrefs.Save();
    }
}