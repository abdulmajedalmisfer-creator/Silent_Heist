using UnityEngine;

public class NoiseOnImpact : MonoBehaviour
{
    public float cooldown = 0.3f;
    float lastTime = -999f;

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.time - lastTime < cooldown) return;
        lastTime = Time.time;

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].HearNoise(transform.position);
        }
    }
}


