using UnityEngine;

public class PickupItem : MonoBehaviour
{
    Rigidbody rb;
    Collider[] cols;
    bool wasKinematic;
    bool held;

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
            wasKinematic = rb.isKinematic;
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            for (int i = 0; i < cols.Length; i++)
                cols[i].enabled = false;
        }
        else
        {
            rb.useGravity = true;
            rb.isKinematic = wasKinematic;

            for (int i = 0; i < cols.Length; i++)
                cols[i].enabled = true;
        }
    }
}
