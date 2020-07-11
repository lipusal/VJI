using System;
using Game.Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainScreen, settingsScreen, infoScreen, statsScreen;
    public TextMeshProUGUI setsTitle, gamesPerSetTitle, hitForceTitle, serveForceTitle, speedTitle, presetName, presetWarning;
    public Slider hitForceSlider, serveForceSlider, speedSlider;
    public Button playButton;
    private int _presetIndex = 0;
    private bool _validStats = true;

    private void Start()
    {
        ChangePreset(0);
    }

    private void Update()
    {
        setsTitle.SetText($"Sets to play: {ScoreManager.GetInstance().MaxSets}");
        gamesPerSetTitle.SetText($"Games per set: {ScoreManager.GetInstance().GamesPerSet}");
        hitForceTitle.SetText($"Hit force: {PlayerStats.GetInstance().HitForce}");
        serveForceTitle.SetText($"Serve force: {PlayerStats.GetInstance().ServeForce}");
        speedTitle.SetText($"Speed: {PlayerStats.GetInstance().Speed}");
    }

    public void OnPlayClick(bool play2Players)
    {
        // Initialize some stuff
        ScoreManager scoreManager = ScoreManager.GetInstance();
        scoreManager.ResetScore();
        scoreManager.SetGameMode(play2Players);
        if (play2Players)
        {
            // Use default stats for 2 player mode
            PlayerStats.GetInstance().Reset();
        }
        else
        {
            scoreManager.SetGameDifficulty(1);
        }
        SceneManager.LoadScene("TenisMatch");
    }

    public void OnPlayPcClick()
    {
        GoToStats();
    }
    
    public void OnSettingsClick()
    {
        GoToSettings();
    }

    public void OnInfoClick()
    {
        GoToInfo();
    }
    
    public void OnBackClick()
    {
        GoToMainScreen();
    }

    public void OnNextPresetClick()
    {
        ChangePreset(1);
    }
    
    public void OnPrevPresetClick()
    {
        ChangePreset(-1);
    }

    public void OnQuitClick()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    private void ChangePreset(int delta)
    {
        int presetSize = StatsPreset.defaultPresets.Length;
        // New index has to be circular
        int newIndex = _presetIndex + delta;
        if (newIndex < 0) {
            newIndex += presetSize;
        }
        newIndex %= presetSize;

        _presetIndex = newIndex;
        StatsPreset preset = StatsPreset.defaultPresets[_presetIndex];

        presetName.SetText(preset.name);
        // Clear stats to avoid validation issues
        PlayerStats.GetInstance().Zero();
        // These slider updates will trigger the onValueChangeCallback, which will call SetHitForce, etc.
        // For some reason, directly setting values doesn't work. See https://answers.unity.com/questions/859625/unity-46-2-ui-sliders-updating-will-not-work.html
        float temp = preset.serveForce;
        serveForceSlider.value = temp;
        temp = preset.hitForce;
        hitForceSlider.value = temp;
        temp = preset.speed;
        speedSlider.value = temp;
    }

    /// <summary>
    /// Set the human player's hit force.
    /// </summary>
    /// <param name="hitForce">Hit force. Specified as float so the UI can hook into this, but UI should only send whole numbers.</param>
    public void SetHitForce(float hitForce)
    {
        PlayerStats.GetInstance().HitForce = hitForce;
        ValidateStats();
    }
    
    /// <summary>
    /// Set the human player's serve force.
    /// </summary>
    /// <param name="serveForce">Serve force. Specified as float so the UI can hook into this, but UI should only send whole numbers.</param>
    public void SetServeForce(float serveForce)
    {
        PlayerStats.GetInstance().ServeForce = serveForce;
        ValidateStats();
    }

    /// <summary>
    /// Set the human player's speed.
    /// </summary>
    /// <param name="speed">Speed. Specified as float so the UI can hook into this, but UI should only send whole numbers.</param>
    public void SetSpeed(float speed)
    {
        PlayerStats.GetInstance().Speed = speed;
        ValidateStats();
    }
    
    public void SetMaxSets(int maxSets)
    {
        ScoreManager.GetInstance().MaxSets = maxSets;
    }
    
    public void SetGamesPerSet(int gamesPerSet)
    {
        ScoreManager.GetInstance().GamesPerSet = gamesPerSet;
    }

    private void GoToStats()
    {
        statsScreen.SetActive(true);
        mainScreen.SetActive(false);
    }
    
    private void GoToSettings()
    {
        settingsScreen.SetActive(true);
        mainScreen.SetActive(false);
    }

    private void GoToInfo()
    {
        infoScreen.SetActive(true);
        mainScreen.SetActive(false);
    }
    
    private void GoToMainScreen()
    {
        settingsScreen.SetActive(false);
        infoScreen.SetActive(false);
        statsScreen.SetActive(false);
        mainScreen.SetActive(true);
    }

    /// <summary>
    /// Validate that the sum of configured stats does not exceed the configured maximum.  If exceeded, enable a warning
    /// and disable the play button.
    /// </summary>
    private void ValidateStats()
    {
        if (PlayerStats.GetInstance().GetTotal() > PlayerStats.MaxStatSum)
        {
            _validStats = false;
            presetWarning.enabled = true;
            playButton.enabled = false;
        }
        else
        {
            _validStats = true;
            presetWarning.enabled = false;
            playButton.enabled = true;
        }
    }
}
