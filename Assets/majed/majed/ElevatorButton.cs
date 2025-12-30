using UnityEngine;
using System.Collections;

public class ElevatorButton : MonoBehaviour
{
    public ElevatorControllerXYZ elevator;

    
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (elevator != null)
                elevator.StartElevatorSequence();

           
        }
    }

 
}
