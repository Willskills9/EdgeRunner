using UnityEngine;

public class HelicopterManager : MonoBehaviour
{
    public GameObject target;
    public float moveSpeed = 2f;
    public Transform rotatingChild;
    public float rotationSpeed = 5f;

    public float fixedY = 60f;

    void Update()
    {
        if (target == null) return;

        // Current position with fixed Z
        Vector3 currentPos = new Vector3(transform.position.x, fixedY, transform.position.z);

        // Target position but force Z = 100
        Vector3 targetPos = new Vector3(target.transform.position.x, fixedY, target.transform.position.z);

        // Move toward the target
        Vector3 newPos = Vector3.MoveTowards(currentPos, targetPos, moveSpeed * Time.deltaTime);

        // Apply position
        transform.position = newPos;

        if (rotatingChild != null)
        {
            Vector3 dir = target.transform.position - rotatingChild.position;

            if (dir.sqrMagnitude > 0.0001f)
            {
                Quaternion desiredRot = Quaternion.LookRotation(dir, Vector3.up);

                // Angle between where the child is facing and where it wants to face
                float angleToTarget = Quaternion.Angle(rotatingChild.rotation, desiredRot);

                float maxAngle = 60f; // the cone limit

                Quaternion targetRot;

                if (angleToTarget > maxAngle)
                {
                    // Clamp the rotation to stay inside the 60-degree cone
                    targetRot = Quaternion.RotateTowards(
                    rotatingChild.rotation,
                    desiredRot,
                    maxAngle
                    );
                }
                else
                {
                    // Target is inside cone; rotate normally
                    targetRot = desiredRot;
                }

                // Smooth rotation toward the allowed orientation
                rotatingChild.rotation = Quaternion.Slerp(
                rotatingChild.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
                );
            }
        }
    }
}
