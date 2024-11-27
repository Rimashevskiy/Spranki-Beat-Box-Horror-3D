using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using DarkFlameDX.Editor;

public class Editor_Tools : Editor
{
    #region Toolbar
    [MenuItem("Tools/Saves/Clear")]
    public static void ClearAllSaves()
    {
        PlayerPrefs.DeleteAll();

        File.Delete(Configuration.FILE_PATCH);
    }

    [MenuItem("Tools/Open Config", false, 0)]
    public static void OpenConfig()
    {
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GetAssetPath(Resources.Load<Object>("Configuration")));
    }

    [MenuItem("Tools/Open Saves")]
    public static void OpenSaves()
    {
        OpenInFileBrowser.Open(Application.persistentDataPath);

        Debug.Log($"Saves path: open {Application.persistentDataPath.Replace(" ", "\\ ") + "/data"}");
    }

    #endregion
}
