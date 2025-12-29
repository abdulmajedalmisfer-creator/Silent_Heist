using UnityEngine;

public class PanelReadTrigger : MonoBehaviour
{
    public GameObject infoPanel;
    public GameObject pressEPrompt;

    public AudioSource narrationSource;
    public AudioClip narrationClip;

    private bool playerInRange = false;
    private bool panelOpen = false;

    void Start()
    {
        infoPanel.SetActive(false);
        pressEPrompt.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!panelOpen)
                OpenPanel();
            else
                ClosePanel();
        }
    }

    void OpenPanel()
    {
        panelOpen = true;

        infoPanel.SetActive(true);
        pressEPrompt.SetActive(false);

        Time.timeScale = 0f; 

        if (narrationSource && narrationClip)
        {
            narrationSource.clip = narrationClip;
            narrationSource.Play();
        }
    }

    void ClosePanel()
    {
        panelOpen = false;

        infoPanel.SetActive(false);
        pressEPrompt.SetActive(true);

        Time.timeScale = 1f; 

        if (narrationSource)
            narrationSource.Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!panelOpen)
                pressEPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pressEPrompt.SetActive(false);

            if (panelOpen)
                ClosePanel();
        }
    }
}
