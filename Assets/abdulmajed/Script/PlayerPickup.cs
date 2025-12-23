using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Transform holdPoint;
    public float pickupRange = 2.5f;
    public LayerMask pickupMask;

    public float throwMinForce = 6f;
    public float throwMaxForce = 18f;
    public float chargeTimeToMax = 1.2f;

    Rigidbody heldRb;
    PickupItem heldItem;

    bool charging;
    float charge;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldRb == null) TryPickup();
            else Drop();
        }

        if (heldRb != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                charging = true;
                charge = 0f;
            }

            if (charging && Input.GetMouseButton(0))
            {
                charge += Time.deltaTime;
                if (charge > chargeTimeToMax) charge = chargeTimeToMax;
            }

            if (charging && Input.GetMouseButtonUp(0))
            {
                Throw();
            }
        }
    }

    void FixedUpdate()
    {
        if (heldRb == null) return;

        Vector3 to = holdPoint.position - heldRb.position;
        heldRb.linearVelocity = to / Time.fixedDeltaTime;
        heldRb.angularVelocity = Vector3.zero;
    }

    void TryPickup()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange, pickupMask, QueryTriggerInteraction.Ignore);
        if (hits.Length == 0) return;

        Collider best = null;
        float bestDist = float.MaxValue;

        for (int i = 0; i < hits.Length; i++)
        {
            float d = Vector3.Distance(transform.position, hits[i].transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = hits[i];
            }
        }

        if (best == null) return;

        Rigidbody rb = best.attachedRigidbody;
        if (rb == null) return;

        PickupItem item = rb.GetComponent<PickupItem>();
        if (item == null) item = best.GetComponentInParent<PickupItem>();
        if (item == null) return;

        heldRb = rb;
        heldItem = item;

        heldItem.SetHeld(true);

        heldRb.transform.SetParent(holdPoint, true);
        heldRb.position = holdPoint.position;
        heldRb.rotation = holdPoint.rotation;

        heldRb.linearVelocity = Vector3.zero;
        heldRb.angularVelocity = Vector3.zero;
        heldRb.useGravity = false;
    }

    void Drop()
    {
        if (heldRb == null) return;

        heldRb.transform.SetParent(null, true);

        heldItem.SetHeld(false);

        heldRb = null;
        heldItem = null;

        charging = false;
        charge = 0f;
    }

    void Throw()
    {
        if (heldRb == null) return;

        float t = chargeTimeToMax <= 0f ? 1f : Mathf.Clamp01(charge / chargeTimeToMax);
        float force = Mathf.Lerp(throwMinForce, throwMaxForce, t);

        Rigidbody rb = heldRb;
        PickupItem item = heldItem;

        rb.transform.SetParent(null, true);

        item.SetHeld(false);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(holdPoint.forward * force, ForceMode.Impulse);

        heldRb = null;
        heldItem = null;

        charging = false;
        charge = 0f;
    }
}
