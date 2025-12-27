using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Settings")]
    public int maxItems = 3;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip pickupSound;
    public AudioClip openSound;
    public AudioClip fullSound;

    List<GameObject> items = new List<GameObject>();

    public bool AddItem(GameObject item)
    {
        if (items.Count >= maxItems)
        {
            if (fullSound) audioSource.PlayOneShot(fullSound);
            return false;
        }

        items.Add(item);
        if (pickupSound) audioSource.PlayOneShot(pickupSound);
        return true;
    }

    public void OpenInventory()
    {
        if (openSound) audioSource.PlayOneShot(openSound);
        Debug.Log("Inventory Opened. Items: " + items.Count);
    }
}
