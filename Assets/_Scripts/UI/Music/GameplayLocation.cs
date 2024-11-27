using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameplayLocation : MonoBehaviour
{
    #region Constants
    public const float MUSIC_SQUARE_LENGTH = 5f;
    #endregion

    #region Variables
    [Header("Main Settings")]
    [SerializeField] private MenuWindow.GameModeType _mode;

    public float CurrentMusicTime { get; private set; }
    public List<Character> Characters { get; private set; }

    private CharacterCell[] _cells;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        Characters = new List<Character>();

        _cells = GetComponentsInChildren<CharacterCell>();
    }

    private void Start()
    {
        StartCoroutine(Coroutine_Playing());
    }
    #endregion

    #region Playing
    private IEnumerator Coroutine_Playing()
    {
        while (true)
        {
            CurrentMusicTime = 0f;

            if (Characters.Count == 0)
                yield return new WaitUntil(() => Characters.Count > 0);

            for (int i = 0; i < Characters.Count; i++)
                Characters[i].PlayMusic();

            for (float i = 0f; i < MUSIC_SQUARE_LENGTH && Characters.Count > 0; i += Time.deltaTime)
            {
                CurrentMusicTime = i;

                yield return null;
            }
        }
    }
    #endregion

    #region Cells
    public void Restart()
    {
        if (Characters.Count == 0)
            return;

        for (int i = 0; i < _cells.Length; i++)
        {
            CharacterCell cell = _cells[i];
            cell.transform.DOLocalMoveY(-4.5f, 0.3f).SetDelay(0.1f * i).OnComplete(() =>
            {
                cell.ResetCharacter();

                cell.transform.DOLocalMoveY(0f, 0.3f);
            });
        }

        Characters.Clear();
    }
    #endregion
}