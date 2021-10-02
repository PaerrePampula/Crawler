using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[Serializable]
class AttainHitBoxWithOverLapBox
{

    [SerializeField] Vector3 hitBoxSize;
    Transform _character;
    LayerMask _hitMask;

    public void InitializeVariables(LayerMask layerMask, Transform character)
    {
        _character = character;
        _hitMask = layerMask;
    }
    public Func<float, Collider[]> getHitBoxes() => (angle) =>
        {
            Vector3 attackVector = Orientation.getVectorRotatedOnYAxisFor(-angle, new Vector3(hitBoxSize.x, 0, hitBoxSize.z));

            return Physics.OverlapBox(_character.position - attackVector/2f, new Vector3(hitBoxSize.x / 2f, 500, hitBoxSize.z / 2f),
        Quaternion.Euler(0, 0, 0), _hitMask);
        }; 
}

