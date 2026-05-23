using UnityEngine;
using UnityEngine.EventSystems;

public class BallMove : MonoBehaviour
{
    private Ball ball;

    private LineRenderer lineRenderer;

    private Vector3 clickStartPos = Vector3.zero;
    private Vector3 clickEndPos = Vector3.zero;

    private Vector3 direction = Vector3.zero;

    private const float MAX_HOLD_GAGE = 15.0f;
    private const float AIM_LINE_LENGTH = 1.2f;

    private float chargeSpeed = 2.5f;

    public float HoldGage { get; set; } = 0.0f;

    private bool isAiming = false;
    private bool isCharging = false;

    private void Awake()
    {
        ball = GetComponent<Ball>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        // UI 누르면 골프 입력 막음
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (ball.State.IsMoving())
        {
            return;
        }    

        // 마우스 처음 클릭 할 때
        if(Input.GetMouseButtonDown(0))
        {
            // 공 위치를 시작 지점으로 설정
            clickStartPos = transform.position;
            isAiming = true;
        }
        
        // 마우스 클릭 중
        if(Input.GetMouseButton(0) && isAiming)
        {
            // 게이지를 쌓음
            ChargePower();
            // 공이 나아갈 방향을 라인으로 보여줌
            DrawLine();
        }

        // 마우스 클릭 땔 때
        if (Input.GetMouseButtonUp(0) && isAiming)
        {
            isAiming = false;
            lineRenderer.enabled = false;

            // 마우스 위치를 월드 위치로 변환 후 반환
            Vector3 clickEndPos = GetMouseWorldPosition();

            // 공이 나아갈 방향 벡터 (Y축 제외)
            direction = GetDirection(clickEndPos);
        }
    }

    private void FixedUpdate()
    {
        // 방향이 있고 에이밍이 끝났을 때
        if (direction != Vector3.zero && !isAiming)
        {
            ball.Rb.AddForce(direction * HoldGage, ForceMode.Impulse);

            direction = Vector3.zero; 
            HoldGage = 0.0f;
        }
    }

    private void ChargePower()
    {
        if (isCharging)
        {
            HoldGage += Time.deltaTime * chargeSpeed;

            if (HoldGage >= MAX_HOLD_GAGE)
            {
                HoldGage = MAX_HOLD_GAGE;
                isCharging = false;
            }
        }
        else
        {
            HoldGage -= Time.deltaTime * chargeSpeed;

            if (HoldGage <= 0.0f)
            {
                HoldGage = 0.0f;
                isCharging = true;
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }

        return clickStartPos;
    }

    private Vector3 GetDirection(Vector3 targetPos)
    {
        Vector3 dir = clickStartPos - targetPos;
        dir.y = 0.0f;

        return dir.normalized;
    }

    private void DrawLine()
    {
        // 마우스 클릭 중일 때 현재 마우스 위치를 ray로 월드 좌표 변환
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 currentMousePos = clickStartPos; // 기본값

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            currentMousePos = hit.point;
        }

        // 방향 계산
        Vector3 dir = (clickStartPos - currentMousePos).normalized;
        dir.y = 0.0f; // 수평 방향만

        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + dir * AIM_LINE_LENGTH);
    }
}