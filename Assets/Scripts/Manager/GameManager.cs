using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Ball Ball;

    // 공 처음 위치
    private Vector3[] ballStartPos = 
    { 
        new Vector3(0.0f, 3.5f, -8.0f),  // 스테이지 1
        new Vector3(-62.5f,3.5f,61.0f),  // 스테이지 2
        new Vector3(0.0f, 3.5f, 0.0f),  // 스테이지 3
        new Vector3(0.0f, 3.5f, 0.0f),  // 스테이지 4
        new Vector3(0.0f, 3.5f, 0.0f)   // 스테이지 5
    };

    public int stage { get; private set; } = 1;

    public bool IsGoal { get; private set; }

    private void Awake()
    {
       if(Instance != null && Instance != this)
       {
            Destroy(Instance);
            return;
       }

       Instance = this;
       DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        IsGoal = false;
    }

    public Vector3 GetBallStartPos()
    {
        return ballStartPos[stage - 1];
    }

    public void ResetBall()
    {
        Ball.ResetPosition(GetBallStartPos());
    }

    public void StageClear()
    {
        stage++;

        SceneManager.LoadScene(stage - 1);
        print("골인");
    }
}