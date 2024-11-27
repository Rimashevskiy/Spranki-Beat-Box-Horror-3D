using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuModeItem : MonoBehaviour
{
    #region Variables
    [Header("Main Settings")]
    [SerializeField] private MenuWindow.GameModeType _mode;
    #endregion

    #region Methods
    public void Select()
    {
        Map.Instance.LoadLocation(_mode);

        Interface.Instance.Windows.Menu.Hide();
        Interface.Instance.Windows.Game.Show();
    }
    #endregion
}
