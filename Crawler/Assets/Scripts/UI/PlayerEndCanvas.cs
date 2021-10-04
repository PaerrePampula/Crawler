using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEndCanvas : MonoBehaviour
{
    CharacterTextBox characterText;
    [SerializeField] AIChatText AIChatText;
    Dictionary<string, List<string>> characterChats = new Dictionary<string, List<string>>();
    bool subscribedToRestartingGame = false;

    private void OnEnable()
    {

    }
    public void InvokeStoryTellerText(string identifier)
    {
        characterText.InvokeTextDisplay(characterChats[identifier][0]);
    }
    public void SubscribeToRestartingGame()
    {
        subscribedToRestartingGame = true;
        characterText.onDoneTalking += RestartGame;
    }
    public void SubscribeToQuittingGame()
    {
        characterText.onDoneTalking += QuitGame;
    }

    private void QuitGame()
    {
        Application.Quit(0);
    }

    private void OnDestroy()
    {
        if (subscribedToRestartingGame) characterText.onDoneTalking -= RestartGame;
    }
    private void RestartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Awake()
    {
        //Initialize all chat texts with identifiers using assets stored in scriptableobjects
        for (int i = 0; i < AIChatText.chats.Count; i++)
        {
            characterChats[AIChatText.chats[i].chatIdentifier] = AIChatText.chats[i].chatTexts;
        }
        characterText = GetComponent<CharacterTextBox>();
        characterText.InitializeSpecialSounds(AIChatText.specialChatSoundsForCharacter);
    }
}
