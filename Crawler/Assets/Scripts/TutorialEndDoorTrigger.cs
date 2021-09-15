using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialEndDoorTrigger : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] PlayableDirector director;
    [SerializeField] string interactionText = "Press [E] to continue once ready!";

    public void DoPlayerInteraction()
    {
        director.Play();
    }

    public string getPlayerInteractionString()
    {
        return interactionText;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
