using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private Rigidbody rb;
    private Collider[] cols;
    private bool held;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cols = GetComponentsInChildren<Collider>(true);
    }

    public void SetHeld(bool isHeld)
    {
        held = isHeld;

        if (rb == null) return;

        if (held)
        {
            // While held: no gravity, stop motion, avoid collisions by trigger
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            for (int i = 0; i < cols.Length; i++)
                cols[i].isTrigger = true;
        }
        else
        {
            // When released/thrown: physics ON
            rb.useGravity = true;
            rb.isKinematic = false;

            for (int i = 0; i < cols.Length; i++)
                cols[i].isTrigger = false;
        }
    }

    public bool IsHeld()
    {
        return held;
    }
}

