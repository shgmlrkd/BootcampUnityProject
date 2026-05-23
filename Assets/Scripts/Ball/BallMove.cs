using UnityEngine;

public class BallMove : MonoBehaviour
{
    private Ball ball;

    private Vector3 clickStartPos = Vector3.zero;
    private Vector3 clickEndPos = Vector3.zero;

    private Vector3 direction = Vector3.zero;

    private const float MAX_HOLD_GAGE = 15.0f;

    private float chargeSpeed = 2.5f;

    private float holdGage = 0.0f;
    private bool isAiming = false;

    private void Start()
    {
        ball = GetComponent<Ball>();
    }

    private void Update()
    {
        if(ball.State.IsMoving())
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
            holdGage += Time.deltaTime * chargeSpeed;

            if (holdGage > MAX_HOLD_GAGE)
            {
                holdGage = MAX_HOLD_GAGE;
            }
        }

        // 마우스 클릭 땔 때
        if (Input.GetMouseButtonUp(0) && isAiming)
        {
            // 마우스 위치를 월드 레이로 변환
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                clickEndPos = hit.point; 
                isAiming = false;
            }

            direction = (clickStartPos - clickEndPos).normalized;
            direction.y = 0.0f;
        }
    }

    private void FixedUpdate()
    {
        // 방향이 있고 에이밍이 끝났을 때
        if (direction != Vector3.zero && !isAiming)
        {
            ball.Rb.AddForce(direction * holdGage, ForceMode.Impulse);

            direction = Vector3.zero; 
            holdGage = 0.0f;
        }
    }
}