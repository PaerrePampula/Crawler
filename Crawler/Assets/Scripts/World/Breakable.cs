using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Breakable : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject brokenPrefab;
    float propHP = 1;
    public bool ChangeHp(float changeAmount, Vector3 changeDirection = new Vector3())
    {
        propHP += changeAmount;
        if (propHP < 0)
        {
            SubstituteBreakable(changeDirection);
        }
        return true;
    }

    public void KillCharacter()
    {

    }

    public void SubstituteBreakable(Vector3 hitdirection)
    {
        GameObject go = Instantiate(brokenPrefab, transform.position, transform.rotation);
        go.transform.parent = transform.root;
        for (int i = 0; i < go.transform.childCount; i++)
        {
            Vector3 pushDir = go.transform.GetChild(i).position - hitdirection.normalized;
            go.transform.GetChild(i).GetComponent<Rigidbody>().AddForce(pushDir * 10);
        }
        Destroy(gameObject);
    }
}

