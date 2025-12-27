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

    [Header("Audio")]
    public PlayerAudio playerAudio;

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

        // Ground check
        isGrounded = Physics.CheckSphere(
            transform.position + Vector3.up * sphereOffset,
            sphereRadius,
            groundLayer
        );

        if (isGrounded)
            canDoubleJump = true;

        if (!isCrouching && Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                DoJump(false);
            }
            else if (canDoubleJump)
            {
                DoJump(true);
                canDoubleJump = false;
            }
        }

        // Toggle crouch (Ctrl)
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!isCrouching) StartCrouch();
            else StopCrouch();
        }

        HandleMovementAudio();
    }

    void FixedUpdate()
    {
        float currentSpeed = speed;

     
        if (!isCrouching && Input.GetKey(KeyCode.LeftShift))
            currentSpeed = sprintSpeed;

        if (isCrouching)
            currentSpeed *= crouchSpeedMultiplier;

        Vector3 move = (transform.right * h + transform.forward * v) * currentSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }

    void DoJump(bool doubleJump)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        if (playerAudio == null) return;

        if (doubleJump)
            playerAudio.PlayDoubleJump();
        else
            playerAudio.PlayJump();
    }

    void HandleMovementAudio()
    {
        if (playerAudio == null)
            return;

        if (isCrouching || !isGrounded)
        {
            playerAudio.StopMove();
            return;
        }

        bool isMoving = Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;

        if (!isMoving)
        {
            playerAudio.StopMove();
            return;
        }

        if (Input.GetKey(KeyCode.LeftShift))
            playerAudio.PlayRun();
        else
            playerAudio.PlayWalk();
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
