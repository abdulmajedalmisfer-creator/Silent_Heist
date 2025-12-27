using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("Hold / Pickup")]
    public Transform holdPoint;
    public float pickupRange = 2.5f;
    public LayerMask pickupMask;

    [Header("Throw")]
    public float throwMinForce = 6f;
    public float throwMaxForce = 18f;
    public float chargeTimeToMax = 1.2f;

    [Header("Keys")]
    public KeyCode holdKey = KeyCode.E;    
    public KeyCode storeKey = KeyCode.I;   

    Rigidbody heldRb;
    PickupItem heldItem;

    bool charging;
    float charge;

    Inventory inventory;

    void Start()
    {
        inventory = GetComponent<Inventory>(); 
    }

    void Update()
    {
        if (Input.GetKeyDown(holdKey))
        {
            if (heldRb == null) TryPickup();
            else Drop();
        }
        if (heldRb != null && Input.GetKeyDown(storeKey))
        {
            StoreHeldItem();
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

        heldRb.position = holdPoint.position;
        heldRb.rotation = holdPoint.rotation;
        heldRb.linearVelocity = Vector3.zero;
        heldRb.angularVelocity = Vector3.zero;
        heldRb.useGravity = false;
    }

    void Drop()
    {
        if (heldRb == null) return;

        heldItem.SetHeld(false);
        heldRb.useGravity = true;

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

        item.SetHeld(false);
        rb.useGravity = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.position = holdPoint.position + holdPoint.forward * 0.35f;
        rb.AddForce(holdPoint.forward * force, ForceMode.Impulse);

        heldRb = null;
        heldItem = null;

        charging = false;
        charge = 0f;
    }

    void StoreHeldItem()
    {
        if (inventory == null || heldItem == null || heldRb == null) return;

        GameObject go = heldItem.gameObject;
        bool added = inventory.AddItem(go);
        if (!added) return;
        heldItem.SetHeld(false);
        heldRb.useGravity = true;
        heldRb = null;
        heldItem = null;
        charging = false;
        charge = 0f;
        go.SetActive(false);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
