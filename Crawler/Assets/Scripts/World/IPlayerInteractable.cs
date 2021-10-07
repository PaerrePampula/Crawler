/// <summary>
/// Interfaces any object in the game world than can be interacted with
/// doors, text boxes, etc.
/// </summary>
interface IPlayerInteractable
{
    public void DoPlayerInteraction();
    //Supplies the UI for text prompt
    public InputAlias[] getPlayerInteractions();
    public bool getPlayerInteraction();
}

