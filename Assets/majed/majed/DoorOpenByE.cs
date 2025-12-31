using UnityEngine;

public class DoorOpenByE : MonoBehaviour
{
    public Animator animator;
    public string animationStateName = "DoorAnim";
    public float interactDistance = 3f;
    public Transform player;
    public GameObject canves;
    private bool isOpen = false;

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        if (dist <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            
           canves.SetActive(true);
            Debug.Log("E key pressed within distance. Playing reverse animation.");
        }
    }

   public void PlayReverseAnimation()
    {
            // تشغيل الأنيميشن بالعكس
            //animator.Play(animationStateName, 0, 1f);

            animator.SetBool("isOpen", true);
            canves.SetActive(false);


    }
}
