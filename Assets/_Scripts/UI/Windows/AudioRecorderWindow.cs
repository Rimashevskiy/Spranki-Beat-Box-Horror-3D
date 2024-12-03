
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AudioRecorderWindow : WindowCore
{
    #region Constants
    private const int AUDIO_SAMPLE_RATE = 44100;
    #endregion

    #region Variables
    [Header("Player Settings")]
    [SerializeField] private Slider _previewProgress;
    [SerializeField] private GameObject _baseButtons;
    [SerializeField] private GameObject _playButton;
    [SerializeField] private GameObject _stopButton;

    [Header("Recording Settings")]
    [SerializeField] private TextMeshProUGUI _recordingTimeLabel;
    [SerializeField] private GameObject _recordingButtons;

    [Header("Preview Settings")]
    [SerializeField] private RectTransform _waveformContainer;
    [SerializeField] private float _waveformScale = 100f;

    private Image _waveformBarPrefab;
    private int _waveformResolution = 100;
    private AudioClip _recordingClip;
    private bool _isRecording = false;
    private AudioSource _audioPlayer;
    private SoundPadItem _targetSoundItem;
    private MenuWindow.GameModeType _targetGameMode => Interface.Instance.Windows.Menu.GameMode;
    #endregion

    #region UnityMethods

    #endregion

    #region Renderer
    public void Show(SoundPadItem targetSoundItem)
    {
        if (!_waveformBarPrefab)
        {
            _waveformBarPrefab = Resources.Load<GameObject>("Prefabs/UI/WaveformPart").GetComponent<Image>();
            _audioPlayer = GetComponent<AudioSource>();
        }

        _audioPlayer.clip = null;
        _targetSoundItem = targetSoundItem;

        _baseButtons.SetActive(true);
        _recordingButtons.SetActive(false);
        _playButton.SetActive(true);
        _stopButton.SetActive(false);

        UpdatePreviewProgress(0f, 0f);

        base.Show();

        ClearWaveform();

        StartCoroutine(LoadAndPlayAudio(() => DrawWaveform(_audioPlayer.clip)));
    }

    public override void Hide()
    {
        base.Hide();

        ClearWaveform();

        _targetSoundItem.SetCustomAudio(_audioPlayer.clip);
    }
    #endregion

    #region Remove
    public void RemoveSound()
    {
        string file = GetAudioSavePath(_targetGameMode, _targetSoundItem.Info.Type);

        if (File.Exists(file))
            File.Delete(file);

        _targetSoundItem.SetCustomAudio(null);

        ClearWaveform();

        base.Hide();
    }
    #endregion

    #region Preview
    private void DrawWaveform(AudioClip clip)
    {
        if (clip == null || _waveformContainer == null || _waveformContainer == null)
        {
            Debug.Log("Waveform visualization dependencies are not set up!");
            return;
        }

        ClearWaveform();

        float partWidth = 1;
        // Расчет количества линий на основе ширины контейнера
        float containerWidth = _waveformContainer.rect.width;
        _waveformResolution = Mathf.FloorToInt(containerWidth / partWidth); // Каждая линия — ширина 2 пикселя

        // Получаем данные из AudioClip
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        int step = Mathf.CeilToInt(samples.Length / (float)_waveformResolution);

        for (int i = 0; i < _waveformResolution; i++)
        {
            float sampleValue = Mathf.Abs(samples[i * step]);

            // Создаем новый бар
            Image bar = Instantiate(_waveformBarPrefab, _waveformContainer);
            RectTransform barRect = bar.GetComponent<RectTransform>();

            float normalizedPosition = i / (float)(_waveformResolution - partWidth);

            // Устанавливаем якоря и позицию
            barRect.anchorMin = new Vector2(normalizedPosition, 0.5f);
            barRect.anchorMax = new Vector2(normalizedPosition, 0.5f);
            barRect.pivot = new Vector2(0.5f, 0.5f);

            // Устанавливаем высоту, растягивая вверх и вниз
            barRect.sizeDelta = new Vector2(partWidth, Mathf.Clamp(sampleValue * _waveformScale, 1f, Mathf.Infinity));
        }
    }

    private void ClearWaveform()
    {
        DestroyAllChildren(_waveformContainer);
    }
    #endregion

    #region Playing
    public void PlayStopClick()
    {
        if (_audioPlayer.isPlaying)
            Stop();
        else
            Play();
    }

    private void Stop()
    {
        StopAllCoroutines();

        _audioPlayer.Stop();

        _playButton.SetActive(true);
        _stopButton.SetActive(false);

        UpdatePreviewProgress(0f, 0f);
    }

    private void Play()
    {
        if (!File.Exists(GetAudioSavePath(_targetGameMode, _targetSoundItem.Info.Type)))
        {
            Debug.Log("Recording file not found!");
            return;
        }

        UpdatePreviewProgress(0f, 0f);
        _playButton.SetActive(false);
        _stopButton.SetActive(true);

        StartCoroutine(LoadAndPlayAudio(() => _audioPlayer.Play()));
    }

    private IEnumerator LoadAndPlayAudio(UnityAction callback)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + GetAudioSavePath(_targetGameMode, _targetSoundItem.Info.Type), AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error loading audio: " + www.error + " | " + GetAudioSavePath(_targetGameMode, _targetSoundItem.Info.Type));
                yield break;
            }

            AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
            _audioPlayer.clip = clip;

            callback?.Invoke();

            while (_audioPlayer.isPlaying)
            {
                UpdatePreviewProgress(_audioPlayer.time / _audioPlayer.clip.length, _audioPlayer.time);
                yield return null;
            }

            Stop();
        }
    }

    private void UpdatePreviewProgress(float value, float fullValue)
    {
        _previewProgress.value = value;

        _recordingTimeLabel.text = $"{FormatCountdown(fullValue)}";
    }
    #endregion

    #region Recording
    public void CancelRecording()
    {
        _isRecording = false;

        _baseButtons.SetActive(true);
        _recordingButtons.SetActive(false);

        Microphone.End(null);

        StopAllCoroutines();

        UpdatePreviewProgress(0f, 0f);
        ClearWaveform();

        StartCoroutine(LoadAndPlayAudio(() => DrawWaveform(_audioPlayer.clip)));
    }

    public void StartRecording()
    {
        if (_isRecording)
        {
            Debug.Log("Already recording!");
            return;
        }

        if (Microphone.devices.Length == 0)
        {
            Debug.Log("No microphone detected!");
            return;
        }

        _isRecording = true;

        _baseButtons.SetActive(false);
        _recordingButtons.SetActive(true);

        UpdatePreviewProgress(0f, 0f);
        ClearWaveform();

        _recordingClip = Microphone.Start(null, false, 5, AUDIO_SAMPLE_RATE);
        StartCoroutine(StopRecordingAfterDelay(5f));
    }

    private IEnumerator StopRecordingAfterDelay(float delay)
    {
        for (float t = 0; t < delay; t += Time.deltaTime)
        {
            UpdatePreviewProgress(t / delay, t);

            yield return null;
        }

        UpdatePreviewProgress(1f, 0f);

        if (_isRecording)
            StopRecording();
    }

    public void StopRecording()
    {
        if (!_isRecording)
        {
            Debug.Log("Not recording!");
            return;
        }

        _isRecording = false;

        _baseButtons.SetActive(true);
        _recordingButtons.SetActive(false);

        Microphone.End(null);

        SaveRecordingToFile();

        StartCoroutine(LoadAndPlayAudio(() => DrawWaveform(_audioPlayer.clip)));
    }
    #endregion

    #region Tools
    private byte[] ConvertToWav(float[] samples, int channels, int sampleRate)
    {
        int byteLength = samples.Length * sizeof(short);
        byte[] wav = new byte[44 + byteLength];

        System.Buffer.BlockCopy(new byte[]
        {
            0x52, 0x49, 0x46, 0x46,
            0, 0, 0, 0,
            0x57, 0x41, 0x56, 0x45,
            0x66, 0x6D, 0x74, 0x20,
            16, 0, 0, 0,
            1, 0,
            (byte)channels, 0,
            (byte)(sampleRate & 0xFF), (byte)((sampleRate >> 8) & 0xFF), (byte)((sampleRate >> 16) & 0xFF), (byte)((sampleRate >> 24) & 0xFF),
            (byte)((sampleRate * channels * 2) & 0xFF), (byte)(((sampleRate * channels * 2) >> 8) & 0xFF), 0, 0,
            (byte)(channels * 2), 0,
            16, 0,
            0x64, 0x61, 0x74, 0x61,
            0, 0, 0, 0
        }, 0, wav, 0, 44);

        short[] intData = new short[samples.Length];
        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * short.MaxValue);
        }

        System.Buffer.BlockCopy(intData, 0, wav, 44, byteLength);

        int fileSize = 44 + byteLength - 8;
        System.Buffer.BlockCopy(System.BitConverter.GetBytes(fileSize), 0, wav, 4, 4);

        int dataChunkSize = byteLength;
        System.Buffer.BlockCopy(System.BitConverter.GetBytes(dataChunkSize), 0, wav, 40, 4);

        return wav;
    }

    private string FormatCountdown(float totalSeconds)
    {
        int seconds = Mathf.FloorToInt(totalSeconds);
        int milliseconds = Mathf.FloorToInt((totalSeconds - seconds) * 100);
        string formattedTime = string.Format("{0:D2}:{1:D2}", seconds, milliseconds);

        return formattedTime;
    }

    private void DestroyAllChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
            GameObject.Destroy(parent.GetChild(i).gameObject);
    }
    #endregion

    #region Save
    public static string GetAudioSavePath(MenuWindow.GameModeType mode, Character.Type type)
    {
        string directoryPath = Path.Combine(Application.persistentDataPath, "Recordings", mode.ToString());

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        return Path.Combine(directoryPath, $"{type}.wav");
    }

    private void SaveRecordingToFile()
    {
        if (_recordingClip == null)
        {
            Debug.Log("No recording to save!");
            return;
        }

        float[] samples = new float[_recordingClip.samples * _recordingClip.channels];
        _recordingClip.GetData(samples, 0);

        byte[] wavData = ConvertToWav(samples, _recordingClip.channels, _recordingClip.frequency);

        File.WriteAllBytes(GetAudioSavePath(_targetGameMode, _targetSoundItem.Info.Type), wavData);
    }
    #endregion
}
