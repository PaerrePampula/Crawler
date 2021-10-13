using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Breakable : MonoBehaviour, IDamageable
{

    [SerializeField] AudioClip _audioClip;
    [SerializeField] GameObject brokenPrefab;
    float propHP = 1;
    DropItemOnTrigger onTrigger;
    private void Start()
    {
        onTrigger = GetComponent<DropItemOnTrigger>();
    }
    public bool ChangeHp(float changeAmount, Vector3 changeDirection = new Vector3(), bool wasCritical = false)
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

            Vector3 pushDir = (go.transform.GetChild(i).position - hitdirection).normalized;
            go.transform.GetChild(i).GetComponent<Rigidbody>().AddForce(pushDir * 250f);
        }
        onTrigger?.TryTriggerDrop();
        if (_audioClip != null) go.GetComponent<AudioSource>().PlayOneShot(_audioClip);
        Destroy(gameObject);
    }
}

