/// <summary>
/// Saves information about e.g can player currently move etc. which gets disabled when player
/// dies, or is in a menu.
/// </summary>
static class Globals
{
    //25 ms for single character on text box
    static float characterTextSpeed = 0.025f;
    static bool _movementControlsAreEnabled = true;
    static bool _controlsAreEnabled = true;
    static float likelinessOfItemDroppingInRoom = 100f;
    static float likelinessOfMookDroppingHp = 20f;
    public static bool ControlsAreEnabled { get => _controlsAreEnabled; set => _controlsAreEnabled = value; }
    public static bool MovementControlsAreEnabled { get => _movementControlsAreEnabled; set => _movementControlsAreEnabled = value; }
    public static float CharacterTextSpeed { get => characterTextSpeed; set => characterTextSpeed = value; }
    public static float LikelinessOfItemDroppingInRoom { get => likelinessOfItemDroppingInRoom; set => likelinessOfItemDroppingInRoom = value; }
    public static float LikelinessOfMookDroppingHp { get => likelinessOfMookDroppingHp; set => likelinessOfMookDroppingHp = value; }
}
