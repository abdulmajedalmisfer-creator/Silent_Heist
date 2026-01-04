using UnityEngine;

public class AlarmSystem : MonoBehaviour
{
    public static AlarmSystem Instance;

    [Header("Alarm Settings")]
    public float cooldown = 1.5f;

    [Header("Sound")]
    public AudioSource alarmSource;  
    public AudioClip alarmClip;        

    float lastTime = -999f;

    void Awake()
    {
        Instance = this;
    }
    public void RaiseAlarm(Vector3 pos)
    {
    
        if (Time.time - lastTime < cooldown)
            return;

        lastTime = Time.time;

        Debug.Log("Alarm raised at position: " + pos);

     
        if (alarmSource != null && alarmClip != null)
        {
            alarmSource.PlayOneShot(alarmClip);
        }       
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Enemy nearest = null;
        float bestDist = Mathf.Infinity;

        foreach (Enemy e in enemies)
        {
            float d = Vector3.Distance(pos, e.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                nearest = e;
            }
        }
        if (nearest != null)
            nearest.RespondToAlarm(pos);
    }
}
