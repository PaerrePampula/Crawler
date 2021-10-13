using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
class LootBeggar
{
    bool triggered;
    public void InvokeThisPerk()
    {
        ItemDropper.Singleton.SubscribeLootDrop();
        triggered = true;
    }
    public void UnsubListener()
    {
        if (triggered)
        {
            ItemDropper.Singleton.UnSubscribeLootDrop();
            triggered = false;
        }
    }
}

