using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[Serializable]
class ChangePlayerStatPerk
{
    [SerializeField] ItemScriptable[] statChangingItems;
    public void InvokeThisPerk()
    {
        for (int i = 0; i < statChangingItems.Length; i++)
        {
            ItemCreator.CreateAndGiveItemToPlayer(statChangingItems[i]);
        }
    }
    
}