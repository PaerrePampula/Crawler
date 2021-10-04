using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] string text = "Touch symbol";
    public delegate void EndTriggered();
    public static event EndTriggered onEndTriggered;
    public void DoPlayerInteraction()
    {
        onEndTriggered?.Invoke();
    }

    public string getPlayerInteractionString()
    {
        return text;
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
