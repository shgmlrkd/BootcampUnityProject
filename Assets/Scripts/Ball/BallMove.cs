using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BallMove : MonoBehaviour
{
    private GameObject indicator;

    private Ball ball;

    private Vector3 ballCenterPos = Vector3.zero;

    private Vector3 direction = Vector3.zero;

    private const float MAX_HOLD_GAGE = 30.0f;
    public float MaxHoldGage
    {
        get => MAX_HOLD_GAGE;
    }

    private float chargeSpeed = 8.0f;

    public float HoldGage { get; set; } = 0.0f;

    private float orbitDistance = 0.3f;

    private bool isAiming = false;
    private bool isCharging = false;

    private void Awake()
    {
        ball = GetComponent<Ball>();

        indicator = transform.Find("DirectionUI").gameObject;
    }

    private void Start()
    {
        // 스테이지 별 골프 공의 위치를 받아옴
        transform.position = GameManager.Instance.GetBallStartPos();

        indicator.SetActive(false);
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
            // 공이 움직이는 중에는 UI가 보이지 않게 처리
            if (indicator.activeSelf)
            { 
                indicator.SetActive(false);
            }

            return;
        }    

        // 마우스 처음 클릭 할 때
        if(Input.GetMouseButtonDown(0))
        {
            // 공 위치를 시작 지점으로 설정
            ballCenterPos = transform.position;
            indicator.SetActive(true);
            isAiming = true;
        }
        
        // 마우스 클릭 중
        if(Input.GetMouseButton(0) && isAiming)
        {
            // 게이지를 쌓음
            ChargePower();
        }

        // 마우스 클릭 땔 때
        if (Input.GetMouseButtonUp(0) && isAiming)
        {
            isAiming = false;
            indicator.SetActive(false);

            // 마우스 위치를 월드 위치로 변환 후 반환
            Vector3 clickEndPos = GetMouseWorldPosition();

            // 공이 나아갈 방향 벡터 (Y축 제외)
            direction = GetDirection(clickEndPos);
        }
    }

    private void LateUpdate()
    {
        // 공이 굴러도 지시계는 회전하지 않게 위치와 회전을 별도 관리
        if (isAiming && indicator.activeSelf)
        {
            UpdateIndicator();
        }
    }

    private void UpdateIndicator()
    {
        Vector3 currentMousePos = GetMouseWorldPosition();

        // 클릭 끝 시점의 위치에서 공의 중심 위치로 향하는 방향벡터
        Vector3 dir = ballCenterPos - currentMousePos;
        dir.y = 0.0f;

        // 충돌이 벗어났을 경우 계산이 불가
        if (dir == Vector3.zero)
        { 
            dir = Vector3.forward; // 기본 방향 설정
        }

        // 정규화
        dir = dir.normalized;

        // 자시계의 위치는 구한 방향 벡터에서 일정 거리를 더한 위치가 됨
        Vector3 orbitPos = transform.position + (dir * orbitDistance);

        indicator.transform.position = orbitPos;

        // Atan2를 쓰면 -180 ~ 180 까지 360도 각도를 라디안 값으로 구할 수 있다.
        // dir.x, dir.z 순으로 넣으면 z축 기준으로 각도를 구하게 되고
        // 오일러는 Degree 값을 받기때문에 라디안 값을 Degree로 변환 시켜주고
        // 자시계의 rotation에 값을 넣어준다.
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        indicator.transform.rotation = Quaternion.Euler(90.0f, angle, 90.0f);
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

        return ballCenterPos;
    }

    private Vector3 GetDirection(Vector3 targetPos)
    {
        Vector3 dir = ballCenterPos - targetPos;
        dir.y = 0.0f;

        return dir.normalized;
    }
}