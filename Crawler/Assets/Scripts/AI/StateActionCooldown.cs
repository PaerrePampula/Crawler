using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[Serializable]
class StateActionCooldown
{
    float lastCooldownTime = 0;
    [SerializeField] float stateActionCooldownLength = 0;
    public Action setCooldown() => () =>
    {
        lastCooldownTime = Time.time;
    };
    public Func<bool> CooldownPassed()
    {
        return () =>
        {
            if (lastCooldownTime == 0) return true;
            return (Time.time > lastCooldownTime + stateActionCooldownLength) ? true : false;
        };
    }
}

