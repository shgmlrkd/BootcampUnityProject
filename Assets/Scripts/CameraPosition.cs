using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraPosition : MonoBehaviour
{
    [SerializeField]
    private float sensitive = 60.0f;

    private Transform golfBallTransform;

    private Vector3 camOffset = new Vector3(0.0f, 0.5f, -2.5f);

    private const float MIN_PITCH = -25.0f;
    private const float MAX_PITCH = 60.0f;
    private const float MAX_DISTANCE = 5.0f;
    private const float CAM_Y_OFFSET = 0.15f;

    private float pitch = 0.0f;
    private float yaw = 0.0f;

    private void Awake()
    {
        golfBallTransform = GameObject.Find("GolfBall").transform;
    }

    private void Update()
    {
        UpdateInput();

        Vector3 camPos = CalculateCameraPosition();
        camPos = ApplyCollision(camPos);

        ApplyCamera(camPos);
        DrawDebug();
    }

    private void UpdateInput()
    {
        // 마우스 이동
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // yaw는 y축 rotation 누적, pitch는 x축 rotation 누적
        yaw += mouseX * sensitive * Time.deltaTime;
        pitch += -mouseY * sensitive * Time.deltaTime;

        // x축 회전은 제한을 뒀음
        pitch = Mathf.Clamp(pitch, MIN_PITCH, MAX_PITCH);
    }

    // 쿼터니언으로 바꿔주고 카메라 위치를 계산해서 넘김
    private Vector3 CalculateCameraPosition()
    {
        Quaternion rot = Quaternion.Euler(pitch, yaw, 0.0f);

        return golfBallTransform.position + rot * camOffset;
    }

    private Vector3 ApplyCollision(Vector3 camPos)
    {
        // 카메라 위치에서 골프공으로 향하는 방향벡터
        Vector3 dir = camPos - golfBallTransform.position;

        // MAX_DISTANCE 길이의 레이를 쏴서 레이어가
        // 장애물인 오브젝트에 충돌 했는지, 충돌한 위치는 어디인지 bool, RaycastHit hit.point 반환
        bool hasHit = Physics.Raycast(golfBallTransform.position, dir, 
            out RaycastHit hit, MAX_DISTANCE, LayerMask.GetMask("Obstacle"));

        // 골프공과 카메라의 거리를 구함
        float camDistance = Vector3.Distance(golfBallTransform.position, camPos);

        // 맞았고 거리가 카메라와 골프공 사이의 거리보다 작다면 카메라 위치는 레이 위치
        if (hasHit && camDistance > hit.distance)
        {
            camPos = hit.point;
        }

        return camPos;
    }

    private void ApplyCamera(Vector3 camPos)
    {
        // 계산된 카메라 위치에서 Y값 오프셋을 넣음
        transform.position = camPos + Vector3.up * CAM_Y_OFFSET;
        // 회전값을 직접 넣음
        transform.rotation = Quaternion.Euler(pitch, yaw, 0.0f);
    }

    private void DrawDebug()
    {
        Quaternion rot = Quaternion.Euler(pitch, yaw, 0.0f);

        Vector3 dir = rot * camOffset;

        Debug.DrawRay(golfBallTransform.position, dir * MAX_DISTANCE, Color.red);
    }
}