using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    #region Variables
    [Header("Renderer Settings")]
    [SerializeField] private Image _playingProgress;
    #endregion

    #region UnityMethods
    private void Update()
    {
        UpdatePlayingProgress();
    }
    #endregion

    #region Playing
    public void UpdatePlayingProgress()
    {
        _playingProgress.fillAmount = Map.Instance.CurrentLocation.Characters.Count > 0 ? 1f - (Map.Instance.CurrentLocation.CurrentMusicTime / GameplayLocation.MUSIC_SQUARE_LENGTH) : 0f;
    }

    public void Restart()
    {
        Map.Instance.CurrentLocation.Restart();
    }
    #endregion
}
