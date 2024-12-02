using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuzzleCubeBlock : MonoBehaviour
{
    #region Variables
    [Header("Main Settings")]
    [SerializeField] private Direction _moveDirection;

    public Direction MoveDirection => _moveDirection;

    private bool _canMove;
    private Collider _collider;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        _collider = GetComponent<Collider>();

        _canMove = true;
    }
    #endregion

    #region Movement
    public void Move()
    {
        if (!_canMove)
            return;

        _canMove = false;
        _collider.isTrigger = true;

        StartCoroutine(Coroutine_Move());
    }

    private IEnumerator Coroutine_Move()
    {
        Vector3 startPosition = transform.localPosition;

        while (true)
        {
            Vector3 direction = new Vector3();
            switch (_moveDirection)
            {
                case Direction.Down:
                    direction = Vector3.down;
                    break;
                case Direction.Up:
                    direction = Vector3.up;
                    break;
                case Direction.Left:
                    direction = Vector3.left;
                    break;
                case Direction.Right:
                    direction = Vector3.right;
                    break;
                case Direction.Forward:
                    direction = Vector3.forward;
                    break;
                case Direction.Back:
                    direction = Vector3.back;
                    break;
            }

            transform.localPosition += direction * 15f * Time.deltaTime;

            Vector3 rayDirection = transform.TransformDirection(transform.localPosition + (direction * 10f)).normalized;
            var ray = new Ray(transform.position, rayDirection);
            float rayLength = new Vector3(direction.x * transform.localScale.x, direction.y * transform.localScale.y, direction.z * transform.localScale.z).magnitude * 0.5f;

            if (Physics.Raycast(ray, out RaycastHit hit, rayLength, 1 << 7, QueryTriggerInteraction.Ignore))
            {
                transform.DOLocalMove(startPosition, 0.15f).OnComplete(() =>
                {
                    _canMove = true;

                    _collider.isTrigger = false;
                });

                yield break;
            }

            if (Vector3.Distance(startPosition, transform.localPosition) > 5)
                break;

            yield return null;
        }

        UnlockGame.Instance.CurrentCube.RemoveBlock(this);

        Destroy(gameObject);
    }
    #endregion

    #region Enums
    public enum Direction
    {
        Right,
        Left,
        Up,
        Down,
        Forward,
        Back
    }
    #endregion
}
