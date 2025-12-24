using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Ground Check")]
    public float sphereOffset = 0.1f;
    public float sphereRadius = 0.3f;
    public LayerMask groundLayer;

    [Header("Movement")]
    public float speed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeedMultiplier = 0.6f;

    [Header("Jump")]
    public float jumpForce = 5f;

    [Header("Crouch")]
    public float crouchHeight = 1.2f;

    Rigidbody rb;
    CapsuleCollider col;

    float h;
    float v;

    bool isGrounded;
    bool canDoubleJump;
    bool isCrouching;

    float standHeight;
    Vector3 standCenter;
    Vector3 crouchCenter;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

        rb.freezeRotation = true;

        standHeight = col.height;
        standCenter = col.center;
        crouchCenter = new Vector3(0, crouchHeight / 2f, 0);
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        // Ground check (Sphere)
        isGrounded = Physics.CheckSphere(
            transform.position + Vector3.down * sphereOffset,
            sphereRadius,
            groundLayer
        );

        if (isGrounded && rb.linearVelocity.y < 0) 
        {
            canDoubleJump = true;
        }

        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                DoJump();
            }
            else if (canDoubleJump)
            {
                DoJump();
                canDoubleJump = false;
            }
        }

        // Toggle crouch (Ctrl)
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!isCrouching) StartCrouch();
            else StopCrouch();
        }
    }

    void FixedUpdate()
    {
        float currentSpeed = speed;

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
            currentSpeed = sprintSpeed;

        if (isCrouching)
            currentSpeed *= crouchSpeedMultiplier;

        Vector3 move = (transform.right * h + transform.forward * v) * currentSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }

    void DoJump()
    {
   
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void StartCrouch()
    {
        isCrouching = true;
        col.height = crouchHeight;
        col.center = crouchCenter;
    }

    void StopCrouch()
    {
        isCrouching = false;
        col.height = standHeight;
        col.center = standCenter;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position + Vector3.up * sphereOffset,
            sphereRadius
        );
    }
}
