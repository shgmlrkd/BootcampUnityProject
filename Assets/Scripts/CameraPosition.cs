using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    private Transform golfBallTransform;

    private Vector3 camOffset = new Vector3(0.0f, 0.5f, -2.5f);

    private float sensitive = 60.0f;

    private float pitch = 0.0f;
    private float yaw = 0.0f;

    private void Awake()
    {
        golfBallTransform = GameObject.Find("GolfBall").transform;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        pitch += mouseY * sensitive * Time.deltaTime;
        yaw += mouseX * sensitive * Time.deltaTime;

        transform.position = golfBallTransform.position + camOffset;
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
    }
}
