using UnityEngine;

public class PickupInteract : MonoBehaviour
{
   public PickupPromptController prompt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       prompt = FindFirstObjectByType<PickupPromptController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            prompt?.ShowPickup();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            prompt?.HidePickup();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
