using UnityEngine;

public class Boost : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            Ball ball = other.GetComponent<Ball>();

            ball.Rb.AddForce(-transform.forward * 100.0f);
        }
    }
}