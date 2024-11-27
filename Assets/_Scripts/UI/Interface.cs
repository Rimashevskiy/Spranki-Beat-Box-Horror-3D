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

    #region Structs
    [System.Serializable]
    public struct WindowsPreset
    {
        public MenuWindow Menu;
        public GameWindow Game;
    }
    #endregion
}
