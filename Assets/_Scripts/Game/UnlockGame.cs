using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnlockGame : Singleton<UnlockGame>
{
    #region Variables
    [Header("Scene Settings")]
    [SerializeField] private GameObject _scene;

    [Header("Cube Settings")]
    [SerializeField] private Transform _cubeRoot;

    [Header("Blocks Settings")]
    [SerializeField] private LayerMask _blocksLayer;

    [Header("Camera Settings")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float _rotationSpeed = 0.13f;

    [Header("Complete Settings")]
    [SerializeField] private ParticleSystem _completeEffect;

    public bool Activated { get; private set; }
    public PuzzleCube CurrentCube { get; private set; }

    private Vector3 _lastMousePosition;
    private bool _isDragging = false;
    private float _lastClickTime;
    private SoundPadItem _targetPadItem;
    private Vector3 _startCubePosition;
    private Quaternion _startCubeRotation;

    private int _puzzleCubeId
    {
        get => PlayerPrefs.GetInt("PuzzleCubeId", 0);
        set => PlayerPrefs.SetInt("PuzzleCubeId", value);
    }
    #endregion

    #region UnityMethods
    public override void Awake()
    {
        base.Awake();

        _startCubePosition = _cubeRoot.position;
        _startCubeRotation = _cubeRoot.rotation;
    }

    private void Update()
    {
        if (!Activated)
            return;

        RotateCamera();

        if (Input.GetMouseButtonDown(0))
            _lastClickTime = Time.realtimeSinceStartup;

        if (Input.GetMouseButtonUp(0) && Time.realtimeSinceStartup - _lastClickTime < 0.1f)
            CheckInteraction();
    }
    #endregion

    #region GameCycle
    public void ShowComplete()
    {
        Unlock();

        _cubeRoot.transform.DOLocalRotate(Vector3.zero, 1f);
        _cubeRoot.transform.DOScale(Vector3.one * 2f, 1f);

        Interface.Instance.Windows.Unlock.ShowComplete();
    }

    public void FinishClick()
    {
        Hide();
    }
    #endregion

    #region Activation
    public void Show(SoundPadItem soundPadItem)
    {
        _targetPadItem = soundPadItem;

        Interface.Instance.Windows.Game.Hide();
        Interface.Instance.Windows.Unlock.Show();

        _cubeRoot.transform.localScale = Vector3.one;
        _cubeRoot.SetPositionAndRotation(_startCubePosition, _startCubeRotation);

        CurrentCube = Instantiate(Resources.Load<GameObject>($"Prefabs/PuzzleCube/PuzzleCube_{_puzzleCubeId}"), _cubeRoot).GetComponent<PuzzleCube>();
        CurrentCube.transform.SetLocalPositionAndRotation(Vector3.zero, new Quaternion());

        CurrentCube.SpawnCharacter(soundPadItem.Info.Mode, soundPadItem.CharacterType);

        CameraManager.Instance.DisableCamera();

        _scene.SetActive(true);

        Activated = true;
    }

    public void Hide()
    {
        Destroy(CurrentCube.gameObject);

        CameraManager.Instance.EnableCamera();

        Interface.Instance.Windows.Game.Show();
        Interface.Instance.Windows.Unlock.Hide();

        _scene.SetActive(false);

        Activated = false;
    }

    public void Unlock()
    {
        if (_targetPadItem)
        {
            _targetPadItem.Unlocked = true;

            _targetPadItem.UpdateLockedPanel();
        }
    }
    #endregion

    #region Camera
    private void RotateCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lastMousePosition = Input.mousePosition;
            _isDragging = true;
        }
        else if (Input.GetMouseButton(0) && _isDragging)
        {
            Vector3 delta = Input.mousePosition - _lastMousePosition;

            delta *= Screen.width * 0.1f;

            float rotationX = delta.y * _rotationSpeed * Time.deltaTime;
            float rotationY = -delta.x * _rotationSpeed * Time.deltaTime;

            _cubeRoot.Rotate(Vector3.up, rotationY, Space.World);
            _cubeRoot.Rotate(Vector3.right, rotationX, Space.World);

            _lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }
    }
    #endregion

    #region Interaction
    private void CheckInteraction()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _blocksLayer))
        {
            if (hit.transform.TryGetComponent<PuzzleCubeBlock>(out PuzzleCubeBlock cubeBlock))
                cubeBlock.Move();
        }
    }
    #endregion
}
