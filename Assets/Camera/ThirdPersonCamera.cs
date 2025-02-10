using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Tooltip("Transform (usually the player) the camera follows.")]
    public Transform target;

    [Tooltip("Offset from the target.")]
    public Vector3 offset = new Vector3(0, 5, -10);

    [Tooltip("Speed for smoothing the camera's movement.")]
    public float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        if (!target) return;

        // Desired camera position
        Vector3 desiredPosition = target.position + offset;
        // Smoothly interpolate between current and desired
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
        // Look at the target's position (center)
        transform.LookAt(target.position);
    }
}