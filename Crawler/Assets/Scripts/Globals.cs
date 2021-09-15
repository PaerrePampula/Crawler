/// <summary>
/// Saves information about e.g can player currently move etc. which gets disabled when player
/// dies, or is in a menu.
/// </summary>
static class Globals
{
    static bool _movementControlsAreEnabled = true;
    static bool _controlsAreEnabled = true;
    public static bool ControlsAreEnabled { get => _controlsAreEnabled; set => _controlsAreEnabled = value; }
    public static bool MovementControlsAreEnabled { get => _movementControlsAreEnabled; set => _movementControlsAreEnabled = value; }
}
