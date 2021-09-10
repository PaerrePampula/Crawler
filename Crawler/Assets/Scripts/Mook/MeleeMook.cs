using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class MeleeMook : BaseMook
    {
    public override void DoAIThing()
    {
        base.DoAIThing();
        Debug.Log("Melee thing");
    }

}

