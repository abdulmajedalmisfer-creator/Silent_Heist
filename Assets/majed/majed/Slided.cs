using UnityEngine;

public class Slided : MonoBehaviour
{
    [Header("Positions")]
    public Vector3 pointA;   // البداية
    public Vector3 pointB;   // النهاية

    [Header("Movement")]
    public float speed = 2f;

    [Header("Rotation")]
    [Range(-180f, 180f)]
    public float rotateY = 0f;   // الدوران حول Y (سلايدر)

    private Vector3 targetPos;
    private Quaternion startRot;
    private Quaternion targetRot;
    private bool isMoving = false;

    void Start()
    {
        transform.position = pointA;
        targetPos = pointB;

        startRot = transform.rotation;
        targetRot = Quaternion.Euler(
            transform.eulerAngles.x,
            rotateY,
            transform.eulerAngles.z
        );
    }

    void Update()
    {
        if (!isMoving) return;

        // الحركة
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );

        // الدوران (ناعم)
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            speed * 50f * Time.deltaTime
        );

        // إذا وصل → يوقف
        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            isMoving = false;
        }
    }

    // يناديها زر E
    public void StartMove()
    {
        isMoving = true;
    }
}
