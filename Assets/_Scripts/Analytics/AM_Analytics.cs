using System.Collections;
using System.Collections.Generic;
using Io.AppMetrica;
using Newtonsoft.Json;
using UnityEngine;

public class AM_Analytics : MonoBehaviour
{
    #region Variables
    public static float StartGameTime { get; set; }
    #endregion

    #region Methods
    public static void OnStartGame()
    {
        Dictionary<string, object> prms = new Dictionary<string, object>
        {
            {$"{1}", $"{1}" }
        };

        string value = JsonConvert.SerializeObject(prms);
        string key = "start_game";

        AppMetrica.ReportEvent(key, value);

        Log(key, value);
    }
    #endregion

    #region Tools
    public static void Log(string key, string value = "Empty")
    {
        Debug.Log($"<color=green>[Analytics] >> {key} : {value}</color>");
    }

    public static int RoundTime(float startTime, float endTime)
    {
        float time = endTime - startTime;

        return Mathf.Abs(Mathf.CeilToInt(time / 10.0f) * 10);
    }
    #endregion
}
