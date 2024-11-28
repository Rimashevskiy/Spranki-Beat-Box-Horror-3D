using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Saves
{
    #region Variables
    public static float MusicVolume;
    #endregion

    #region Constructor
    static Saves()
    {
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);

        GameStatusHelper.Create();
    }

    public static void Save()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
    }
    #endregion
}

public class GameStatusHelper : MonoBehaviour
{
    #region Variables
    public static bool Created { get; private set; }

    private const string OBJECT_NAME = "[Saves.Helper]";
    #endregion

    #region Constructor
    public static void Create()
    {
        if (Created)
            return;

        GameObject tempHelper = new GameObject(OBJECT_NAME);
        tempHelper.AddComponent<GameStatusHelper>();

        Created = true;
    }
    #endregion

    #region UnityMethods
    private void OnApplicationQuit()
    {
        Saves.Save();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            OnApplicationQuit();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            OnApplicationQuit();
    }

    private void OnDestroy()
    {
        OnApplicationQuit();
    }
    #endregion
}