using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource source;

    [Header("Footstep Clip")]
    public AudioClip footsteps;

    [Header("Pitch")]
    [Range(0.6f, 1.4f)] public float crouchPitch = 0.75f;
    [Range(0.6f, 1.4f)] public float walkPitch = 1f;
    [Range(0.6f, 1.4f)] public float runPitch = 1.25f;

    [Header("Volume")]
    [Range(0f, 1f)] public float crouchVolume = 0.3f;
    [Range(0f, 1f)] public float walkVolume = 0.6f;
    [Range(0f, 1f)] public float runVolume = 0.9f;

    [Header("Jump")]
    public AudioClip jump;
    [Range(0f, 2f)] public float jumpVolume = 1.2f;

    public AudioClip doubleJump;
    [Range(0f, 2f)] public float doubleJumpVolume = 1.4f;

    void Awake()
    {
        if (source == null)
            source = GetComponent<AudioSource>();
    }

    void PlayLoop(float pitch, float volume)
    {
        if (source.isPlaying &&
            Mathf.Approximately(source.pitch, pitch) &&
            Mathf.Approximately(source.volume, volume))
            return;

        source.clip = footsteps;
        source.pitch = pitch;
        source.volume = volume;
        source.loop = true;
        source.Play();
    }

    public void PlayCrouch()
    {
        PlayLoop(crouchPitch, crouchVolume);
    }

    public void PlayWalk()
    {
        PlayLoop(walkPitch, walkVolume);
    }

    public void PlayRun()
    {
        PlayLoop(runPitch, runVolume);
    }

    public void StopMove()
    {
        if (source.loop)
            source.Stop();
    }

    public void PlayJump()
    {
        source.pitch = 1f;
        source.PlayOneShot(jump, jumpVolume);
    }

    public void PlayDoubleJump()
    {
        source.pitch = 1f;
        source.PlayOneShot(doubleJump, doubleJumpVolume);
    }
}
