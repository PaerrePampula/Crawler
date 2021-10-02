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
    public Func<float, Collider[]> getHitBoxes() => (angle) => Physics.OverlapBox(_character.position + _character.transform.TransformDirection(new Vector3(0, 0, hitBoxSize.z / 2f)), new Vector3(hitBoxSize.x / 2f, 500, hitBoxSize.z / 2f),
        Quaternion.Euler(0, angle, 0), _hitMask);
}

