using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    #region Variables
    [Header("Main Settings")]
    [SerializeField] private bool _canGotToMenu;
    #endregion

    #region Methods
    public void Click()
    {
        Interface.Instance.Windows.Settings.Show(_canGotToMenu);
    }
    #endregion
}
