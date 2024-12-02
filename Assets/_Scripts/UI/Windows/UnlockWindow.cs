using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockWindow : WindowCore
{
    #region Variables
    [Header("Complete Settings")]
    [SerializeField] private GameObject _completePanel;
    [SerializeField] private ParticleSystem[] _completeEffect;
    #endregion

    #region Renderer
    public override void Show()
    {
        _completePanel.SetActive(false);

        StopCompleteEffect();

        base.Show();
    }
    #endregion

    #region Complete
    public void ShowComplete()
    {
        _completePanel.SetActive(true);

        PlayCompleteEffect();
    }
    #endregion

    #region Buttons
    public void BackClick()
    {
        UnlockGame.Instance.Hide();

        _completePanel.SetActive(false);

        StopCompleteEffect();
    }

    public void FinishClick()
    {
        BackClick();
    }
    #endregion

    #region Effects
    private void PlayCompleteEffect()
    {
        for (int i = 0; i < _completeEffect.Length; i++)
            _completeEffect[i].Play();
    }

    private void StopCompleteEffect()
    {
        for (int i = 0; i < _completeEffect.Length; i++)
            _completeEffect[i].Stop();
    }
    #endregion
}
