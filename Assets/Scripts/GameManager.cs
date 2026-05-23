using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
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

    public void StageClear()
    {
        IsGoal = true;
        print("골인");
    }
}