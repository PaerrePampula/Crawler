using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[Serializable]
class GlockPerk
{
    Heading heading;
    [SerializeField] GameObject pistolPrefab;
    [SerializeField] GameObject lineRenderer;
    [SerializeField] LayerMask hittableMask;
    [SerializeField] AIChatText chatText;
    [SerializeField] AudioClip shotSound;
    bool triggered;
    public void UnsubListener()
    {
        if (triggered)
        {
            Player.Singleton.GetComponent<PlayerWeapon>().attackDelegates -= FireGun;
            triggered = false;
        }
    }
    public void InvokeThisPerk()
    {
        GameObject go = GameObject.Instantiate(pistolPrefab, Player.Singleton.transform);
        Player.Singleton.GetComponent<PlayerWeapon>().attackDelegates += FireGun;
        YellRandomLine();
        triggered = false;
    }
    void YellRandomLine()
    {
        int random = UnityEngine.Random.Range(0, chatText.chats[0].chatTexts[0].Length);
        PlayerController.Singleton.GetComponentInChildren<CharacterTextBox>().InvokeTextDisplay(chatText.chats[0].chatTexts[random]);
        Player.Singleton.StartCoroutine(ActionDelayer.actionWait(() => YellRandomLine(), Time.time + 15f));
    }
    private void FireGun()
    {
        if (heading == null)
        {
            heading = Player.Singleton.GetComponent<Heading>();
        }
        GameObject cloneRenderer = GameObject.Instantiate(lineRenderer, Player.Singleton.transform.position, Quaternion.identity);
        cloneRenderer.GetComponent<LineRenderer>().SetPosition(1, heading.getHeadingVector().normalized * 100);
        cloneRenderer.GetComponent<LineRenderer>().SetPosition(0, PlayerController.Singleton.transform.position);
        RaycastHit raycastHit;
        Player.Singleton.GetComponent<AudioSource>().PlayOneShot(shotSound);
        if (Physics.Raycast(Player.Singleton.transform.position + Vector3.up*0.15f, heading.getHeadingVector().normalized, out raycastHit, 100, hittableMask))
        {
            if (raycastHit.collider.GetComponent<IDamageable>() != null)
            {
                raycastHit.collider.GetComponent<IDamageable>().ChangeHp(-50+ Player.Singleton.getBonusDamage(-50));
            }
            cloneRenderer.GetComponent<LineRenderer>().SetPosition(1, raycastHit.point);
        }

    }
}

