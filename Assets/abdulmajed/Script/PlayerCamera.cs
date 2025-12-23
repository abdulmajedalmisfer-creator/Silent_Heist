using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public Transform cam;
    Vector2 input;
    public float speed;
    float xRotation = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxis("Mouse X") * speed;
        input.y = Input.GetAxis("Mouse Y") * speed;

        xRotation -= input.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * input.x);

    }

}
