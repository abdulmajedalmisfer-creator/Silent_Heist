using UnityEngine;

public class AlarmSystem : MonoBehaviour
{
    public static AlarmSystem Instance;
    public float cooldown = 1.5f;  
    float lastTime = -999f;
    void Awake()
    {
        Instance = this;
    }
    public void RaiseAlarm(Vector3 pos)
    {
        Debug.Log("Alarm raised at position: " + pos);
        if (Time.time - lastTime < cooldown) return;
        lastTime = Time.time;
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
