using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class GenericStateFuncs
{

    public static Func<bool> metAggroRange(float range, Transform target, Transform thisTransform) => () => (Vector3.Distance(target.position, thisTransform.position) <= range);
}
