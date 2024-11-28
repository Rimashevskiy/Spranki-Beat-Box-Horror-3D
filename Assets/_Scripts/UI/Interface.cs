using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : Singleton<Interface>
{
    #region Variables
    [Header("Windows Settings")]
    [SerializeField] private WindowsPreset _windows;

    public WindowsPreset Windows => _windows;
    #endregion

    #region UnityMethods
    private void Start()
    {
        Windows.Settings.LoadSettings();
    }
    #endregion

    #region Structs
    [System.Serializable]
    public struct WindowsPreset
    {
        public MenuWindow Menu;
        public GameWindow Game;
        public SettingsWindow Settings;
        public UnlockWindow Unlock;
    }
    #endregion
}
