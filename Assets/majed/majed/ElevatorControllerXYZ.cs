using UnityEngine;

public class ElevatorControllerXYZ : MonoBehaviour
{
    [Header("Elevator Positions")]
    public Vector3 pointA;
    public Vector3 pointB;

    [Header("Elevator Rotations")]
    public Vector3 rotA;
    public Vector3 rotB;

    [Header("Doors")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("Door Positions")]
    public Vector3 leftClosedPos;
    public Vector3 leftOpenPos;
    public Vector3 rightClosedPos;
    public Vector3 rightOpenPos;

    [Header("Door Rotations")]
    public Vector3 leftClosedRot;
    public Vector3 leftOpenRot;
    public Vector3 rightClosedRot;
    public Vector3 rightOpenRot;

    [Header("Speeds")]
    public float moveSpeed = 2f;
    public float rotateSpeed = 120f;
    public float doorSpeed = 2f;
    public float doorRotateSpeed = 120f;

    [Header("Timings")]
    public float doorWaitTime = 3f;

    private enum State
    {
        IdleAtA,
        OpeningAtA,
        WaitingOpenAtA,
        ClosingAtA,
        MovingToB,
        OpeningAtB,
        WaitingOpenAtB,
        ClosingAtB,
        MovingToA
    }

    private State state = State.IdleAtA;
    private float timer;

    void Start()
    {
        transform.position = pointA;
        transform.rotation = Quaternion.Euler(rotA);

        leftDoor.localPosition = leftClosedPos;
        leftDoor.localRotation = Quaternion.Euler(leftClosedRot);

        rightDoor.localPosition = rightClosedPos;
        rightDoor.localRotation = Quaternion.Euler(rightClosedRot);
    }

    void Update()
    {
        switch (state)
        {
            case State.OpeningAtA:
                if (MoveDoors(leftOpenPos, leftOpenRot, rightOpenPos, rightOpenRot))
                {
                    state = State.WaitingOpenAtA;
                    timer = 0f;
                }
                break;

            case State.WaitingOpenAtA:
                timer += Time.deltaTime;
                if (timer >= doorWaitTime)
                    state = State.ClosingAtA;
                break;

            case State.ClosingAtA:
                if (MoveDoors(leftClosedPos, leftClosedRot, rightClosedPos, rightClosedRot))
                    state = State.MovingToB;
                break;

            case State.MovingToB:
                MoveElevator(pointB, rotB);
                if (Reached(transform.position, pointB))
                    state = State.OpeningAtB;
                break;

            case State.OpeningAtB:
                if (MoveDoors(leftOpenPos, leftOpenRot, rightOpenPos, rightOpenRot))
                {
                    state = State.WaitingOpenAtB;
                    timer = 0f;
                }
                break;

            case State.WaitingOpenAtB:
                timer += Time.deltaTime;
                if (timer >= doorWaitTime)
                    state = State.ClosingAtB;
                break;

            case State.ClosingAtB:
                if (MoveDoors(leftClosedPos, leftClosedRot, rightClosedPos, rightClosedRot))
                    state = State.MovingToA;
                break;

            case State.MovingToA:
                MoveElevator(pointA, rotA);
                if (Reached(transform.position, pointA))
                {
                    state = State.OpeningAtA;   // ✅ التغيير هنا
                    timer = 0f;
                }
                break;
        }
    }

    // ===== فتح الباب يدويًا =====
    public void OpenDoorsManual()
    {
        if (state == State.IdleAtA)
            state = State.OpeningAtA;
    }

    // ===== تشغيل المصعد =====
    public void StartElevatorSequence()
    {
        if (state == State.IdleAtA)
            state = State.OpeningAtA;
    }

    // ===== Helpers =====
    void MoveElevator(Vector3 targetPos, Vector3 targetRot)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.Euler(targetRot),
            rotateSpeed * Time.deltaTime
        );
    }

    bool MoveDoors(
        Vector3 leftTargetPos, Vector3 leftTargetRot,
        Vector3 rightTargetPos, Vector3 rightTargetRot
    )
    {
        leftDoor.localPosition = Vector3.MoveTowards(
            leftDoor.localPosition,
            leftTargetPos,
            doorSpeed * Time.deltaTime
        );

        leftDoor.localRotation = Quaternion.RotateTowards(
            leftDoor.localRotation,
            Quaternion.Euler(leftTargetRot),
            doorRotateSpeed * Time.deltaTime
        );

        rightDoor.localPosition = Vector3.MoveTowards(
            rightDoor.localPosition,
            rightTargetPos,
            doorSpeed * Time.deltaTime
        );

        rightDoor.localRotation = Quaternion.RotateTowards(
            rightDoor.localRotation,
            Quaternion.Euler(rightTargetRot),
            doorRotateSpeed * Time.deltaTime
        );

        return
            Vector3.Distance(leftDoor.localPosition, leftTargetPos) < 0.01f &&
            Quaternion.Angle(leftDoor.localRotation, Quaternion.Euler(leftTargetRot)) < 0.5f &&
            Vector3.Distance(rightDoor.localPosition, rightTargetPos) < 0.01f &&
            Quaternion.Angle(rightDoor.localRotation, Quaternion.Euler(rightTargetRot)) < 0.5f;
    }

    bool Reached(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b) < 0.01f;
    }
}
