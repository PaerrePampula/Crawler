using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[Serializable]
class AttainHitBoxWithCircle
{

    [SerializeField] float hitBoxRadius;
    Transform _character;
    [SerializeField] GameObject hitBoxObject;
    LayerMask _hitMask;
    GameObject instantiatedWarning;
    public void InitializeVariables(LayerMask layerMask, Transform character)
    {
        _character = character;
        _hitMask = layerMask;

    }
    public Action displayHitBoxWarning() => () =>
    {
        instantiatedWarning = GameObject.Instantiate(hitBoxObject);
        instantiatedWarning.transform.position = _character.position;
        instantiatedWarning.transform.localScale = new Vector3(hitBoxRadius * 2, hitBoxRadius * 2, hitBoxRadius * 2);
    };
    public Action updateHitBoxWarning() => () =>
    {
        if (instantiatedWarning != null)
        instantiatedWarning.transform.position = _character.position;
    };
    public Action stopWarning() => () =>
    {
        GameObject.Destroy(instantiatedWarning);
    };
    public Func<float, Collider[]> getHitBoxes() => (angle) => Physics.OverlapSphere(_character.position, hitBoxRadius, _hitMask);
}

