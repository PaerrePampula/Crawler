using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellerDiscussion : MonoBehaviour
{
    bool hasMetPlayer = false;
    [SerializeField] AIChatText sellerChatScriptable;
    Dictionary<string, List<string>> sellerChats = new Dictionary<string, List<string>>();
    CharacterTextBox characterText;
    private void Awake()
    {
        //Initialize all chat texts with identifiers using assets stored in scriptableobjects
        for (int i = 0; i < sellerChatScriptable.chats.Count; i++)
        {
            sellerChats[sellerChatScriptable.chats[i].chatIdentifier] = sellerChatScriptable.chats[i].chatTexts;
        }
        characterText = GetComponent<CharacterTextBox>();
    }
    private void OnEnable()
    {
        SalesPersonTalkingTrigger.onPersonApproach += greetPlayer;
        SalesPersonTalkingTrigger.onPersonLeave += sayGoodByeToPlayer;
        Seller.onGamble += discussGamble;
    }

    private void discussGamble(bool success)
    {
        characterText.InvokeTextDisplay(chooseRandomFromIdentifierForChat("Gambling_Success"));
    }

    private void sayGoodByeToPlayer()
    {
        characterText.InvokeTextDisplay(chooseRandomFromIdentifierForChat("Saying_Goodbye"));
    }

    private void greetPlayer()
    {
        if (!hasMetPlayer)
        {
            characterText.InvokeTextDisplay(chooseRandomFromIdentifierForChat("Greet_NewPlayer"));
            hasMetPlayer = true;
        }
        else
        {
            characterText.InvokeTextDisplay(chooseRandomFromIdentifierForChat("Greet_MetPlayer"));
        }
    }
    string chooseRandomFromIdentifierForChat(string identifier)
    {
        int random = UnityEngine.Random.Range(0, sellerChats[identifier].Count);
        return sellerChats[identifier][random];
    }

    private void OnDestroy()
    {
        SalesPersonTalkingTrigger.onPersonApproach -= greetPlayer;
        SalesPersonTalkingTrigger.onPersonLeave -= sayGoodByeToPlayer;
    }
}
