using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWindow : WindowCore
{
    #region Variables
    public SoundPad MainSoundPad { get; private set; }
    #endregion

    #region UnityMethods
    private void Awake()
    {
        MainSoundPad = GetComponentInChildren<SoundPad>();
    }
    #endregion
}
