using System;
using UnityEngine;
using UnityEngine.UI;

public class SensitiveUI : MonoBehaviour
{
    public CameraController CamController;
    private Slider slider;

    private const float MAX_SENSITIVE = 400.0f;
    private const float MIN_SENSITIVE = 0.0f;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        slider.minValue = MIN_SENSITIVE;
        slider.maxValue = MAX_SENSITIVE;

        slider.value = CamController.Move.Sensitive;

        slider.onValueChanged.AddListener(ChangeSensitive);
    }

    private void ChangeSensitive(float value)
    {
        CamController.Move.Sensitive = value;
    }
}
