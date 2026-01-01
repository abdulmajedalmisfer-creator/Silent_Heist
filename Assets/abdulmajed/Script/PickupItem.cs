using UnityEngine;

public class PickupItem : MonoBehaviour
{
    bool held;

    public void SetHeld(bool isHeld)
    {
        held = isHeld;
    }

    public bool IsHeld()
    {
        return held;
    }
}
