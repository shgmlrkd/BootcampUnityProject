using UnityEngine;

public class BallState : MonoBehaviour
{
    private Ball ball;

    private bool prevmove = false;
    private void Awake()
    {
        ball = GetComponent<Ball>();
    }

    private void Update()
    {
        bool ismoving = IsMoving();

        if(prevmove != IsMoving())
        {
            print(ismoving ? "공 움직이는 중" : "공 멈춤");
            
            prevmove = IsMoving();
        }
    }

    public bool IsMoving()
    {
        return ball.Rb.linearVelocity.sqrMagnitude > 0.001f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            GameManager.Instance.StageClear();
        }

        if(other.CompareTag("Fall"))
        {
            GameManager.Instance.ResetBall();
        }
    }
}