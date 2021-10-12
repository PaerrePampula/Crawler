using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Perk class for the perk "Laser Beam"
/// </summary>
[Serializable]
class SwordBeam
{
    [SerializeField] GameObject laserBeam;
    [SerializeField] AudioClip beamSound;
    Heading heading;
    bool triggered;
    void DelegateLaserBeam()
    {
        Player.Singleton.GetComponent<PlayerWeapon>().attackDelegates += FireLaserBeam;
        triggered = true;
    }
    public void UnsubListener()
    {
        if (triggered)
        {
            Player.Singleton.GetComponent<PlayerWeapon>().attackDelegates -= FireLaserBeam;
            triggered = false;
        }
    }
    private void FireLaserBeam()
    {
        if (Player.Singleton.Hp >= Player.Singleton.MaxHp*0.8f)
        {
            if (heading == null)
            {
                heading = Player.Singleton.GetComponent<Heading>();

            }
            GameObject beam = GameObject.Instantiate(laserBeam, heading.transform.position + heading.getHeadingVector().normalized, Quaternion.Euler(new Vector3(0, -heading.getPlayerHeadingAngle() + 90, 0)));
            Player.Singleton.GetComponent<AudioSource>().PlayOneShot(beamSound);
            beam.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1200f);
        }
    }

    public void InvokeThisPerk()
    {
        DelegateLaserBeam();
    }

}
