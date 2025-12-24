using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GuardInteract : MonoBehaviour
{
    [Header("References")]
    public Transform player;              // اللاعب
    public GameObject ticketObject;       // التذكرة
    public TextMeshProUGUI messageText;   // نص الشاشة
    public Image stickerImage;            // صورة الستكر

    [Header("Settings")]
    public float interactDistance = 6f;
    public float messageDuration = 15f;

    bool isShowing = false;

    void Update()
    {
        if (player == null || messageText == null || stickerImage == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        if (dist <= interactDistance && Input.GetKeyDown(KeyCode.E) && !isShowing)
        {
            if (ticketObject == null || !ticketObject.activeInHierarchy)
            {
                ShowMessage("🎟️ -500 RIYAL\nYou are welcome to enter, Sir");
                stickerImage.gameObject.SetActive(true);
            }
            else
            {
                ShowMessage("⏰ Do you have a ticket?\nYou are already late.\nThe gate will close in 5 minutes.");
            }
        }
    }

    void ShowMessage(string msg)
    {
        StartCoroutine(MessageRoutine(msg));
    }

    IEnumerator MessageRoutine(string msg)
    {
        isShowing = true;

        // شغّل النص والصورة
        messageText.text = msg;
        messageText.gameObject.SetActive(true);
        

        yield return new WaitForSeconds(messageDuration);

        // طفّيهم
        messageText.gameObject.SetActive(false);
        stickerImage.gameObject.SetActive(false);

        isShowing = false;
    }
}
