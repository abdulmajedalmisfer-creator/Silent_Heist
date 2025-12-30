using UnityEngine;

public class dd : MonoBehaviour
{
    public ElevatorControllerXYZ elevator;
    public acc access;   // نربط كلاس الكرت

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (access.accessCard)
            {
                elevator.StartElevatorSequence();
            }
            else
            {
                Debug.Log("You need access card");
            }
        }
    }
}
