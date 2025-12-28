using UnityEngine;

public class ElevatorButton : MonoBehaviour
{
    private Slided elevator;
    private bool playerNear = false;

    void Start()
    {
        elevator = FindFirstObjectByType<Slided>();
    }

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            elevator.StartMove(); // ✅ الاسم الصحيح
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = false;
    }
}
