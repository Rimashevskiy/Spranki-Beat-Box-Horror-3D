using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : Singleton<BackgroundManager>
{
    #region Variables
    [Header("Main Settings")]
    [SerializeField] private Transform _backgroundRoot;

    private GameObject _currentBackground;
    #endregion

    #region Methods
    public void SetBackground(MenuWindow.GameModeType background)
    {
        ClearBackground();

        _currentBackground = Instantiate(Resources.Load<GameObject>($"Prefabs/Backgrounds/Background_{background}"), _backgroundRoot);
        _currentBackground.transform.SetLocalPositionAndRotation(Vector2.zero, new Quaternion());
    }

    public void ClearBackground()
    {
        if (_currentBackground)
            Destroy(_currentBackground);
    }
    #endregion
}
