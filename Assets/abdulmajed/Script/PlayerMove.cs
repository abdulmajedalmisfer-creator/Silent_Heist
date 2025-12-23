using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Ground Check")]
    public float sphereOffset;
    public float sphereRadius;
    public LayerMask groundLayer;

    [Header("Movement")]
    public float speed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeedMultiplier = 0.6f;

    [Header("Jump")]
    public float jumpForce = 5f;

    [Header("Crouch")]
    public float standHeight = 2f;
    public float crouchHeight = 1.2f;

    private Rigidbody rb;
    private CapsuleCollider col;

    float h;
    float v;

    private bool isGrounded;
    private bool doubleJump;
    private bool isCrouching;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.up * sphereOffset, sphereRadius);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

        standHeight = col.height;
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

       
        isGrounded = Physics.CheckSphere(
            transform.position + transform.up * sphereOffset,
            sphereRadius,
            groundLayer
        );

        if (isGrounded)
        {
            doubleJump = true;
        }

        // Jump + Double Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
        else if (Input.GetButtonDown("Jump") && !isGrounded && doubleJump)
        {
            Jump();
            doubleJump = false;
        }

        // Toggle Crouch (Ctrl)
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!isCrouching) StartCrouch();
            else StopCrouch();
        }
    }

    void FixedUpdate()
    {
        float currentSpeed = speed;

        // Sprint (Shift)
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            currentSpeed = sprintSpeed;
        }

        // Crouch speed
        if (isCrouching)
        {
            currentSpeed *= crouchSpeedMultiplier;
        }

        Vector3 movement = (transform.right * h + transform.forward * v) * currentSpeed;
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void StartCrouch()
    {
        isCrouching = true;
        col.height = crouchHeight;
        col.center = new Vector3(0, crouchHeight / 2f, 0);
    }

    void StopCrouch()
    {
        isCrouching = false;
        col.height = standHeight;
        col.center = new Vector3(0, standHeight / 2f, 0);
    }
}
