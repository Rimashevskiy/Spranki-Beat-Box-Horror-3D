using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "Configuration", menuName = "Configuration/Create", order = 1)]
public class Configuration : ScriptableObject
{
    #region Constants
    public static string FILE_PATCH => Application.persistentDataPath + "/data";
    #endregion

    #region Variables
    [Header("Characters Settings")]
    [SerializeField] private CharacterDataPreset[] _characters;

    [Header("Cells Settings")]
    [SerializeField] private Color _cellDefaultColor;
    [SerializeField] private Color _cellSelectedColor;

    #endregion

    #region Methods
    public CharacterDataPreset GetCharacterInfo(Character.Type type)
    {
        for (int i = 0; i < _characters.Length; i++)
            if (_characters[i].Type == type)
                return _characters[i];

        return null;
    }

    public Color GetCellColor(bool isSelected)
    {
        return isSelected ? _cellSelectedColor : _cellDefaultColor;
    }
    #endregion

    #region Classes
    [System.Serializable]
    public class CharacterDataPreset
    {
        public string Name;
        public Character.Type Type;
        public Sprite Icon;
        public AudioClip Sound;
        public Color Color;
    }
    #endregion

    #region Singleton
    private static Configuration _data;
    public static Configuration Data
    {
        get
        {
            if (!_data)
                _data = Resources.Load<Configuration>("Configuration");

            return _data;
        }
    }
    #endregion
}
