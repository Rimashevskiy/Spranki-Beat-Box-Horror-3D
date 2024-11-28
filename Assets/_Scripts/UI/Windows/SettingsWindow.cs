using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : WindowCore
{
    #region Variables
    [Header("Music Settings")]
    [SerializeField] private Slider _musicVolume;

    [Header("Buttons Settings")]
    [SerializeField] private GameObject _goToMenuButton;
    #endregion

    #region Methods
    public void Show(bool canGoToMenu)
    {
        base.Show();

        _goToMenuButton.SetActive(canGoToMenu);
    }

    public void LoadSettings()
    {
        _musicVolume.value = Saves.MusicVolume;

        AudioListener.volume = Saves.MusicVolume;
    }

    public void UpdateMusicVolume()
    {
        Saves.MusicVolume = _musicVolume.value;

        AudioListener.volume = Saves.MusicVolume;
    }

    public void GoToMenu()
    {
        Map.Instance.UnloadLocation();

        Interface.Instance.Windows.Game.Hide();
        Interface.Instance.Windows.Menu.Show();

        Hide();
    }
    #endregion
}
