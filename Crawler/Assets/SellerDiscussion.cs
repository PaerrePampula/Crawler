using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellerDiscussion : MonoBehaviour
{
    bool hasMetPlayer = false;
    [SerializeField] AIChatText sellerChatScriptable;
    Dictionary<string, string> sellerChats = new Dictionary<string, string>();
    CharacterTextBox characterText;
    private void Awake()
    {
        //Initialize all chat texts with identifiers using assets stored in scriptableobjects
        for (int i = 0; i < sellerChatScriptable.chats.Count; i++)
        {
            sellerChats[sellerChatScriptable.chats[i].chatIdentifier] = sellerChatScriptable.chats[i].chatText;
        }
        characterText = GetComponent<CharacterTextBox>();
    }
    private void OnEnable()
    {
        SalesPersonTalkingTrigger.onPersonApproach += greetPlayer;
        SalesPersonTalkingTrigger.onPersonLeave += sayGoodByeToPlayer;
    }

    private void sayGoodByeToPlayer()
    {
        characterText.InvokeTextDisplay(sellerChats["Saying_Goodbye"]);
    }

    private void greetPlayer()
    {
        if (!hasMetPlayer)
        {
            characterText.InvokeTextDisplay(sellerChats["Greet_NewPlayer"]);
            hasMetPlayer = true;
        }
        else
        {
            characterText.InvokeTextDisplay(sellerChats["Greet_MetPlayer"]);
        }
    }

    private void OnDestroy()
    {
        SalesPersonTalkingTrigger.onPersonApproach -= greetPlayer;
        SalesPersonTalkingTrigger.onPersonLeave -= sayGoodByeToPlayer;
    }
}
