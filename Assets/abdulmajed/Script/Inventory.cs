using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Settings")]
    public int maxItems = 12;

    [Header("UI")]
    public GameObject inventoryPanel;     // InventoryPanel
    public Image[] slotIcons;             // اسحب هنا ItemIcon حق كل Slot (عددها = عدد السلوت)

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip pickupSound;
    public AudioClip openSound;
    public AudioClip fullSound;

    private List<GameObject> items = new List<GameObject>();

    void Start()
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        ClearAllSlotsUI();
    }

    public bool AddItem(GameObject item, Sprite icon)
    {
        if (items.Count >= maxItems)
        {
            if (fullSound && audioSource) audioSource.PlayOneShot(fullSound);
            return false;
        }

        int slotIndex = GetFirstEmptySlotIndex();
        if (slotIndex == -1)
        {
            if (fullSound && audioSource) audioSource.PlayOneShot(fullSound);
            return false;
        }

        items.Add(item);

        // حط الايقونة في أول سولت فاضي
        if (slotIcons[slotIndex] != null)
        {
            slotIcons[slotIndex].sprite = icon;
            slotIcons[slotIndex].enabled = true;      // ✅ مهم
            var c = slotIcons[slotIndex].color;
            c.a = 1f;                                  // ✅ مهم
            slotIcons[slotIndex].color = c;
        }

        if (pickupSound && audioSource) audioSource.PlayOneShot(pickupSound);
        return true;
    }



    public void ToggleInventory()
    {
        if (inventoryPanel == null) return;

        bool newState = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(newState);

        if (newState && openSound && audioSource)
            audioSource.PlayOneShot(openSound);
    }

    int GetFirstEmptySlotIndex()
    {
        for (int i = 0; i < slotIcons.Length; i++)
        {
            if (slotIcons[i] == null) continue;

            // فاضي إذا ما فيه sprite (حتى لو enabled false)
            if (slotIcons[i].sprite == null)
                return i;
        }
        return -1;
    }

    void SetSlotIcon(int index, Sprite icon)
    {
        if (index < 0 || index >= slotIcons.Length) return;

        Image img = slotIcons[index];
        if (img == null) return;

        img.sprite = icon;

        // خلها تظهر
        img.enabled = true;
        Color c = img.color;
        c.a = 1f;
        img.color = c;
    }

    void ClearAllSlotsUI()
    {
        if (slotIcons == null) return;

        for (int i = 0; i < slotIcons.Length; i++)
        {
            if (slotIcons[i] == null) continue;

            slotIcons[i].sprite = null;

            // نخليها مخفية بالبداية
            slotIcons[i].enabled = false;
            Color c = slotIcons[i].color;
            c.a = 0f;
            slotIcons[i].color = c;
        }
    }
}