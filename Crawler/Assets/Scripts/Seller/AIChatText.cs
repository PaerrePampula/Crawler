using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// An asset for all possible dialogue for a character
/// a single text asset is accessed using the identifier.
/// </summary>
[CreateAssetMenu (menuName = "NPC Dialogue")]
class AIChatText : ScriptableObject
{
    public List<Chat> chats = new List<Chat>();
    public SpecialChatSound[] specialChatSoundsForCharacter;
}
[Serializable]
class Chat
{
    public string chatIdentifier;
    [TextArea]
    public List<string> chatTexts;
}
[Serializable]
public class SpecialChatSound
{
    public string characterIdentifier;
    public AudioClip specialSound;
}
