using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    #region Variables
    [Header("Main Settings")]
    [SerializeField] private Camera _mainCamera;

    public Camera MainCamera => _mainCamera;
    #endregion

    #region Active
    public void EnableCamera()
    {
        _mainCamera.enabled = true;
    }

    public void DisableCamera()
    {
        _mainCamera.enabled = false;
    }
    #endregion
}
