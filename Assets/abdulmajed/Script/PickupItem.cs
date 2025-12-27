using UnityEngine;

public class PickupItem : MonoBehaviour
{
    Rigidbody rb;
    Collider[] cols;
    bool held;

    bool[] originalTriggers;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cols = GetComponentsInChildren<Collider>(true);

        originalTriggers = new bool[cols.Length];
        for (int i = 0; i < cols.Length; i++)
            originalTriggers[i] = cols[i].isTrigger;
    }

    public void SetHeld(bool isHeld)
    {
        held = isHeld;
        if (rb == null) return;

        if (held)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            for (int i = 0; i < cols.Length; i++)
                cols[i].isTrigger = true;
        }
        else
        {
            rb.useGravity = true;

            for (int i = 0; i < cols.Length; i++)
                cols[i].isTrigger = originalTriggers[i];
        }
    }

    public bool IsHeld()
    {
        return held;
    }
}
