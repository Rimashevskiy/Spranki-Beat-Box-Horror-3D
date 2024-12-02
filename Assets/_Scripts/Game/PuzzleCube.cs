using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCube : MonoBehaviour
{
    #region Variables
    [Header("Block Settings")]
    [SerializeField] private Transform _blocksRoot;

    [Header("Character Settings")]
    [SerializeField] private Transform _characterRoot;

    private List<PuzzleCubeBlock> _blocks;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        _blocks = new List<PuzzleCubeBlock>(GetComponentsInChildren<PuzzleCubeBlock>());
    }
    #endregion

    #region GameCycle
    public void RemoveBlock(PuzzleCubeBlock cubeBlock)
    {
        _blocks.Remove(cubeBlock);

        Debug.Log(_blocks.Count);

        if (_blocks.Count == 0)
            UnlockGame.Instance.ShowComplete();
    }
    #endregion

    #region Character
    public void SpawnCharacter(MenuWindow.GameModeType mode, Character.Type type)
    {
        GameObject character = Instantiate(Resources.Load<GameObject>($"Prefabs/Characters/{mode}/{type}"), _characterRoot);
        character.transform.SetLocalPositionAndRotation(Vector3.zero, new Quaternion());
    }
    #endregion

    #region Tools
#if UNITY_EDITOR
    [ContextMenu("Polish Blocks")]
    private void PolishBlocks()
    {
        int blocksCount = _blocksRoot.childCount;
        List<Material> materials = new List<Material>(Resources.LoadAll<Material>("Materials/PuzzleCube/"));

        for (int i = 0; i < blocksCount; i++)
        {
            _blocksRoot.position = new Vector3(RoundToNearest(_blocksRoot.position.x, 1), RoundToNearest(_blocksRoot.position.y, 1), RoundToNearest(_blocksRoot.position.z, 1));
            _blocksRoot.localScale = new Vector3(RoundToNearest(_blocksRoot.localScale.x, 1), RoundToNearest(_blocksRoot.localScale.y, 1), RoundToNearest(_blocksRoot.localScale.z, 1));

            _blocksRoot.GetChild(i).name = $"Block_{i}";
            _blocksRoot.GetChild(i).GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Count)];
        }
    }

    private float RoundToNearest(float value, int decimalPlaces)
    {
        return (float)System.Math.Round(value, decimalPlaces);
    }
#endif
    #endregion
}
