using UnityEngine;
using System.Collections;

public class LaserAlarm : MonoBehaviour
{
    [Header("Laser Parts")]
    public LineRenderer line;
    public Collider triggerCol;

    [Header("Disable Settings")]
    public float defaultDisableDuration = 8f;

    bool isOn = true;
    Coroutine disableCo;
    void Awake()
    {
        if (line == null) line = GetComponent<LineRenderer>();
        if (triggerCol == null) triggerCol = GetComponent<Collider>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (!isOn) return;

        if (other.CompareTag("Player"))
        {
            if (AlarmSystem.Instance != null)
                AlarmSystem.Instance.RaiseAlarm(transform.position);
        }
    }
    public void DisableTemporarily()
    {
        DisableTemporarily(defaultDisableDuration);
    }
    public void DisableTemporarily(float duration)
    {
        if (disableCo != null) StopCoroutine(disableCo);
        disableCo = StartCoroutine(DisableRoutine(duration));
    }
    IEnumerator DisableRoutine(float duration)
    {
        SetLaser(false);
        yield return new WaitForSeconds(duration);
        SetLaser(true);
        disableCo = null;
    }
    void SetLaser(bool state)
    {
        isOn = state;
        if (line != null) line.enabled = isOn;
        if (triggerCol != null)
        {
            triggerCol.enabled = isOn;
        }
    }
}
