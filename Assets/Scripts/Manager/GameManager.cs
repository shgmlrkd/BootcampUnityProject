using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Ball Ball;

    private const float MAX_STAGE = 3;

    // 공 처음 위치
    private Vector3[] ballStartPos = 
    { 
        new Vector3(0.0f, 3.5f, -8.0f),  // 스테이지 1
        new Vector3(-46.0f, 3.5f, 5.0f),  // 스테이지 2
        new Vector3(0.0f, 28.5f, 62.0f),  // 스테이지 3
    };

    public int stage { get; private set; } = 1;

    private void Awake()
    {
       if(Instance != null && Instance != this)
       {
            Destroy(gameObject);
            return;
       }

       Instance = this;
       DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StageClear();
        }
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
        if(stage == MAX_STAGE)
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
            return;
        }

        stage++;

        SceneManager.LoadScene(stage - 1);
    }
}