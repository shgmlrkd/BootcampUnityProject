using UnityEngine;

public class CameraState : MonoBehaviour
{
    public enum State
    {
        FreeLook,
        Aim,
        Follow
    }

    private CameraController cameraController;

    public State Current { get; private set; }

    private void Awake()
    {
        cameraController = GetComponent<CameraController>();

        Current = State.FreeLook;
    }

    public void Change(State next)
    {
        Current = next;
    }
}
