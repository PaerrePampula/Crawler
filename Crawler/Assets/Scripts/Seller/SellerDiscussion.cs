using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Controls all the mechanics for the seller characters' speech
/// </summary>
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
        characterText.InitializeSpecialSounds(sellerChatScriptable.specialChatSoundsForCharacter);
    }
    private void OnEnable()
    {
        SalesPersonTalkingTrigger.onPersonApproach += greetPlayer;
        SalesPersonTalkingTrigger.onPersonLeave += sayGoodByeToPlayer;
        Seller.onGamble += discussGamble;
        Seller.onHealthBuy += discussHealthBuy;
    }

    private void discussHealthBuy(bool success)
    {
        string state = (success) ? "Buy_Success" : "Buy_Failure";
        characterText.InvokeTextDisplay(chooseRandomFromIdentifierForChat(state));
    }

    private void discussGamble(bool success)
    {
        string state = (success) ? "Gambling_Success" : "Buy_Failure";
        characterText.InvokeTextDisplay(chooseRandomFromIdentifierForChat(state));
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

    private void OnDisable()
    {
        SalesPersonTalkingTrigger.onPersonApproach -= greetPlayer;
        SalesPersonTalkingTrigger.onPersonLeave -= sayGoodByeToPlayer;
        Seller.onGamble -= discussGamble;
        Seller.onHealthBuy -= discussHealthBuy;
    }
}
