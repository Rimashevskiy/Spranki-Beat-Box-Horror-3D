using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCell : MonoBehaviour
{
    #region Variables
    [Header("Renderer Settings")]
    [SerializeField] private GameObject _body;

    [Header("Character Settings")]
    [SerializeField] private Transform _characterRoot;

    public SoundPadItem PadItem { get; private set; }
    public Character.Type CurrentCharacter { get; private set; }
    public bool Selected
    {
        get => _selected;
        set
        {
            _bodyRenderer.material.color = Configuration.Data.GetCellColor(value);

            _selected = value;
        }
    }

    private bool _selected;
    private Character _character;
    private SkinnedMeshRenderer _bodyRenderer;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        _bodyRenderer = _body.GetComponentInChildren<SkinnedMeshRenderer>();

        ResetCharacter();
    }
    #endregion

    #region Renderer
    public void SetCharacter(SoundPadItem soundPadItem)
    {
        CurrentCharacter = soundPadItem.CharacterType;
        PadItem = soundPadItem;

        _body.SetActive(false);

        bool isCustomMode = Interface.Instance.Windows.Menu.GameMode == MenuWindow.GameModeType.Custom;
        MenuWindow.GameModeType mode = isCustomMode ? MenuWindow.GameModeType.Cute : Interface.Instance.Windows.Menu.GameMode;

        _character = Instantiate(Resources.Load<GameObject>($"Prefabs/Characters/{mode}/{CurrentCharacter}"), _characterRoot).GetComponent<Character>();
        _character.transform.SetLocalPositionAndRotation(Vector3.zero, new Quaternion());

        _character.Cell = this;

        if (isCustomMode)
            _character.SetCustomSound(soundPadItem.CustomAudio);
    }

    public void ResetCharacter()
    {
        Selected = false;

        _body.SetActive(true);

        if (PadItem)
            PadItem.ResetItem();
        if (_character)
            Destroy(_character.gameObject);

        _character = null;
    }
    #endregion
}
