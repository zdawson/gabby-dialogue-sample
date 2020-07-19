using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gabby Dialogue Sample/Character Definition")]
public class DialogueCharacter : ScriptableObject
{
    [Serializable]
    private class CharacterPortrait
    {
        public string name = "default";
        public Sprite portrait = null;
    }

    public string internalName = "NewCharacter";
    public string displayName = "New Character";
    [SerializeField]
    private CharacterPortrait[] _portraits = null;

    [NonSerialized]
    public Dictionary<string, Sprite> _portraitMap = null;

    public Dictionary<string, Sprite> Portraits
    {
        get
        {
            if (_portraitMap == null)
            {
                InitPortraitMap();
            }
            return _portraitMap;
        }
        private set => _portraitMap = value;
    }

    private void InitPortraitMap()
    {
        _portraitMap = new Dictionary<string, Sprite>();
        foreach (CharacterPortrait portrait in _portraits)
        {
            _portraitMap.Add(portrait.name, portrait.portrait);
        }
    }
}
