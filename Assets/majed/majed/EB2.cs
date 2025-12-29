using UnityEngine;
using System.Collections;

public class EB2 : MonoBehaviour
{
    public EC2 elevator;

    [Header("Hide Settings")]
    public GameObject objectToHide;   // الأوبجكت اللي يختفي (الزر غالبًا)
    public float hideTime = 3f;        // مدة الاختفاء

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (elevator != null)
                elevator.StartElevatorSequence();

            if (objectToHide != null)
                StartCoroutine(HideRoutine());
        }
    }

    IEnumerator HideRoutine()
    {
        objectToHide.SetActive(false);          // إخفاء
        yield return new WaitForSeconds(hideTime);
        objectToHide.SetActive(true);           // إرجاع
    }
}
