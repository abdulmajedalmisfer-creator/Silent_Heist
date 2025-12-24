using UnityEngine;
using TMPro;
using System.Collections;

public class GuardInteract : MonoBehaviour
{
    [Header("References")]
    public Transform player;              // اللاعب
    public GameObject ticketObject;       // التذكرة (GameObject بالمشهد)
    public TextMeshProUGUI messageText;   // نص الشاشة (TMP)

    [Header("Settings")]
    public float interactDistance = 2.5f;
    public float messageDuration = 3f;

    bool isShowing = false;

    void Update()
    {
        if (player == null || messageText == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        if (dist <= interactDistance && Input.GetKeyDown(KeyCode.E) && !isShowing)
        {
            if (ticketObject == null || !ticketObject.activeInHierarchy)
                ShowMessage("🎟️ -500 RIYAL  you are welcome to enter Sir ");
            else
                ShowMessage("⏰ DO You have a ticket ? AND you are Alrady LATE TO ENTER 5 MINUTES and the GATE WILL BE CLOSED");
        }
    }

    void ShowMessage(string msg)
    {
        StartCoroutine(MessageRoutine(msg));
    }

    IEnumerator MessageRoutine(string msg)
    {
        isShowing = true;

        messageText.text = msg;
        messageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(messageDuration);

        messageText.gameObject.SetActive(false);
        isShowing = false;
    }
}
