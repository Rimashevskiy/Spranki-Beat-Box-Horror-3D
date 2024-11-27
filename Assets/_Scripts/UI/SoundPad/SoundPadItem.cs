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

    [Header("Renderer Settings")]
    [SerializeField] private Image[] _backgroundHolder;
    [SerializeField] private Image[] _iconHolder;

    public RectTransform DragPanel => _dragPanel;
    public Character.Type CharacterType => _characterType;
    public bool Used { get; private set; }

    private UIEffect[] _colorFilters;
    private Configuration.CharacterDataPreset _info;
    private CanvasGroup _backgroundAlpha;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        _colorFilters = GetComponentsInChildren<UIEffect>(true);
        _info = Configuration.Data.GetCharacterInfo(_characterType);
        _backgroundAlpha = GetComponent<CanvasGroup>();

        SetRenderer();
        ResetItem();
    }
    #endregion

    #region Renderer
    private void SetRenderer()
    {
        for (int i = 0; i < _backgroundHolder.Length; i++)
            _backgroundHolder[i].color = _info.Color;
        for (int i = 0; i < _iconHolder.Length; i++)
            _iconHolder[i].overrideSprite = _info.Icon;
    }
    #endregion

    #region Drag
    public void StartDrag()
    {
        if (Used)
            return;

        ActiveDragPanel(true);
        ChangeColor(true);

        Interface.Instance.Windows.Game.MainSoundPad.StartDragItem(this);
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
