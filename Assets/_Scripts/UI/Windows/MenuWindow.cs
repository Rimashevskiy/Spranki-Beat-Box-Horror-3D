using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWindow : WindowCore
{
    #region Variables
    public GameModeType GameMode { get; set; }
    #endregion

    #region Enums
    public enum GameModeType
    {
        Cute,
        Scary,
        Custom
    }
    #endregion
}
