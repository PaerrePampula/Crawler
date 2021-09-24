using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[CreateAssetMenu (menuName = "NPC Dialogue")]
class AIChatText : ScriptableObject
{
    public List<Chat> chats = new List<Chat>();
}
[Serializable]
class Chat
{
    public string chatIdentifier;
    [TextArea] public string chatText;
}

