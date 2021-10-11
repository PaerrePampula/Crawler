using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
class DoublingMoney
{
    bool triggered;
    public void InvokeThisPerk()
    {
        MoneyItemType.onMoneyChange += DoubleMoney;
        triggered = true;
    }
    public void UnsubPerkListener()
    {
        if (triggered)
        {
            MoneyItemType.onMoneyChange -= DoubleMoney;
            triggered = false;
        }
    }
    private void DoubleMoney(int amount)
    {
        PlayerEconomy.Singleton.ChangePlayerMoney(amount);
    }
}

