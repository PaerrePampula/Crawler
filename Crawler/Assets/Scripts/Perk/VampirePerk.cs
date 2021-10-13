using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
class VampirePerk
{
    bool triggered;
    public void InvokeThisPerk()
    {
        Room.onRoomClear += HealPlayer;
        triggered = true;
    }
    public void UnsubListener()
    {
        if (triggered)
        {
            triggered = false;
            Room.onRoomClear -= HealPlayer;
        }
    }

    private void HealPlayer(Room room)
    {
        Player.Singleton.ChangeHp(5);
    }
}

