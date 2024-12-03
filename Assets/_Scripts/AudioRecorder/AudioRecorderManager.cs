using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Recorder;

[RequireComponent(typeof(CanvasGroupFader))]
public class AudioRecorderManager : MonoBehaviour
{
    public static AudioRecorderManager Instance { get; private set; }

    [SerializeField] private AudioRecorder _audio_recorder;
    [SerializeField] private AudioPlayer _audio_player;

    #region Private Variables
    private CanvasGroupFader _canvasGroupFader;
    #endregion

    private void Awake()
    {
        // Реализация синглтона
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        //assign components to variables
        _canvasGroupFader = GetComponent<CanvasGroupFader>();
    }

    public void OnRecorderWindowOpen(Character.Type character_type)
    {
        _audio_recorder.UpdateRecordWindow(character_type);
    }
}
