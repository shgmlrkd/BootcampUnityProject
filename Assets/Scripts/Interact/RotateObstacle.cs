using UnityEngine;

public class RotateObstacle : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotateDirection = Vector3.up;

    [SerializeField]
    private float rotateSpeed = 50.0f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 rotationDelta = rotateDirection * rotateSpeed * Time.fixedDeltaTime;

        Quaternion targetRotation = Quaternion.Euler(rotationDelta);

        rb.MoveRotation(rb.rotation * targetRotation);
    }
}
