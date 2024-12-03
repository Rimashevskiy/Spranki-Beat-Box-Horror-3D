using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Variables
    [Header("Main Settings")]
    [SerializeField] private Type _myType;

    public CharacterCell Cell { get; set; }

    private AudioSource _myAudio;
    private Configuration.CharacterDataPreset _info;
    private Animator _myAnimator;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        _myAudio = GetComponent<AudioSource>();
        _myAnimator = GetComponentInChildren<Animator>();
        _info = Configuration.Data.GetCharacterInfo(Interface.Instance.Windows.Menu.GameMode, _myType);

        if (!_myAudio.clip)
            _myAudio.clip = _info.Sound;
        _myAudio.loop = false;

        Map.Instance.CurrentLocation.Characters.Add(this);
    }

    private void OnDestroy()
    {
        if (Map.Instance)
            Map.Instance.CurrentLocation.Characters.Remove(this);
    }
    #endregion

    #region Music
    public void SetCustomSound(AudioClip sound)
    {
        _myAudio.clip = sound;
    }

    public void PlayMusic()
    {
        _myAudio.PlayOneShot(_myAudio.clip);

        //_myAudio.Stop();
        //_myAnimator.Play("Play", 0, 0f);
    }
    #endregion

    #region Enums
    public enum Type
    {
        None,
        Orange,
        Red,
        Silver,
        Green,
        Gray,
        Brown,
        Lime,
        Sky,
        Purple,
        Yellow,
        Tan,
        White,
        Pink,
        Blue,
        Black,
        FunBot,
        Gold,
        Sun,
        Tree,
        Computer
    }
    #endregion
}
