using UnityEngine;

public class Ramp : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 downPos;
    private Vector3 upPos;

    private float upHeight = 1.1f;

    private float upSpeed = 12.0f; 
    private float downSpeed = 2.0f;

    private float waitTime = 1.6f;

    private bool isUp = false;

    private float timer = 0.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        downPos = rb.position;
        upPos = downPos + Vector3.up * upHeight;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer < waitTime)
        {
            return;
        }

        Vector3 target = isUp ? downPos : upPos;
        float speed = isUp ? downSpeed : upSpeed;

        rb.MovePosition(Vector3.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime));

        if (Vector3.Distance(rb.position, target) < 0.01f)
        {
            isUp = !isUp;
            timer = 0.0f;
        }
    }
}