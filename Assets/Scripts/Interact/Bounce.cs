using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField]
    private float bounceForce = 1000.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Ball ball = other.GetComponent<Ball>();

            ball.Rb.AddForce(Vector3.up * bounceForce);
        }
    }
}
