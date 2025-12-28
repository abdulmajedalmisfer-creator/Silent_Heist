using UnityEngine;

public class ElevatorButton : MonoBehaviour
{
    private ElevatorSlideXYZ elevator; // الكلاس الجديد
    private bool playerNear = false;

    void Start()
    {
        // نبحث عن المصعد في المشهد
        elevator = FindFirstObjectByType<ElevatorSlideXYZ>();

        if (elevator == null)
        {
            Debug.LogError("❌ لم يتم العثور على ElevatorSlideXYZ في المشهد");
        }
    }

    void Update()
    {
        if (!playerNear || elevator == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            elevator.StartMove();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = false;
    }
}
