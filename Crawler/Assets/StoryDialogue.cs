using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryDialogue : MonoBehaviour
{
    [SerializeField] AIChatText story;
    Dictionary<string, List<string>> storyChats = new Dictionary<string, List<string>>();
    CharacterTextBox characterText;
    private void Awake()
    {
        //Initialize all chat texts with identifiers using assets stored in scriptableobjects
        for (int i = 0; i < story.chats.Count; i++)
        {
            storyChats[story.chats[i].chatIdentifier] = story.chats[i].chatTexts;
        }
        characterText = GetComponent<CharacterTextBox>();
        characterText.InitializeSpecialSounds(story.specialChatSoundsForCharacter);
    }
    public void InvokeDialogue(string name)
    {
        characterText.InvokeTextDisplay(storyChats[name][0]);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
