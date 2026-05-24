using UnityEngine;

public class Boost : MonoBehaviour
{
    [SerializeField]
    private float boostForce = 1000.0f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            Ball ball = other.GetComponent<Ball>();

            ball.Rb.AddForce(-transform.forward * boostForce);
        }
    }
}