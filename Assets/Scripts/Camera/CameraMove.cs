using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    private CameraController cameraController;
    private Transform ball;
    private BallState ballState;

    private Vector3 offset = new Vector3(0.0f, 0.5f, -3.5f);

    private const float MIN_PITCH = -25.0f;
    private const float MAX_PITCH = 60.0f;

    private const float MAX_DISTANCE = 5.0f;

    private const float CAM_Y_OFFSET = 0.25f;

    public float Sensitive { get; set; } = 100.0f;

    private float pitch;
    private float yaw;

    private float lockPitch;
    private float lockYaw;

    private void Awake()
    {
        cameraController = GetComponent<CameraController>();
        ball = GameObject.Find("GolfBall").transform;
        ballState = ball.GetComponent<BallState>();
    }

    private void Update()
    {
        // 카메라 상태 업데이트
        UpdateState();

        // Aim이면 마우스 움직이는대로 카메라 이동 계산
        if (cameraController.State.Current != CameraState.State.Aim)
        {
            UpdateInput();
        }

        // 카메라 위치 받기
        Vector3 camPos = CalculateCameraPosition();

        // 지형지물에 카메라가 충돌한다면 계산해서
        // 투과되지 않은 위치로 수정
        camPos = ApplyCollision(camPos);

        // 최종적으로 position, rotation을 변경
        ApplyCamera(camPos);

        DrawDebug();
    }
    private void UpdateState()
    {
        // 좌클릭 중이면 상태는 Aim
        if (!IsUIInput() && Input.GetMouseButton(0))
        {
            cameraController.State.Change(CameraState.State.Aim);

            lockPitch = pitch;
            lockYaw = yaw;

            return;
        }

        // 공이 움직이면 상태는 Follow
        if (ballState.IsMoving())
        {
            cameraController.State.Change(CameraState.State.Follow);
            return;
        }

        // 둘다 아닐 경우 FreeLook
        cameraController.State.Change(CameraState.State.FreeLook);
    }

    private void UpdateInput()
    {
        // 우클릭 누를 때만 카메라 회전
        if (!Input.GetMouseButton(1))
        {
            return;
        }

        // 마우스 이동
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // yaw는 y축 rotation 누적, pitch는 x축 rotation 누적
        yaw += mouseX * Sensitive * Time.deltaTime;
        pitch += -mouseY * Sensitive * Time.deltaTime;

        // x축 회전은 제한을 뒀음
        pitch = Mathf.Clamp(pitch, MIN_PITCH, MAX_PITCH);
    }

    // 카메라 상태가 FreeLook이면 그대로 이동
    // Aim이면 마지막에 받은 pitch, yaw를 사용해서 위치 고정
    private Vector3 CalculateCameraPosition()
    {
        float usePitch = pitch;
        float useYaw = yaw;

        if (cameraController.State.Current == CameraState.State.Aim)
        {
            usePitch = lockPitch;
            useYaw = lockYaw;
        }

        Quaternion rot = Quaternion.Euler(usePitch, useYaw, 0.0f);

        return ball.position + rot * offset;
    }

    private Vector3 ApplyCollision(Vector3 camPos)
    {
        // 카메라 위치에서 골프공으로 향하는 방향벡터
        Vector3 dir = camPos - ball.position;

        // MAX_DISTANCE 길이의 레이를 쏴서 레이어가
        // 장애물인 오브젝트에 충돌 했는지, 충돌한 위치는 어디인지 bool, RaycastHit hit.point 반환
        bool hasHit = Physics.Raycast(ball.position, dir,
            out RaycastHit hit, MAX_DISTANCE, LayerMask.GetMask("Obstacle"));

        // 골프공과 카메라의 거리를 구함
        float camDistance = Vector3.Distance(ball.position, camPos);

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

        transform.LookAt(ball.position);
    }

    private void DrawDebug()
    {
        Quaternion rot = Quaternion.Euler(pitch, yaw, 0.0f);

        Vector3 dir = rot * offset;

        Debug.DrawRay(ball.position, dir * MAX_DISTANCE, Color.red);
    }

    private bool IsUIInput()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}