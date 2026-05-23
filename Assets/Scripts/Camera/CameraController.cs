using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CameraMove Move { get; private set; }
    public CameraState State { get; private set; }

    private void Awake()
    {
        Move = GetComponent<CameraMove>();
        State = GetComponent<CameraState>();
    }
}