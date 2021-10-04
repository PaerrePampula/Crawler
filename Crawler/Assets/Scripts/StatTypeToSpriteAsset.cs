using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Game UI asset lists/Stat to sprite asset")]
class StatTypeToSpriteAsset : ScriptableObject
{
    public List<StatTypeToSprite> statTypesToSprites = new List<StatTypeToSprite>();
}
[Serializable]
public class StatTypeToSprite
{
    [SerializeField] Sprite sprite;
    [SerializeField] StatType itemBehaviourType;

    public StatType ItemBehaviourType { get => itemBehaviourType; set => itemBehaviourType = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
}

