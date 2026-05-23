using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody Rb { get; private set; }

    public BallMove Move { get; private set; }
    public BallState State { get; private set; }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();

        Move = GetComponent<BallMove>();
        State = GetComponent<BallState>();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Ball = this;
        }
    }

    public void ResetPosition(Vector3 pos)
    {
        Rb.linearVelocity = Vector3.zero;
        Rb.angularVelocity = Vector3.zero;

        transform.position = pos;
    }
}