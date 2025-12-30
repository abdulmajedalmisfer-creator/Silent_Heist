using UnityEngine;

public class acc : MonoBehaviour
{


    public bool accessCard = false;

    public GameObject accessCardObject; // الكرت في المشهد

    void Update()
    {
       
    }

    // لما اللاعب يلمس الكرت
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            accessCard = true;                 // صار معه كرت
            accessCardObject.SetActive(false); // إخفاء الكرت
            Debug.Log("Access card collected");
        }
    }
}
