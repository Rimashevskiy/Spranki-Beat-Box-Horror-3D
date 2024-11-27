using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPad : MonoBehaviour
{
    #region Variables
    [Header("Drag Settings")]
    [SerializeField] private LayerMask _cellLayer;
    [SerializeField] private RectTransform _itemTopSortRoot;

    public SoundPadItem SelectedItem { get; private set; }

    private SoundPadItem[] _soundPadItems;
    private Vector3 _padMoveOffset;
    private Dictionary<Collider, CharacterCell> _scannedCells;
    private CharacterCell _lastSelectedCell;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        _soundPadItems = GetComponentsInChildren<SoundPadItem>();
        _scannedCells = new Dictionary<Collider, CharacterCell>();
    }

    private void Update()
    {
        if (SelectedItem)
            ItemDragging();

        if (Input.GetMouseButtonDown(0))
        {
            CharacterCell cell = CellRaycast();
            if (cell)
                cell.ResetCharacter();
        }
    }
    #endregion

    #region Drag
    public void StartDragItem(SoundPadItem soundPadItem)
    {
        SelectedItem = soundPadItem;

        SelectedItem.DragPanel.transform.SetParent(_itemTopSortRoot);

        _padMoveOffset = SelectedItem.transform.position - Input.mousePosition;
    }

    private void ItemDragging()
    {
        SelectedItem.DragPanel.transform.position = Input.mousePosition + _padMoveOffset;

        CharacterCell cell = CellRaycast();

        if (cell && _lastSelectedCell)
            _lastSelectedCell.Selected = false;
        else if (!cell && _lastSelectedCell)
            _lastSelectedCell.Selected = false;

        _lastSelectedCell = cell;

        if (_lastSelectedCell)
            _lastSelectedCell.Selected = true;

        if (Input.GetMouseButtonUp(0))
            EndDragItem();
    }

    private void EndDragItem()
    {
        CharacterCell cell = CellRaycast();
        if (cell)
        {
            cell.SetCharacter(SelectedItem);

            SelectedItem.UseItem();
        }
        else
            SelectedItem.ResetItem();

        SelectedItem.DragPanel.transform.SetParent(SelectedItem.transform);

        if (_lastSelectedCell)
            _lastSelectedCell.Selected = false;

        SelectedItem = null;
    }
    #endregion

    #region Raycast
    private CharacterCell CellRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _cellLayer))
        {
            if (_scannedCells.ContainsKey(hit.collider))
                return _scannedCells[hit.collider];
            else
            {
                CharacterCell cell = hit.collider.GetComponentInParent<CharacterCell>();

                _scannedCells.Add(hit.collider, cell);

                if (cell)
                    return cell;
            }
        }

        return null;
    }
    #endregion
}
