using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : Singleton<Map>
{
    #region Variables
    [Header("Location Settings")]
    [SerializeField] private Transform _locationRoot;

    public GameplayLocation CurrentLocation { get; private set; }
    #endregion

    #region UnityMethods

    #endregion

    #region Location
    public void LoadLocation(MenuWindow.GameModeType gameMode)
    {
        UnloadLocation();

        if (gameMode == MenuWindow.GameModeType.Custom)
            gameMode = MenuWindow.GameModeType.Cute;

        CurrentLocation = Instantiate(Resources.Load<GameObject>($"Prefabs/Mods/{gameMode}"), _locationRoot).GetComponentInChildren<GameplayLocation>();
        CurrentLocation.transform.SetLocalPositionAndRotation(Vector3.zero, new Quaternion());

        BackgroundManager.Instance.SetBackground(gameMode);
    }

    public void UnloadLocation()
    {
        if (CurrentLocation)
            Destroy(CurrentLocation.gameObject);
    }
    #endregion
}
