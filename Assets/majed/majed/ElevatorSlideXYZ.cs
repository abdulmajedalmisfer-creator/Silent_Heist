using UnityEngine;

public class ElevatorSlideXYZ : MonoBehaviour
{
    [Header("Positions")]
    public Vector3 pointA;   // Position البداية
    public Vector3 pointB;   // Position النهاية

    [Header("Rotations (X Y Z)")]
    public Vector3 rotationA; // Rotation البداية
    public Vector3 rotationB; // Rotation النهاية

    [Header("Speed")]
    public float moveSpeed = 2f;
    public float rotateSpeed = 120f;

    private bool isMoving = false;

    void Start()
    {
        // نثبت نقطة البداية
        transform.position = pointA;
        transform.rotation = Quaternion.Euler(rotationA);
    }

    void Update()
    {
        if (!isMoving) return;

        // 🔹 الحركة
        transform.position = Vector3.MoveTowards(
            transform.position,
            pointB,
            moveSpeed * Time.deltaTime
        );

        // 🔹 الدوران (X Y Z)
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.Euler(rotationB),
            rotateSpeed * Time.deltaTime
        );

        // ⛔ يوقف عند الوصول
        if (
            Vector3.Distance(transform.position, pointB) < 0.05f &&
            Quaternion.Angle(transform.rotation, Quaternion.Euler(rotationB)) < 0.5f
        )
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
