using UnityEngine;
using System.Collections;

public class ElevatorButton : MonoBehaviour
{
    public ElevatorControllerXYZ elevator;

    
     // الأوبجكت اللي يختفي (الزر غالبًا)
          // مدة الاختفاء

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (elevator != null)
                elevator.StartElevatorSequence();

           
        }
    }

 
}
