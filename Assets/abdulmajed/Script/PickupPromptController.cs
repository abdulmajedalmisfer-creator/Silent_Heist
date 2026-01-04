using UnityEngine;

public class PickupPromptController : MonoBehaviour
{
    public GameObject PickupPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PickupPanel == null)
            PickupPanel.SetActive(false);
    }
    public void ShowPickup() => PickupPanel?.SetActive(true);
    public void HidePickup() => PickupPanel?.SetActive(false);


    void Update()
    {
        
    }
}
