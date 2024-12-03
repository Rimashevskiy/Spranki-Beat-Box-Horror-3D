using System.Collections;
using System.Collections.Generic;
using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.UI;

public class SoundPadItem : MonoBehaviour
{
    #region Variables
    [Header("Main Settings")]
    [SerializeField] private Character.Type _characterType;

    [Header("Drag Settings")]
    [SerializeField] private RectTransform _dragPanel;

    [Header("Locked Settings")]
    [SerializeField] private GameObject _lockedPanel;

    [Header("Renderer Settings")]
    [SerializeField] private Image[] _backgroundHolder;
    [SerializeField] private Image[] _iconHolder;

    [Header("Recording Settings")]
    [SerializeField] private Toggle _hasRecording;

    public RectTransform DragPanel => _dragPanel;
    public Character.Type CharacterType => _characterType;
    public bool Used { get; private set; }
    public Configuration.CharacterDataPreset Info { get; private set; }
    public AudioClip CustomAudio { get; private set; }
    public bool Unlocked
    {
        get => !Info.LockedOnStart || PlayerPrefs.GetInt($"CharacterUnlocked_{Info.Mode}_{Info.Type}", 0) == 1;
        set => PlayerPrefs.SetInt($"CharacterUnlocked_{Info.Mode}_{Info.Type}", value ? 1 : 0);
    }

    private UIEffect[] _colorFilters;
    private CanvasGroup _backgroundAlpha;
    private float _startDragTime;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        _colorFilters = GetComponentsInChildren<UIEffect>(true);
        _backgroundAlpha = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        Info = Configuration.Data.GetCharacterInfo(Interface.Instance.Windows.Menu.GameMode, _characterType);

        UpdateRecordingStatus();
        UpdateLockedPanel();
        SetRenderer();
        ResetItem();
    }
    #endregion

    #region Recording
    private void UpdateRecordingStatus()
    {
        _hasRecording.gameObject.SetActive(Interface.Instance.Windows.Menu.GameMode == MenuWindow.GameModeType.Custom);
        _hasRecording.isOn = CustomAudio;
    }

    public void StartAudioRecording()
    {
        Interface.Instance.Windows.AudioRecorder.Show(this);
    }

    public void SetCustomAudio(AudioClip audio)
    {
        CustomAudio = audio;

        UpdateRecordingStatus();
    }
    #endregion

    #region Renderer
    private void SetRenderer()
    {
        for (int i = 0; i < _backgroundHolder.Length; i++)
            _backgroundHolder[i].color = Info.Color;
        for (int i = 0; i < _iconHolder.Length; i++)
            _iconHolder[i].overrideSprite = Info.Icon;
    }
    #endregion

    #region Lock
    public void UnlockClick()
    {
        UnlockGame.Instance.Show(this);
    }

    public void UpdateLockedPanel()
    {
        _lockedPanel.SetActive(!Unlocked);
    }
    #endregion

    #region Drag
    public void StartDrag()
    {
        if (Used || !Unlocked)
            return;

        _startDragTime = Time.realtimeSinceStartup;

        ActiveDragPanel(true);
        ChangeColor(true);

        Interface.Instance.Windows.Game.MainSoundPad.StartDragItem(this);
    }

    public void StopDrag()
    {
        if (Unlocked && Interface.Instance.Windows.Menu.GameMode == MenuWindow.GameModeType.Custom && Time.realtimeSinceStartup - _startDragTime < 0.1f)
        {
            StartAudioRecording();
        }
    }

    public void ResetItem()
    {
        ChangeColor(false);
        ActiveDragPanel(false);

        _dragPanel.localPosition = Vector3.zero;
        Used = false;
    }

    public void UseItem()
    {
        Used = true;

        ActiveDragPanel(false);
    }

    public void ActiveDragPanel(bool active)
    {
        _dragPanel.gameObject.SetActive(active);
    }
    #endregion

    #region Color
    private void ChangeColor(bool toGrey)
    {
        for (int i = 0; i < _colorFilters.Length; i++)
            _colorFilters[i].enabled = toGrey;

        _backgroundAlpha.alpha = toGrey ? 0.6f : 1f;
    }
    #endregion
}
