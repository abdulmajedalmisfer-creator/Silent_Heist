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

    Collider[] heldColliders;
    bool prevUseGravity;
    bool prevKinematic;

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
        if (Input.GetKeyDown(storeKey))
        {
            if (heldRb != null) StoreHeldItem();
            else if (inventory != null) inventory.ToggleInventory();
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
    void TryPickup()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange, pickupMask, QueryTriggerInteraction.Ignore);
        if (hits.Length == 0) return;

        Collider best = null;
        float bestDist = float.MaxValue;

        foreach (var h in hits)
        {
            float d = Vector3.Distance(transform.position, h.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = h;
            }
        }

        if (best == null)
            return;
        Rigidbody rb = best.attachedRigidbody;
        if (rb == null)
            return;

        PickupItem item = rb.GetComponentInParent<PickupItem>();
        if (item == null) return;

        heldRb = rb;
        heldItem = item;
        heldItem.SetHeld(true);

        prevUseGravity = rb.useGravity;
        prevKinematic = rb.isKinematic;
        heldColliders = rb.GetComponentsInChildren<Collider>();
        foreach (var c in heldColliders)
            c.enabled = false;

        rb.useGravity = false;
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.transform.SetParent(holdPoint, false);
        rb.transform.localPosition = Vector3.zero;
        rb.transform.localRotation = Quaternion.identity;
    }

    void Drop()
    {
        if (heldRb == null) return;
        heldItem.SetHeld(false);
        heldRb.transform.SetParent(null, true);
        if (heldColliders != null)
        {
            foreach (var c in heldColliders)
                c.enabled = true;
        }
        heldRb.isKinematic = prevKinematic;
        heldRb.useGravity = prevUseGravity;

        Cleanup();
    }

    void Throw()
    {
        if (heldRb == null) return;
        float t = Mathf.Clamp01(charge / chargeTimeToMax);
        float force = Mathf.Lerp(throwMinForce, throwMaxForce, t);
        heldItem.SetHeld(false);
        heldRb.transform.SetParent(null, true);

        if (heldColliders != null)
        {
            foreach (var c in heldColliders)
                c.enabled = true;
        }
        heldRb.isKinematic = false;
        heldRb.useGravity = true;

        heldRb.linearVelocity = Vector3.zero;
        heldRb.angularVelocity = Vector3.zero;

        heldRb.position = holdPoint.position + holdPoint.forward * 0.35f;
        heldRb.AddForce(holdPoint.forward * force, ForceMode.Impulse);

        Cleanup();
    }
    void StoreHeldItem()
    {
        if (inventory == null || heldItem == null) return;

        InventoryItemData data = heldItem.GetComponentInParent<InventoryItemData>();
        if (data == null || !data.canStore) return;

        bool added = inventory.AddItem(heldItem.gameObject, data.icon);
        if (!added) return;

        heldItem.SetHeld(false);
        heldRb.transform.SetParent(null, true);

        Cleanup();
        data.gameObject.SetActive(false);
    }

    void Cleanup()
    {
        heldRb = null;
        heldItem = null;
        heldColliders = null;
        charging = false;
        charge = 0f;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
