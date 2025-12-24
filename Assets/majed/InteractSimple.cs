using UnityEngine;

public class InteractSimple : MonoBehaviour
{
    public Transform player;          // اللاعب
    public float interactDistance = 2f;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            TakeObject();
        }
    }

    void TakeObject()
    {
        Debug.Log("Object Taken");
        gameObject.SetActive(false); // يخفي الأوبجكت
    }
}
