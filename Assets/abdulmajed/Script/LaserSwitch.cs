using UnityEngine;

public class LaserSwitch : MonoBehaviour
{
    [Header("Turn off all lasers under this parent")]
    public Transform lasersParent;

    [Header("Settings")]
    public KeyCode key = KeyCode.E;
    public float disableTime = 6f;

    bool playerNear;

    void Update()
    {
        if (!playerNear) return;

        if (Input.GetKeyDown(key))
        {
            if (lasersParent == null) return;

            LaserAlarm[] lasers = lasersParent.GetComponentsInChildren<LaserAlarm>(true);
            for (int i = 0; i < lasers.Length; i++)
                lasers[i].DisableTemporarily(disableTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerNear = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerNear = false;
    }
}
